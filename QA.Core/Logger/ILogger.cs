// Owners: Alexey Abretov, Nikolay Karlov

using System;
using System.Collections.Generic;
using QA.Core.Logger;

namespace QA.Core
{
    /// <summary>
    /// Описывает контракт журналирования
    /// </summary>
    public interface ILogger : IDisposable
    {
        /// <summary>
        /// Журналирует информацию об исключении
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="exception">Экземпляр исключения</param>
        void ErrorException(
            string message,
            Exception exception,
            params object[] parameters);

        /// <summary>
        /// Журналирует сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        void Info(
            string message,
            params object[] parameters);

        /// <summary>
        /// Журналирует сообщение (отложенное вычисление).
        /// Лямбда-выражение не выполняется, если отключен данный уровень записи в лог
        /// </summary>
        /// <param name="message">Сообщение</param>
        void Info(Func<string, string> message, params object[] parameters);

        /// <summary>
        /// Журналирует отладочную информацию
        /// </summary>
        /// <param name="message">Сообщение</param>
        void Debug(
            string message,
            params object[] parameters);

        /// <summary>
        /// Журналирует отладочную информацию (отложенное вычисление). 
        /// Лямбда-выражение не выполняется, если отключен данный уровень записи в лог
        /// </summary>
        /// <param name="message">Сообщение</param>
        void Debug(
            Func<string, string> message,
            params object[] parameters);

        /// <summary>
        /// Журналирует фатальную ошибку
        /// </summary>
        /// <param name="errors">Экземпляр ошибки</param>
        void Fatal(
            string message,
            Exception exception,
            params object[] parameters);

        /// <summary>
        /// Журналирует сообщение.
        /// Лямбда-выражение не выполняется, если отключен данный уровень записи в лог
        /// </summary>
        /// <param name="message">Сообщение</param>
        void Error(
            Func<string, string> message,
           params object[] parameters);

        /// <summary>
        /// Журналирует сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        void Error(
           string message,
           params object[] parameters);

        /// <summary>
        /// Журналирует сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        void Fatal(
           string message,
           params object[] parameters);

        /// <summary>
        /// Журналирует сообщение.
        /// Лямбда-выражение не выполняется, если отключен данный уровень записи в лог
        /// </summary>
        /// <param name="message">Сообщение</param>
        void Fatal(
           Func<string, string> message,
           params object[] parameters);

        ///// <summary>
        ///// Добавление сообщения с долполнительными параметрами
        ///// </summary>
        ///// <param name="message"></param>
        ///// <param name="propertiesSetter"></param>
        ///// <param name="eventLevel"></param>
        //void Log(Func<string> message, Action<IDictionary<object, object>> propertiesSetter, EventLevel eventLevel);

        /// <summary>
        /// Добавление сообщения
        /// </summary>
        /// <param name="message"></param>
        /// <param name="propertiesSetter"></param>
        /// <param name="eventLevel"></param>
        void Log(Func<string> message, EventLevel eventLevel);
    }
}
