// Owners: Alexey Abretov, Nikolay Karlov

using System;

namespace QA.Core.Logger
{
    /// <summary>
    /// Базовый интерфейс сервиса, реализующего журналирование
    /// </summary>
    public interface ILogger : IDisposable
    {
        /// <summary>
        /// Журналирование сообщения об исключении c уровнем <see cref="F:EventLevel.Error"/>
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="exception">Экземпляр исключения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        void ErrorException(string message, Exception exception, params object[] parameters);

        /// <summary>
        /// Журналирование сообщения c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Info"/>
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="parameters">Дополнительные параметы</param>
        void Info(string message, params object[] parameters);

        /// <summary>
        /// Журналирование сообщения (отложенное вычисление) c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Info"/>
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        [Obsolete("Необходимо использовать метод с сигнатурой \"void Info(Func<string> message, params object[] parameters)\"")]
        void Info(Func<string, string> message, params object[] parameters);

        /// <summary>
        /// Журналирование сообщения (отложенное вычисление) c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Info"/>
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        void Info(Func<string> message, params object[] parameters);

        /// <summary>
        /// Журналирование сообщения с уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Debug"/>
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="parameters">Дополнительные параметы</param>
        void Debug(string message, params object[] parameters);

        /// <summary>
        /// Журналирование сообщения (отложенное вычисление) c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Debug"/>
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        [Obsolete("Необходимо использовать метод с сигнатурой \"void Debug(Func<string> message, params object[] parameters)\"")]
        void Debug(Func<string, string> message, params object[] parameters);

        /// <summary>
        /// Журналирование сообщения (отложенное вычисление) c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Debug"/>
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        void Debug(Func<string> message, params object[] parameters);

        /// <summary>
        /// Журналирует сообщения c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Error"/>
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="parameters">Дополнительные параметы</param>
        void Error(string message, params object[] parameters);

        /// <summary>
        /// Журналирование сообщения.
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        [Obsolete("Необходимо использовать метод с сигнатурой \"void Error(Func<string> message, params object[] parameters)\"")]
        void Error(Func<string, string> message, params object[] parameters);

        /// <summary>
        /// Журналирование сообщения (отложенное вычисление) c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Error"/>
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        void Error(Func<string> message, params object[] parameters);

        /// <summary>
        /// Журналирование сообщения c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Fatal"/>
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="exception">Экземпляр исключения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        void Fatal(string message, Exception exception, params object[] parameters);

        /// <summary>
        /// Журналирование сообщения c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Fatal"/>
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="parameters">Дополнительные параметы</param>
        void Fatal(string message, params object[] parameters);

        /// <summary>
        /// Журналирование сообщения (отложенное вычисление) c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Fatal"/>
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        [Obsolete("Необходимо использовать метод с сигнатурой \"void Fatal(Func<string> message, params object[] parameters)\"")]
        void Fatal(Func<string, string> message, params object[] parameters);

        /// <summary>
        /// Журналирование сообщения (отложенное вычисление) c уровнем <see cref="EventLevel"/>.<see cref="EventLevel.Fatal"/>
        /// </summary>
        /// <remarks>Лямбда-выражение не выполняется, если отключен данный уровень записи в лог</remarks>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="parameters">Дополнительные параметы</param>
        void Fatal(Func<string> message, params object[] parameters);

        /// <summary>
        /// Журналирование сообщения
        /// </summary>
        /// <param name="message">Делегат формирования сообщения</param>
        /// <param name="eventLevel">Уровень критичности сообщения</param>
        void Log(Func<string> message, EventLevel eventLevel);
    }
}
