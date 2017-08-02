using System;
using System.Linq;

namespace QA.Core.Logger
{
    /// <summary>
    /// Реализация журналирования на основе System.Diagnostics.Trace.WriteLine.
    /// </summary>
    public class NullLogger : ILogger
    {
        /// <summary>
        /// Журналирование сообщения об исключении c уровнем <see cref="F:EventLevel.Error"/>
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="exception">Экземпляр исключения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void ErrorException(string message, Exception exception, params object[] parameters)
        {
            var errorMsg = CreateErrorMessage(exception);
            WriteMessage("Error", message, errorMsg);
        }

        /// <summary>
        /// Журналирование сообщения c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Info"/>
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void Info(string message, params object[] parameters)
        {
            WriteMessage("Info", message);
        }

        /// <summary>
        /// Журналирование сообщения (отложенное вычисление) c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Info"/>
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        [Obsolete("Необходимо использовать метод с сигнатурой \"void Info(Func<string> message, params object[] parameters)\"")]
        public void Info(Func<string, string> message, params object[] parameters)
        {
            WriteMessage("Info", message(null));
        }

        /// <summary>
        /// Журналирование сообщения (отложенное вычисление) c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Info"/>
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void Info(Func<string> message, params object[] parameters)
        {
            WriteMessage("Info", message());
        }

        /// <summary>
        /// Журналирование сообщения с уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Debug"/>
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void Debug(string message, params object[] parameters)
        {
            WriteMessage("Debug", message);
        }

        /// <summary>
        /// Журналирование сообщения (отложенное вычисление) c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Debug"/>
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        [Obsolete("Необходимо использовать метод с сигнатурой \"void Debug(Func<string> message, params object[] parameters)\"")]
        public void Debug(Func<string, string> message, params object[] parameters)
        {
            WriteMessage("Debug", message(null));
        }

        /// <summary>
        /// Журналирование сообщения (отложенное вычисление) c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Debug"/>
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void Debug(Func<string> message, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Журналирует сообщения c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Error"/>
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void Error(string message, params object[] parameters)
        {
            WriteMessage("Error", message);
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
            WriteMessage("Error", message(null));
        }

        /// <summary>
        /// Журналирование сообщения (отложенное вычисление) c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Error"/>
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void Error(Func<string> message, params object[] parameters)
        {
            WriteMessage("Error", message());
        }

        /// <summary>
        /// Журналирование сообщения c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Fatal"/>
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void Fatal(string message, params object[] parameters)
        {
            WriteMessage("Fatal", message);
        }

        /// <summary>
        /// Журналирование сообщения c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Fatal"/>
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="exception">Экземпляр исключения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void Fatal(string message, Exception exception, params object[] parameters)
        {
            var errorMsg = CreateErrorMessage(exception);
            WriteMessage("Fatal", message, errorMsg);
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
            WriteMessage("Fatal", message(null));
        }

        /// <summary>
        /// Журналирование сообщения (отложенное вычисление) c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Fatal"/>
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        public virtual void Fatal(Func<string> message, params object[] parameters)
        {
            WriteMessage("Fatal", message());
        }

        /// <summary>
        /// Журналирование сообщения
        /// </summary>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="propertiesSetter">Делегат формирования параметров</param>
        /// <param name="eventLevel">Требуемый уровень критичности сообщения</param>
        public void Log(Func<string> message, Action<System.Collections.Generic.IDictionary<object, object>> propertiesSetter, EventLevel eventLevel)
        {
            WriteMessage(eventLevel.ToString(), message());
        }

        /// <summary>
        /// Журналирование сообщения
        /// </summary>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="eventLevel">Уровень критичности сообщения</param>
        public virtual void Log(Func<string> message, EventLevel eventLevel)
        {
            WriteMessage(eventLevel.ToString(), message());
        }

        /// <summary>
        /// Освобождение ресурсов
        /// </summary>
        public void Dispose() { }

        /// <summary>
        /// Запись сообщения в трейс-лог
        /// </summary>
        protected virtual void WriteMessage(string level, string message, string error = null)
        {
            System.Diagnostics.Trace.WriteLine("***************************************************************");
            System.Diagnostics.Trace.WriteLine($"{DateTime.Now} {level.ToUpper()}: {message} {error}");
            System.Diagnostics.Trace.WriteLine("***************************************************************");
        }

        private static string CreateErrorMessage(Exception exception)
        {
            var errorMsg = string.Join(Environment.NewLine, exception.Flat().Select(x => x.Message));
            return errorMsg;
        }
    }
}
