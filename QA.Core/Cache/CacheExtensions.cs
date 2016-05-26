using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime;
using System.Threading;
using QA.Core.Logger;
using System.Threading.Tasks;

namespace QA.Core.Cache
{
    /// <summary>
    /// расширения для классов кэширования
    /// </summary>
    public static class CacheExtensions
    {
        private static ConcurrentDictionary<string, object> _lockers = new ConcurrentDictionary<string, object>();
        private static readonly ILogger _logger = ObjectFactoryBase.Resolve<ILogger>();
        private const int TRYENTER_TIMEOUT_MS = 7000;
        public static ConcurrentDictionary<string, object> Lockers
        {
            get
            {
                return _lockers;
            }
        }


        private static readonly bool _providerType = ObjectFactoryBase.Resolve<ICacheProvider>().GetType() == typeof(VersionedCacheProvider3);
        private static readonly bool _vProviderType = ObjectFactoryBase.Resolve<IVersionedCacheProvider>().GetType() == typeof(VersionedCacheProvider3);
        
        
        /// <summary>
        /// Потокобезопасно берет объект из кэша, если его там нет, то вызывает функцию для получения данных
        /// и кладет результат в кэш
        /// </summary>
        /// <typeparam name="T">тип объектов в кэше</typeparam>
        /// <param name="provider">провайдер кэша</param>
        /// <param name="key">тэг, в общем случае представляет имя класса сервиса + имя метода + список параметров</param>
        /// <param name="expiration">время жизни в кэше</param>
        /// <param name="getData">функция для получения данных, если объектов кэше нет. нужно использовать анонимный делегат</param>
        /// <returns>закэшированне данные, если они присутствуют в кэше или результат выполнения функции</returns>
        public static T GetOrAdd<T>(this ICacheProvider provider, string key, TimeSpan expiration, Func<T> getData)
        {
            var supportCallbacks = _providerType;
            object result = provider.Get(key);
            object deprecatedResult = null;

            if (result == null)
            {
                object localLocker = _lockers.GetOrAdd(key, new object());

                bool lockTaken = false;
                try
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();


                    if (supportCallbacks)
                    {
                        // проверяем, что есть предыдущее значение
                        deprecatedResult = provider.Get(VersionedCacheProvider3.CalculateDeprecatedKey(key));

                        if (deprecatedResult != null)
                        {
                            // если есть, то обновлять данные будет только 1 поток
                            Monitor.TryEnter(localLocker, ref lockTaken);
                        }
                        else
                        {
                            Monitor.TryEnter(localLocker, TRYENTER_TIMEOUT_MS, ref lockTaken);
                        }
                    }
                    else
                    {
                        Monitor.TryEnter(localLocker, TRYENTER_TIMEOUT_MS, ref lockTaken);
                    }

                    if (lockTaken)
                    {
                        result = provider.Get(key);

                        var time1 = sw.ElapsedMilliseconds;

                        if (result == null)
                        {
                            result = getData();
                            sw.Stop();
                            var time2 = sw.ElapsedMilliseconds;

                            CheckPerformance(key, time1, time2);

                            if (result != null)
                            {
                                provider.Set(key, result, expiration);
                                if (supportCallbacks && deprecatedResult != null)
                                {
                                    // если был устаревший объект в кеше, то удалим его
                                    provider.Invalidate(VersionedCacheProvider3.CalculateDeprecatedKey(key));
                                }
                            }
                        }
                    }
                    else
                    {
                        if (supportCallbacks && deprecatedResult != null)
                        {
                            return Convert<T>(deprecatedResult);
                        }

                        var time1 = sw.ElapsedMilliseconds;
                        _logger.Log(() => string.Format("Долгое нахождение в ожидании обновления кэша {1} ms, ключ: {0} ", key, time1), EventLevel.Warning);

                        result = getData();

                        sw.Stop();
                        var time2 = sw.ElapsedMilliseconds;

                        CheckPerformance(key, time1, time2, reportTime1: false);

                        if (result != null)
                        {
                            provider.Set(key, result, expiration);

                            if (supportCallbacks && deprecatedResult != null)
                            {
                                provider.Invalidate(VersionedCacheProvider3.CalculateDeprecatedKey(key));
                            }
                        }
                    }
                }
                finally
                {
                    if (lockTaken)
                    {
                        Monitor.Exit(localLocker);
                    }
                }
            }
            return Convert<T>(result);
        }

