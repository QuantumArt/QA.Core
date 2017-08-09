using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using QA.Core;
using QA.Core.Logger;
using QA.Core.Performance;

namespace QA.Core.Performance
{
    /// <summary>
    /// Класс, предоставляющий возможность прекращать обращения к функционалу на некоторое время на основе статистики работы этого функционала. 
    /// Класс потокобезопасный, должен быть синглтоном.
    /// Если работа функционала заблокирована, то бросается исключение OperationCancelledException
    /// </summary>
    public class RapidFailProtector
    {
        private const int _capacity = 4096;
        ConcurrentBuffer<ExecutionResult> _queue = new ConcurrentBuffer<ExecutionResult>(_capacity);

        private string _systemName;
        private RapidFailConfig _config;
        private int _activeRequests = 0;
        private ILogger _logger;
        private bool _isInLockedState;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="systemName">Имя системы</param>
        /// <param name="config">конфигурация</param>
        public RapidFailProtector(string systemName, RapidFailConfig config)
        {
            _systemName = systemName;
            _config = config;
            _isInLockedState = false;
            _logger = ObjectFactoryBase.Resolve<ILogger>();
        }


        /// <summary>
        /// Выполнить действие с учетом статистики
        /// </summary>
        /// <typeparam name="T">Возвращаемый тип</typeparam>
        /// <param name="func">Функция, которая вызывается</param>
        /// <returns></returns>
        public T Do<T>(Func<T> func)
        {
            if (!_config.IsEnabled)
            {
                return func();
            }

            var now = DateTime.Now;

            CheckHealth(now, log: _config.Log);

            var sw = new Stopwatch();

            try
            {
                T result;
                try
                {
                    Interlocked.Increment(ref _activeRequests);
                    sw.Start();



                    result = func();
                    Volatile.Write(ref _isInLockedState, false);
                }
                finally
                {
                    sw.Stop();
                    Interlocked.Decrement(ref _activeRequests);
                }

                _queue.Enqueue(new ExecutionResult(sw.ElapsedMilliseconds, now, false));
                return result;

            }
            catch
            {
                _queue.Enqueue(new ExecutionResult(sw.ElapsedMilliseconds, now, true));

                Volatile.Write(ref _isInLockedState, true);

                throw;
            }
        }

        /// <summary>
        /// Выполнить действие асинхронно
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public async Task<T> DoAsync<T>(Func<Task<T>> func)
        {
            if (!_config.IsEnabled)
                return await func();

            var now = DateTime.Now;

            CheckHealth(now, log: _config.Log);

            var sw = new Stopwatch();
            try
            {
                T result;
                try
                {
                    Interlocked.Increment(ref _activeRequests);
                    sw.Start();

                    result = await func();
                    Volatile.Write(ref _isInLockedState, false);
                }
                finally
                {
                    sw.Stop();
                    Interlocked.Decrement(ref _activeRequests);
                }

                _queue.Enqueue(new ExecutionResult(sw.ElapsedMilliseconds, now, false));
                return result;

            }
            catch
            {
                _queue.Enqueue(new ExecutionResult(sw.ElapsedMilliseconds, now, true));
                Volatile.Write(ref _isInLockedState, true);
                throw;
            }
        }

        /// <summary>
        /// Выполнить действие
        /// </summary>
        /// <param name="func"></param>
        public void Do(Action func)
        {
            Do<object>(() =>
            {
                func();
                return null;
            });
        }

        /// <summary>
        /// Текущее состояние (не производится проверка статистики)
        /// </summary>
        /// <returns></returns>
        public bool IsLocked()
        {
            if (this._config.IsEnabled == false)
                return true;

            return Volatile.Read(ref _isInLockedState);
        }

        /// <summary>
        /// Принудительная проверка того, что защищаемый функционал работает. Возвращает true, если функционал заблокирован (т.е. не работает)
        /// </summary>
        /// <returns></returns>
        public bool CheckIsLocked()
        {
            return !(_config.IsEnabled && CheckHealth(DateTime.Now,
                throwOnError: false,
                log: false));
        }

