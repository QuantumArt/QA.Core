// Owners: Alexey Abretov, Nikolay Karlov

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using log4net.Config;
using log4net.Core;

namespace QA.Core.Logger
{
    /// <summary>
    /// Реализация журналирования на основе Log4Net
    /// </summary>
    public class Log4NetLogger : ILogger
    {
        /// <summary>
        /// Экзеппляр логгера
        /// </summary>
        protected log4net.ILog Logger { get; set; }

        /// <summary>
        /// Инициализация экземпляра <see cref="Log4NetLogger"/>
        /// </summary>
        /// <param name="fileName">Путь к файлу конфигурации</param>
        /// <param name="loggerName">Имя логгера</param>
        public Log4NetLogger(string fileName, string loggerName)
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(fileName));
            Logger = log4net.LogManager.GetLogger(loggerName);
        }

        /// <summary>
        /// Инициализация экземпляра <see cref="Log4NetLogger"/>
        /// </summary>
        public Log4NetLogger()
        {
            Logger = log4net.LogManager.GetLogger(typeof(Log4NetLogger));
            XmlConfigurator.Configure();
        }

        /// <summary>
        /// Журналирование сообщения об исключении c уровнем <see cref="F:EventLevel.Error"/>
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="exception">Экземпляр исключения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void ErrorException(string message, Exception exception, params object[] parameters)
        {
            try
            {
                Error(message, parameters);
                foreach (var ex in exception.Flat())
                {
                    Logger.Error($"{ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}{ex.Source}", ex);
                }               
            }
            catch(Exception logEx)
            {
                Trace.WriteLine(logEx.Message);
            }
        }

        /// <summary>
        /// Журналирование сообщения c уровнем <see cref="F:EventLevel.Info"/>
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void Info(string message, params object[] parameters)
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
        /// Журналирование сообщения (отложенное вычисление) c уровнем <see cref="F:EventLevel.Info"/>
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        [Obsolete("Необходимо использовать метод с сигнатурой \"void Info(Func<string> message, params object[] parameters)\"")]
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
        /// Журналирование сообщения (отложенное вычисление) c уровнем <see cref="F:EventLevel.Info"/>
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void Info(Func<string> message, params object[] parameters)
        {
            try
            {
                if (Logger.IsInfoEnabled)
                {
                    Info(message(), parameters);
                }
            }
            catch (Exception ex)
            {
                ErrorException("Ошибка во время логирования", ex);
            }
        }

        /// <summary>
        /// Журналирование сообщения с уровнем <see cref="F:EventLevel.Debug"/>
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void Debug(string message, params object[] parameters)
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
        /// Журналирование сообщения (отложенное вычисление) c уровнем <see cref="F:EventLevel.Debug"/>
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        [Obsolete("Необходимо использовать метод с сигнатурой \"void Debug(Func<string> message, params object[] parameters)\"")]
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
        /// Журналирование сообщения (отложенное вычисление) c уровнем <see cref="F:EventLevel.Debug"/>
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void Debug(Func<string> message, params object[] parameters)
        {
            try
            {
                if (Logger.IsDebugEnabled)
                {
                    Debug(message(), parameters);
                }
            }
            catch (Exception ex)
            {
                ErrorException("Ошибка во время логирования", ex);
            }
        }

        /// <summary>
        /// Журналирование сообщения c уровнем <see cref="F:EventLevel.Error"/>
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void Error(string message, params object[] parameters)
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
        /// Журналирование сообщения (отложенное вычисление) c уровнем <see cref="F:EventLevel.Error"/>
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        [Obsolete("Необходимо использовать метод с сигнатурой \"void Error(Func<string> message, params object[] parameters)\"")]
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
        /// Журналирование сообщения (отложенное вычисление) c уровнем <see cref="F:EventLevel.Error"/>
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void Error(Func<string> message, params object[] parameters)
        {
            if (Logger.IsInfoEnabled)
            {
                try
                {
                    Error(message(), parameters);
                }
                catch (Exception ex)
                {
                    ErrorException("Ошибка во время логирования", ex);
                }
            }
        }

        /// <summary>
        /// Журналирование сообщения c уровнем <see cref="F:EventLevel.Fatal"/>
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="exception">Экземпляр исключения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void Fatal(string message, Exception exception, params object[] parameters)
        {
            try
            {
                Fatal(message, parameters);
                foreach (var ex in exception.Flat())
                {
                    Logger.Fatal($"{ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}{ex.Source}", ex);
                }
            }
            catch (Exception ex)
            {
                ErrorException("Ошибка во время логирования", ex);
            }
        }

        /// <summary>
        /// Журналирование сообщения c уровнем <see cref="F:EventLevel.Fatal"/>
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void Fatal(string message, params object[] parameters)
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
        /// Журналирование сообщения (отложенное вычисление) c уровнем <see cref="F:EventLevel.Fatal"/>
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        [Obsolete("Необходимо использовать метод с сигнатурой \"void Fatal(Func<string> message, params object[] parameters)\"")]
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

        /// <summary>
        /// Журналирование сообщения (отложенное вычисление) c уровнем <see cref="F:EventLevel.Fatal"/>
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void Fatal(Func<string> message, params object[] parameters)
        {
            if (Logger.IsInfoEnabled)
            {
                try
                {
                    Fatal(message(), parameters);
                }
                catch (Exception ex)
                {
                    ErrorException("Ошибка во время логирования", ex);
                }
            }
        }

        /// <summary>
        /// Журналирование сообщения
        /// </summary>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="eventLevel">Требуемый уровень критичности сообщения</param>
        public virtual void Log(Func<string> message, EventLevel eventLevel)
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

            var data = new LoggingEventData
            {
                Level = logLevel,
                Message = message(),
                TimeStamp = DateTime.Now
            };


            if (propertiesSetter != null)
            {
                var dict = new Dictionary<object, object>();
                propertiesSetter(dict);
                data.Properties = new log4net.Util.PropertiesDictionary();
                foreach (var item in dict)
                {
                    data.Properties[item.Key.ToString()] = item.Value;
                }
            }

            var theEvent = new LoggingEvent(data);

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
