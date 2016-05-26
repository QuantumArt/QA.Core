// Owners: Alexey Abretov, Nikolay Karlov

using System;
using System.IO;
using NLog;
using NLog.Config;
using System.Diagnostics;
using QA.Core.Logger;
using System.Collections.Generic;

namespace QA.Core
{
    /// <summary>
    /// Реализует журналирование на основе Nlog
    /// </summary>
    public class NLogLogger : ILogger
    {
        private LogFactory _factory;

        /// <summary>
        /// Инициализирует экземпляр журнала
        /// </summary>
        /// <param name="fileName">Путь к файлу конфигурации. Поиск производится в
        /// AppDomain.CurrentDomain.BaseDirectory и AppDomain.CurrentDomain.BaseDirectory\bin</param>
        public NLogLogger(
            string fileName)
        {
            var path1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            if (!File.Exists(path1))
            {
                var path2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", fileName);

                if (!File.Exists(path2))
                {
                    Trace.WriteLine(string.Format("Configuration path is not found. Seached in locations: {0} {1}", path1, path2));
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
        }

        /// <summary>
        /// Инициализирует экземпляр журнала. Скрытый.
        /// </summary>
        private NLogLogger()
        {
        }

        /// <summary>
        /// Экзеппляр журнала
        /// </summary>
        protected NLog.Logger Logger
        {
            get
            {
                try
                {
                    return _factory.GetCurrentClassLogger();
                }
                catch
                {
                    return null;
                }
            }
        }

        #region logging
        /// <summary>
        /// Журналирует информацию об исключении
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="exception">Экземпляр исключения</param>
        public void ErrorException(
            string message,
            Exception exception,
            params object[] parameters)
        {
            if (Logger == null)
            {
                return;
            }

            try
            {
                Error(message, parameters);

                foreach (var ex in exception.Flat())
                {
                    Logger.ErrorException(ex.Message, ex);
                }
            }
            catch (Exception logEx)
            {
                Trace.WriteLine(logEx.Message);
            }
        }

        /// <summary>
        /// Журналирует сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        public virtual void Info(
            string message,
            params object[] parameters)
        {
            if (Logger == null)
            {
                return;
            }

            try
            {
                Logger.Info(string.Format(message, parameters));
            }
            catch (Exception ex)
            {
                ErrorException("Ошибка во время логирования", ex);
            }
        }

        /// <summary>
        /// Журналирует сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        public virtual void Info(
           Func<string, string> message,
            params object[] parameters)
        {
            if (Logger == null)
            {
                return;
            }

            try
            {
                Logger.Info(() => message(string.Empty));
            }
            catch (Exception ex)
            {
                ErrorException("Ошибка во время логирования", ex);
            }
        }

        /// <summary>
        /// Журналирует сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        public virtual void Debug(
            string message,
            params object[] parameters)
        {
            if (Logger == null)
            {
                return;
            }

            try
            {
                Logger.Debug(string.Format(message, parameters));
            }
            catch (Exception ex)
            {
                ErrorException("Ошибка во время логирования", ex);
            }
        }

        /// <summary>
        /// Журналирует сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        public virtual void Debug(
            Func<string, string> message,
            params object[] parameters)
        {
            if (Logger == null)
            {
                return;
            }

            try
            {
                Logger.Debug(() => message(string.Empty));
            }
            catch (Exception ex)
            {
                ErrorException("Ошибка во время логирования", ex);
            }
        }

        /// <summary>
        /// Журналирует фатальную ошибку
        /// </summary>
        /// <param name="errors">Экземпляр ошибки</param>
        public virtual void Fatal(
            string message,
            Exception exception,
            params object[] parameters)
        {
            if (Logger == null)
            {
                return;
            }

            try
            {
                Fatal(message, parameters);

                foreach (var ex in exception.Flat())
                {
                    Logger.FatalException(ex.Message, ex);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Журналирует сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        public void Error(string message, params object[] args)
        {
            if (args == null || args.Length == 0)
            {
                Logger.Log(LogLevel.Error, message);
            }
            else
            {
                Logger.Log(LogLevel.Error, string.Format(message, args));
            }
        }

        /// <summary>
        /// Журналирует сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        public void Error(Func<string, string> message, params object[] args)
        {
            try
            {
                Logger.Log(LogLevel.Error, () => message(string.Empty));
            }
            catch (Exception ex)
            {
                ErrorException("Ошибка во время логирования", ex);
            }
        }

        /// <summary>
        /// Журналирует сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        public void Fatal(string message, params object[] args)
        {
            if (args == null || args.Length == 0)
            {
                Logger.Log(LogLevel.Fatal, message);
            }
            else
            {
                Logger.Log(LogLevel.Fatal, string.Format(message, args));
            }
        }

        /// <summary>
        /// Журналирует сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        public void Fatal(Func<string, string> message, params object[] args)
        {
            try
            {
                Logger.Log(LogLevel.Fatal, () => message(string.Empty));
            }
            catch (Exception ex)
            {
                ErrorException("Ошибка во время логирования", ex);
            }
        }
        #endregion

        /// <summary>
        /// Освобождение ресурсов
        /// </summary>
        public void Dispose()
        {
            if (_factory != null)
            {
                _factory.Dispose();
            }
        }

        public void Log(Func<string> message, Action<IDictionary<object, object>> propertiesSetter, EventLevel eventLevel)
        {
            LogLevel logLevel = null;
            var logger = Logger;
            switch (eventLevel)
            {
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

            if (logger.IsEnabled(logLevel))
                return;

            LogEventInfo theEvent = new LogEventInfo(logLevel, logger.Name, message());

            if (propertiesSetter != null)
                propertiesSetter(theEvent.Properties);

            logger.Log(theEvent);
        }

        public void Log(Func<string> message, EventLevel eventLevel)
        {
            Log(message, null, eventLevel);
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
