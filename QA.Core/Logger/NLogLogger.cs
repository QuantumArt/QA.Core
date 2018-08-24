// Owners: Alexey Abretov, Nikolay Karlov

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using NLog;
using NLog.Config;
#pragma warning disable 1591

namespace QA.Core.Logger
{
    /// <summary>
    /// Реализация журналирования на основе NLog
    /// </summary>
    public class NLogLogger : ILogger
    {
        private readonly LogFactory _factory;
        private readonly Lazy<NLog.Logger> _logger;

        /// <summary>
        /// Экзеппляр логгера
        /// </summary>
        protected NLog.Logger CurrentLogger
        {
            get
            {
                try
                {
                    return _logger.Value;
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Инициализация экземпляра <see cref="NLogLogger"/>
        /// </summary>
        /// <param name="fileName">Путь к файлу конфигурации. Поиск производится в
        /// AppDomain.CurrentDomain.BaseDirectory и AppDomain.CurrentDomain.BaseDirectory\bin</param>
        public NLogLogger(string fileName) : this(fileName, null)
        {
        }

        /// <summary>
        /// Инициализация экземпляра <see cref="NLogLogger"/>
        /// </summary>
        /// <param name="fileName">Путь к файлу конфигурации. Поиск производится в
        /// AppDomain.CurrentDomain.BaseDirectory и AppDomain.CurrentDomain.BaseDirectory\bin</param>
        /// <param name="loggerName">Название лога</param>
        public NLogLogger(string fileName, string loggerName)
        {
            var path1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            if (!File.Exists(path1))
            {
                var path2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", fileName);

                if (!File.Exists(path2))
                {
                    Trace.WriteLine($"Configuration path is not found. Seached in locations: {path1} {path2}");
                    return;
                }
                path1 = path2;
            }

            try
            {
                _factory = new LogFactory(new XmlLoggingConfiguration(path1));
            }
            catch
            {
                _factory = new LogFactory();
            }

            _logger = new Lazy<NLog.Logger>(() => loggerName == null ? _factory.GetCurrentClassLogger() : _factory.GetLogger(loggerName), System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);
        }

        /// <summary>
        /// Инициализация экземпляра <see cref="NLogLogger"/>
        /// </summary>
        private NLogLogger() { }

        /// <summary>
        /// Журналирование сообщения об исключении c уровнем <see cref="F:EventLevel.Error"/>
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="exception">Экземпляр исключения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void ErrorException(string message, Exception exception, params object[] parameters)
        {
            if (CurrentLogger == null)
            {
                return;
            }

            try
            {
                Error(message, parameters);

                foreach (var ex in exception.Flat())
                {
                    CurrentLogger.Error(ex, ex.Message);
                }
            }
            catch (Exception logEx)
            {
                Trace.WriteLine(logEx.Message);
            }
        }

        /// <summary>
        /// Журналирование сообщения c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Info"/>
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void Info(string message, params object[] parameters)
        {
            if (CurrentLogger == null)
            {
                return;
            }

            try
            {
                if (parameters == null || parameters.Length == 0)
                {
                    CurrentLogger.Info(message);
                }
                else
                {
                    CurrentLogger.Info(string.Format(message, parameters));
                }
            }
            catch (Exception ex)
            {
                ErrorException("Ошибка во время логирования", ex);
            }
        }

        /// <summary>
        /// Журналирование сообщения (отложенное вычисление) c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Info"/>
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        [Obsolete("Необходимо использовать метод с сигнатурой \"void Info(Func<string> message, params object[] parameters)\"")]
        public virtual void Info(Func<string, string> message, params object[] parameters)
        {
            if (CurrentLogger == null)
            {
                return;
            }

            try
            {
                CurrentLogger.Info(() => message(string.Empty));
            }
            catch (Exception ex)
            {
                ErrorException("Ошибка во время логирования", ex);
            }
        }

        /// <summary>
        /// Журналирование сообщения (отложенное вычисление) c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Info"/>
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void Info(Func<string> message, params object[] parameters)
        {
            if (CurrentLogger == null)
            {
                return;
            }

            try
            {
                CurrentLogger.Info(() => message());
            }
            catch (Exception ex)
            {
                ErrorException("Ошибка во время логирования", ex);
            }
        }

        /// <summary>
        /// Журналирование сообщения с уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Debug"/>
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void Debug(string message, params object[] parameters)
        {
            if (CurrentLogger == null)
            {
                return;
            }

            try
            {
                if (parameters == null || parameters.Length == 0)
                {
                    CurrentLogger.Debug(message);
                }
                else
                {
                    CurrentLogger.Debug(string.Format(message, parameters));
                }
            }
            catch (Exception ex)
            {
                ErrorException("Ошибка во время логирования", ex);
            }
        }

        /// <summary>
        /// Журналирование сообщения (отложенное вычисление) c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Debug"/>
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        [Obsolete("Необходимо использовать метод с сигнатурой \"void Debug(Func<string> message, params object[] parameters)\"")]
        public virtual void Debug(Func<string, string> message, params object[] parameters)
        {
            if (CurrentLogger == null)
            {
                return;
            }

            try
            {
                CurrentLogger.Debug(() => message(string.Empty));
            }
            catch (Exception ex)
            {
                ErrorException("Ошибка во время логирования", ex);
            }
        }

        /// <summary>
        /// Журналирование сообщения (отложенное вычисление) c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Debug"/>
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void Debug(Func<string> message, params object[] parameters)
        {
            if (CurrentLogger == null)
            {
                return;
            }

            try
            {
                CurrentLogger.Debug(() => message());
            }
            catch (Exception ex)
            {
                ErrorException("Ошибка во время логирования", ex);
            }
        }

        /// <summary>
        /// Журналирует сообщения c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Error"/>
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void Error(string message, params object[] parameters)
        {
            if (CurrentLogger == null)
            {
                return;
            }

            if (parameters == null || parameters.Length == 0)
            {
                CurrentLogger.Log(LogLevel.Error, message);
            }
            else
            {
                CurrentLogger.Log(LogLevel.Error, string.Format(message, parameters));
            }
        }

        /// <summary>
        /// Журналирование сообщения.
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        [Obsolete("Необходимо использовать метод с сигнатурой \"void Error(Func<string> message, params object[] parameters)\"")]
        public void Error(Func<string, string> message, params object[] parameters)
        {
            if (CurrentLogger == null)
            {
                return;
            }

            try
            {
                CurrentLogger.Log(LogLevel.Error, () => message(string.Empty));
            }
            catch (Exception ex)
            {
                ErrorException("Ошибка во время логирования", ex);
            }
        }

        /// <summary>
        /// Журналирование сообщения (отложенное вычисление) c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Error"/>
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void Error(Func<string> message, params object[] parameters)
        {
            if (CurrentLogger == null)
            {
                return;
            }

            try
            {
                CurrentLogger.Log(LogLevel.Error, () => message());
            }
            catch (Exception ex)
            {
                ErrorException("Ошибка во время логирования", ex);
            }
        }

        /// <summary>
        /// Журналирование сообщения c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Fatal"/>
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="exception">Экземпляр исключения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void Fatal(string message, Exception exception, params object[] parameters)
        {
            if (CurrentLogger == null)
            {
                return;
            }

            try
            {
                Fatal(message, parameters);

                foreach (var ex in exception.Flat())
                {
                    CurrentLogger.Fatal(ex, ex.Message);
                }
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// Журналирование сообщения c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Fatal"/>
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void Fatal(string message, params object[] parameters)
        {
            if (CurrentLogger == null)
            {
                return;
            }

            if (parameters == null || parameters.Length == 0)
            {
                CurrentLogger.Log(LogLevel.Fatal, message);
            }
            else
            {
                CurrentLogger.Log(LogLevel.Fatal, string.Format(message, parameters));
            }
        }

        /// <summary>
        /// Журналирование сообщения (отложенное вычисление) c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Fatal"/>
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        [Obsolete("Необходимо использовать метод с сигнатурой \"void Fatal(Func<string> message, params object[] parameters)\"")]
        public void Fatal(Func<string, string> message, params object[] parameters)
        {
            if (CurrentLogger == null)
            {
                return;
            }

            try
            {
                CurrentLogger.Log(LogLevel.Fatal, () => message(string.Empty));
            }
            catch (Exception ex)
            {
                ErrorException("Ошибка во время логирования", ex);
            }
        }

        /// <summary>
        /// Журналирование сообщения (отложенное вычисление) c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Fatal"/>
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void Fatal(Func<string> message, params object[] parameters)
        {
            if (CurrentLogger == null)
            {
                return;
            }

            try
            {
                CurrentLogger.Log(LogLevel.Fatal, () => message());
            }
            catch (Exception ex)
            {
                ErrorException("Ошибка во время логирования", ex);
            }
        }

        /// <summary>
        /// Журналирование сообщения
        /// </summary>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="eventLevel">Уровень критичности сообщения</param>
        public void Log(Func<string> message, EventLevel eventLevel)
        {
            Log(message, null, eventLevel);
        }

        /// <summary>
        /// Журналирование сообщения
        /// </summary>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="propertiesSetter">Делегат формирования параметров</param>
        /// <param name="eventLevel">Требуемый уровень критичности сообщения</param>
        public void Log(Func<string> message, Action<IDictionary<object, object>> propertiesSetter, EventLevel eventLevel)
        {
            LogLevel logLevel = null;
            var logger = CurrentLogger;
            switch (eventLevel)
            {
                case EventLevel.Trace:
                    logLevel = LogLevel.Trace;
                    break;
                case EventLevel.Debug:
                    logLevel = LogLevel.Debug;
                    break;
                case EventLevel.Info:
                    logLevel = LogLevel.Info;
                    break;
                case EventLevel.Warning:
                    logLevel = LogLevel.Warn;
                    break;
                case EventLevel.Error:
                    logLevel = LogLevel.Error;
                    break;
                case EventLevel.Fatal:
                    logLevel = LogLevel.Fatal;
                    break;
                default:
                    logLevel = LogLevel.Info;
                    break;
            }

            if (!logger.IsEnabled(logLevel))
                return;

	        LogEventInfo theEvent;

			try
	        {
				theEvent = new LogEventInfo(logLevel, logger.Name, message());
	        }
	        catch (Exception ex)
	        {
		        theEvent = new LogEventInfo(logLevel, logger.Name,
		            $"{ex.GetType() .Name} при попытке выполнить лямбду логгирования: {ex.Message + (ex.InnerException == null ? "" : " " + ex.InnerException.Message)}");
	        }

            theEvent.TimeStamp = DateTime.Now;

            propertiesSetter?.Invoke(theEvent.Properties);

            logger.Log(theEvent);
        }

        /// <summary>
        /// Освобождение ресурсов
        /// </summary>
        public void Dispose()
        {
            _factory?.Dispose();
        }

        public void SetGlobalContextVariable(string key, string value)
        {
            GlobalDiagnosticsContext.Set(key, value);
        }

        public void RemoveGlobalContextVariable(string key)
        {
            GlobalDiagnosticsContext.Remove(key);
        }

        public string GetGlobalContextVariable(string key)
        {
            return GlobalDiagnosticsContext.Get(key);
        }
    }
}