        /// <summary>
        /// Потокобезопасно берет объект из кэша, если его там нет, то вызывает асинхронную функцию для получения данных
        /// и кладет результат в кэш
        /// </summary>
        /// <typeparam name="T">тип объектов в кэше</typeparam>
        /// <param name="provider">провайдер кэша</param>
        /// <param name="key">тэг, в общем случае представляет имя класса сервиса + имя метода + список параметров</param>
        /// <param name="expiration">время жизни в кэше</param>
        /// <param name="getData">функция для получения данных, если объектов кэше нет. нужно использовать асинхронный анонимный делегат</param>
        /// <returns>закэшированне данные, если они присутствуют в кэше или результат выполнения функции</returns>
        public static async Task<T> GetOrAddAsync<T>(this ICacheProvider provider, string key, TimeSpan expiration, Func<Task<T>> getData)
        {
            var supportCallbacks = _providerType;
            object result = provider.Get(key);
            object deprecatedResult = null;

            if (result == null)
            {
                object localLocker = _lockers.GetOrAdd(key, new object());

                bool lockTaken = false;
                try
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();


                    if (supportCallbacks)
                    {
                        // проверяем, что есть предыдущее значение
                        deprecatedResult = provider.Get(VersionedCacheProvider3.CalculateDeprecatedKey(key));

                        if (deprecatedResult != null)
                        {
                            // если есть, то обновлять данные будет только 1 поток
                            Monitor.TryEnter(localLocker, ref lockTaken);
                        }
                        else
                        {
                            Monitor.TryEnter(localLocker, TRYENTER_TIMEOUT_MS, ref lockTaken);
                        }
                    }
                    else
                    {
                        Monitor.TryEnter(localLocker, TRYENTER_TIMEOUT_MS, ref lockTaken);
                    }

                    if (lockTaken)
                    {
                        result = provider.Get(key);

                        var time1 = sw.ElapsedMilliseconds;

                        if (result == null)
                        {
                            result = await getData().ConfigureAwait(false);
                            sw.Stop();
                            var time2 = sw.ElapsedMilliseconds;

                            CheckPerformance(key, time1, time2, false);

                            if (result != null)
                            {
                                provider.Set(key, result, expiration);
                                if (supportCallbacks && deprecatedResult != null)
                                {
                                    // если был устаревший объект в кеше, то удалим его
                                    provider.Invalidate(VersionedCacheProvider3.CalculateDeprecatedKey(key));
                                }
                            }
                        }
                    }
                    else
                    {
                        if (supportCallbacks && deprecatedResult != null)
                        {
                            return Convert<T>(deprecatedResult);
                        }

                        var time1 = sw.ElapsedMilliseconds;
                        _logger.Error("Долгое нахождение в ожидании обновления кэша {1} ms, ключ: {0} ", key, time1);

                        result = await getData().ConfigureAwait(false);

                        sw.Stop();
                        var time2 = sw.ElapsedMilliseconds;

                        CheckPerformance(key, time1, time2, false);

                        if (result != null)
                        {
                            provider.Set(key, result, expiration);
                        }
                    }
                }
                finally
                {
                    if (lockTaken)
                    {
                        Monitor.Exit(localLocker);
                    }
                }
            }
            return Convert<T>(result);
        }

        private static T Convert<T>(object result)
        {
            return result == null ? default(T) : (T)result;
        }

        private static void CheckPerformance(string key, long time1, long time2, bool reportTime1 = true)
        {
            var elapsed = time2 - time1;
            if (elapsed > 5000)
            {
                _logger.Log(() => string.Format("Долгое получение данных время: {0} мс, ключ: {1}, time1: {2}, time2: {3}",
                    elapsed, key, time1, time2), EventLevel.Warning);
            }
            if (reportTime1 && time1 > 1000)
            {
                _logger.Log(() => string.Format("Долгая проверка кеша: {0} мс, ключ: {1}",
                    time1, key), EventLevel.Warning);
            }
        }


        /// <summary>
        /// Потокобезопасно берет объект из кэша, если его там нет, то вызывает функцию для получения данных
        /// и кладет результат в кэш
        /// </summary>
        /// <typeparam name="T">тип объектов в кэше</typeparam>
        /// <param name="provider">провайдер кэша</param>
        /// <param name="key">тэг, в общем случае представляет имя класса сервиса + имя метода + список параметров</param>
        /// <param name="tags">список зависимых контентов</param>
        /// <param name="expiration">время жизни в кэше</param>
        /// <param name="getData">функция для получения данных, если объектов кэше нет. нужно использовать анонимный делегат</param>
        /// <returns>закэшированне данные, если они присутствуют в кэше или результат выполнения функции</returns>
        public static T GetOrAdd<T>(this IVersionedCacheProvider provider, string key, string[] tags, TimeSpan expiration, Func<T> getData)
        {
            var supportCallbacks = _vProviderType;
            object result = provider.Get(key, tags);
            object deprecatedResult = null;
            if (result == null)
            {
                object localLocker = _lockers.GetOrAdd(key, new object());
                bool lockTaken = false;
                try
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();

                    if (supportCallbacks)
                    {
                        // проверяем, что есть предыдущее значение
                        deprecatedResult = provider.Get(VersionedCacheProvider3.CalculateDeprecatedKey(key));

                        if (deprecatedResult != null)
                        {
                            Monitor.TryEnter(localLocker, ref lockTaken);
                        }
                        else
                        {
                            Monitor.TryEnter(localLocker, TRYENTER_TIMEOUT_MS, ref lockTaken);
                        }
                    }
                    else
                    {
                        Monitor.TryEnter(localLocker, TRYENTER_TIMEOUT_MS, ref lockTaken);
                    }

                    if (lockTaken)
                    {
                        result = provider.Get(key, tags);
                        var time1 = sw.ElapsedMilliseconds;
                        if (result == null)
                        {
                            DateTime startT = DateTime.Now;
                            result = getData();
                            sw.Stop();
                            var time2 = sw.ElapsedMilliseconds;

                            CheckPerformance(key, time1, time2);

                            if (result != null)
                            {
                                provider.Add(result, key, tags, expiration);
                                if (supportCallbacks && deprecatedResult != null)
                                {
                                    provider.Invalidate(VersionedCacheProvider3.CalculateDeprecatedKey(key));
                                }
                            }
                        }
                    }
                    else
                    {
                        if (supportCallbacks && deprecatedResult != null)
                        {
                            return Convert<T>(deprecatedResult);
                        }

                        var time1 = sw.ElapsedMilliseconds;
                        _logger.Log(() => string.Format("Долгое нахождение в ожидании обновления кэша {1} ms, ключ: {0} ", key, time1),
                            EventLevel.Warning);

                        result = getData();
                        sw.Stop();
                        var time2 = sw.ElapsedMilliseconds;

                        CheckPerformance(key, time1, time2);

                        if (result != null)
                        {
                            provider.Add(result, key, tags, expiration);
                            if (supportCallbacks && deprecatedResult != null)
                            {
                                provider.Invalidate(VersionedCacheProvider3.CalculateDeprecatedKey(key));
                            }
                        }
                    }
                }
                finally
                {
                    if (lockTaken)
                    {
                        Monitor.Exit(localLocker);
                    }
                }
            }

            return Convert<T>(result);
        }
    }
}
