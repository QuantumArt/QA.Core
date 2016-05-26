// Owners: Alexey Abretov, Nikolay Karlov

using System;
using System.IO;
using log4net.Config;
using System.Diagnostics;
using System.Collections.Generic;
using QA.Core.Logger;
using log4net.Core;

namespace QA.Core
{
    /// <summary>
    /// Реализует журналирование на основе Log4Net
    /// </summary>
    public class Log4NetLogger : ILogger
    {
        /// <summary>
        /// Создает экземпляр логгера
        /// </summary>
        /// <param name="fileName">конфиг</param>
        /// <param name="loggerName">имя логгера</param>
        public Log4NetLogger(
            string fileName,
            string loggerName)
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(fileName));
            Logger = log4net.LogManager.GetLogger(loggerName);
        }

        /// <summary>
        /// Создает экземпляр логгера
        /// </summary>
        public Log4NetLogger()
        {
            Logger = log4net.LogManager.GetLogger(typeof(Log4NetLogger));
            XmlConfigurator.Configure();
        }

        /// <summary>
        /// Экзеппляр логгера
        /// </summary>
        protected log4net.ILog Logger
        {
            get;
            set;
        }

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
            try
            {
                Error(message, parameters);
                foreach (var ex in exception.Flat())
                {
                    Logger.Error(string.Format("{0}\n{1}\n{2}", ex.Message, ex.StackTrace, ex.Source), ex);
                }               
            }
            catch(Exception logEx)
            {
                Trace.WriteLine(logEx.Message);
            }
        }

        /// <summary>
        /// Журналирует сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        public void Info(
            string message,
            params object[] parameters)
        {
            try
            {
                if (parameters == null || parameters.Length == 0)
                {
                    Logger.Info(message);
                }
                else
                {
                    Logger.Info(string.Format(message, parameters));
                }
            }
            catch (Exception ex)
            {
                ErrorException("Ошибка во время логирования", ex);
            }
        }

        /// <summary>
        /// Журналирует сообщение (отложенное вычисление)
        /// </summary>
        /// <param name="message">Сообщение</param>
        public void Info(Func<string, string> message, params object[] parameters)
        {
            try
            {
                if (Logger.IsInfoEnabled)
                {
                    Info(message(string.Empty), parameters);
                }
            }
            catch (Exception ex)
            {
                ErrorException("Ошибка во время логирования", ex);
            }
        }

        /// <summary>
        /// Журналирует отладочную информацию
        /// </summary>
        /// <param name="message">Сообщение</param>
        public void Debug(
            string message,
            params object[] parameters)
        {
            try
            {
                if (parameters == null || parameters.Length == 0)
                {
                    Logger.Debug(message);
                }
                else
                {
                    Logger.Debug(string.Format(message, parameters));
                }
            }
            catch (Exception ex)
            {
                ErrorException("Ошибка во время логирования", ex);
            }
        }

        /// <summary>
        /// Журналирует отладочную информацию (отложенное вычисление)
        /// </summary>
        /// <param name="message">Сообщение</param>
        public void Debug(Func<string, string> message, params object[] parameters)
        {
            try
            {
                if (Logger.IsDebugEnabled)
                {
                    Debug(message(string.Empty), parameters);
                }
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
        public void Fatal(
            string message,
            Exception exception,
            params object[] parameters)
        {
            try
            {
                Fatal(message, parameters);
                foreach (var ex in exception.Flat())
                {
                    Logger.Fatal(string.Format("{0}\n{1}\n{2}", ex.Message, ex.StackTrace, ex.Source), ex);
                }
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
        public void Error(
            string message,
            params object[] parameters)
        {
            try
            {
                if (parameters == null || parameters.Length == 0)
                {
                    Logger.Error(message);
                }
                else
                {
                    Logger.Error(string.Format(message, parameters));
                }
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
        public void Fatal(
            string message,
            params object[] parameters)
        {
            try
            {
                if (parameters == null || parameters.Length == 0)
                {
                    Logger.Fatal(message);
                }
                else
                {
                    Logger.Fatal(string.Format(message, parameters));
                }
            }
            catch (Exception ex)
            {
                ErrorException("Ошибка во время логирования", ex);
            }
        }

        /// <summary>
        /// Журналирует сообщение (отложенное вычисление)
        /// </summary>
        /// <param name="message">Сообщение</param>
        public void Error(Func<string, string> message, params object[] parameters)
        {
            if (Logger.IsInfoEnabled)
            {
                try
                {
                    Error(message(string.Empty), parameters);
                }
                catch (Exception ex)
                {
                    ErrorException("Ошибка во время логирования", ex);
                }
            }
        }
        /// <summary>
        /// Журналирует сообщение (отложенное вычисление)
        /// </summary>
        /// <param name="message">Сообщение</param>
        public void Fatal(Func<string, string> message, params object[] parameters)
        {
            if (Logger.IsInfoEnabled)
            {
                try
                {
                    Fatal(message(string.Empty), parameters);
                }
                catch (Exception ex)
                {
                    ErrorException("Ошибка во время логирования", ex);
                }
            }
        }

        public void Log(Func<string> message, EventLevel eventLevel)
        {
            Log(message, null, eventLevel);
        }

        public void Log(Func<string> message, Action<IDictionary<object, object>> propertiesSetter, EventLevel eventLevel)
        {
            Level logLevel = null;
            var logger = Logger;
            switch (eventLevel)
            {
                case EventLevel.Debug:
                    logLevel = Level.Debug;
                    break;
                case EventLevel.Info:
                    logLevel = Level.Info;
                    break;
                case EventLevel.Warning:
                    logLevel = Level.Warn;
                    break;
                case EventLevel.Error:
                    logLevel = Level.Error;
                    break;
                case EventLevel.Fatal:
                    logLevel = Level.Fatal;
                    break;
                default:
                    logLevel = Level.Info;
                    break;
            }

            if (!logger.Logger.IsEnabledFor(logLevel))
                return;

            LoggingEventData data = new LoggingEventData();

            data.Level = logLevel;
            data.Message = message();
            data.TimeStamp = DateTime.Now;
            
            if (propertiesSetter != null)
            {
                Dictionary<object, object> dict = new Dictionary<object, object>();
                propertiesSetter(dict);
                data.Properties = new log4net.Util.PropertiesDictionary();
                foreach (var item in dict)
                {
                    data.Properties[item.Key.ToString()] = item.Value;
                }
            }

            LoggingEvent theEvent = new LoggingEvent(data);

            Logger.Logger.Log(theEvent);
        }

        /// <summary>
        /// Освобождение ресурсов
        /// </summary>
        public void Dispose()
        {
            Logger = null;
        }
    }
}
