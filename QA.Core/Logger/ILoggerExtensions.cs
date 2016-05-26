
using System;
using System.Runtime.CompilerServices;
using QA.Core.Logger;
namespace QA.Core
{
    /// <summary>
    /// Уровень критичности сообщения
    /// </summary>
    public static class ILoggerExtensions
    {
        /// <summary>
        /// Логирование при условии, что уровень журналирования включен. В текст сообщения включено имя метода, из оторого вызывается логирование
        /// </summary>
        /// <param name="logger">логгер</param>
        /// <param name="msgFactory">функция, которая возвраащет сообщение для логирования</param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public static ILogger LogInfo(this ILogger logger, Func<string> msgFactory, [CallerMemberName]string caller = "")
        {
            logger.Log(() => string.Format("{0} -> {1}", msgFactory(), caller), EventLevel.Info);
            return logger;
        }

        /// <summary>
        /// Логирование при условии, что уровень журналирования включен. В текст сообщения включено имя метода, из оторого вызывается логирование
        /// </summary>
        /// <param name="logger">логгер</param>
        /// <param name="msgFactory">функция, которая возвраащет сообщение для логирования</param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public static ILogger LogTrace(this ILogger logger, Func<string> msgFactory, [CallerMemberName]string caller = "")
        {
            logger.Log(() => string.Format("{0} -> {1}", msgFactory(), caller), EventLevel.Trace);
            return logger;
        }

        /// <summary>
        /// Логирование при условии, что уровень журналирования включен. В текст сообщения включено имя метода, из оторого вызывается логирование
        /// </summary>
        /// <param name="logger">логгер</param>
        /// <param name="msgFactory">функция, которая возвраащет сообщение для логирования</param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public static ILogger LogDebug(this ILogger logger, Func<string> msgFactory, [CallerMemberName]string caller = "")
        {
            logger.Log(() => string.Format("{0} -> {1}", msgFactory(), caller), EventLevel.Debug);
            return logger;
        }

        /// <summary>
        /// Логирование при условии, что уровень журналирования включен. В текст сообщения включено имя метода, из оторого вызывается логирование
        /// </summary>
        /// <param name="logger">логгер</param>
        /// <param name="msgFactory">функция, которая возвраащет сообщение для логирования</param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public static ILogger LogError(this ILogger logger, Func<string> msgFactory, [CallerMemberName]string caller = "")
        {
            logger.Log(() => string.Format("{0} -> {1}", msgFactory(), caller), EventLevel.Error);
            return logger;
        }

        /// <summary>
        /// Логирование при условии, что уровень журналирования включен. В текст сообщения включено имя метода, из оторого вызывается логирование
        /// </summary>
        /// <param name="logger">логгер</param>
        /// <param name="msgFactory">функция, которая возвраащет сообщение для логирования</param>
        /// <param name="caller"></param>
        /// <returns></returns>
        public static ILogger LogFatal(this ILogger logger, Func<string> msgFactory, [CallerMemberName]string caller = "")
        {
            logger.Log(() => string.Format("{0} -> {1}", msgFactory(), caller), EventLevel.Fatal);
            return logger;
        }
    }
}