        /// <summary>
        /// Быстрая проверка того, что защищаемый функционал работает. Возвращает true, если функционал заблокирован (т.е. не работает).
        /// Выполняетсчя быстро, так как используется последнее состояние системы
        /// </summary>
        /// <returns></returns>
        public bool CheckIsLockedFast()
        {
            if (_config.IsEnabled)
            {
                DateTime date = DateTime.Now;
                var latest = _queue.Latest;

                if (latest != null && latest.Time + _config.TimeInterval >= date)
                {
                    return Volatile.Read(ref _isInLockedState);
                }

                return !CheckHealth(DateTime.Now, throwOnError: false, log: false);
            }

            return false;
        }

        #region private methods
        private bool CheckHealth(DateTime now, bool throwOnError = true, bool log = true)
        {
            int maxErrorCount = _config.MaxErrorCount;
            TimeSpan interval = _config.TimeInterval;
            long threshold = _config.TimeoutThreshold;
            double thresholdRatio = _config.ThresholdRatio;
            int maxActiveRequestsCount = _config.MaxActiveRequestsCount;
            int minActiveRequestsCount = _config.MinActiveRequestsCount;
            int minRequestsCount = _config.MinRequestsCount;
            var activeRequests = Volatile.Read(ref _activeRequests);
            double percentage = -1;
            int failed = -1;

            // проверка по количеству одновременных транзакций
            if (activeRequests > _config.MaxActiveRequestsCount)
            {
                _logger.Info(_ => string.Format("RapidFailProtection {0} active transactions: {1} max: {2}",
                    _systemName, activeRequests, maxActiveRequestsCount));

                // для того, чтобы очередь вытеснялась, добавляем значение
                _queue.Enqueue(new ExecutionResult(5, now, false, false));
                Volatile.Write(ref _isInLockedState, true);

                if (throwOnError)
                    throw new OperationCancelledException(string.Format("Обработка запроса к сервису {0} была прекращена из-за превышения допустимого количества одновременных транзакций. Active: {1}, max: {2}",
                        _systemName,
                        activeRequests,
                        _config.MaxActiveRequestsCount));

                return false;
            }


            // проверка по кол-ву ошибок
            var all = _queue
                .Where(x => x.Time < now && x.Time >= now - interval)
                .ToList();

            var errorsCount = all.Where(x => x.IsError).Count();

            if (errorsCount > maxErrorCount)
            {
                _logger.Info(_ => string.Format("RapidFailProtection {0} errors: {1} max: {2}", _systemName, errorsCount, maxErrorCount));
                Volatile.Write(ref _isInLockedState, true);

                if (throwOnError)
                    throw new OperationCancelledException(string.Format("Обработка запроса к сервису {0} была прекращена из-за превышения допустимого количестка ошибок (current: {1}, max: {2}).",
                        _systemName,
                        errorsCount,
                        maxErrorCount));

                return false;
            }


            // проверка по временам отклика
            failed = all
                .Where(x => x.TimeOut > threshold)
                .Count();

            percentage = failed / (double)all.Count;

            if (all.Count > 0 && all.Count >= minRequestsCount
                            && activeRequests >= minActiveRequestsCount)
            {
                if (percentage > thresholdRatio)
                {
                    _logger.Info(string.Format("RapidFailProtection {0} too much timeouts: {1}, thresholdRatio: {2}, active requests: {3}, minRequestsCount {4}",
                        _systemName, 
                        percentage, 
                        thresholdRatio, 
                        activeRequests, 
                        minRequestsCount));

                    Volatile.Write(ref _isInLockedState, true);

                    if (throwOnError)
                        throw new OperationCancelledException(string.Format("Обработка запроса к сервису {0} была прекращена из-за превышения допустимой доли долгих ответов.Текущая величина: {1} Допустимо не более: {2} при пороге {3} ms",
                            _systemName, 
                            percentage, 
                            thresholdRatio, 
                            threshold));

                    return false;
                }
            }

            if (log)
            {
                _logger.Info(_ => ("RapidFailProtection OK " + ObjectDumper.DumpObject(
                           new
                           {
                               activeRequests,
                               percentage,
                               failed,
                               errorsCount,
                               allCount = all.Count
                           })));
            }

            return true;
        }
        #endregion

        #region nested class
        sealed class ExecutionResult
        {
            public ExecutionResult(long timeout, DateTime time, bool isError, bool isTurned = false)
            {
                Time = time;
                TimeOut = timeout;
                IsError = isError;
                IsTurned = isTurned;
            }

            public readonly long TimeOut;
            public readonly DateTime Time;
            public readonly bool IsError;
            public readonly bool IsTurned;
        }
        #endregion
    }
}
