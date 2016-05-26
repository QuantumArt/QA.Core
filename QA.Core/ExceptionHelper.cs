// Owners: Karlov Nikolay

using System;
using System.Collections.Generic;
using System.Linq;

namespace QA.Core
{
    /// <summary>
    ///  Расширения Exception
    /// </summary>
    public static class ExceptionHelper
    {
        /// <summary>
        /// Представляет вложенные Exception с InnerExceptions в коллекцию
        /// <remarks>Сортировка: от родительского к дочернему</remarks>
        /// </summary>
        /// <param name="exception">Исключение</param>
        /// <returns>Коллекция исключений</returns>
        public static IEnumerable<Exception> Flat(this Exception exception)
        {
            Exception ex = exception;

            while (ex != null)
            {
                yield return ex;
                ex = ex.InnerException;
            }
        }

        /// <summary>
        /// Представляет вложенные Exception с InnerExceptions в коллекцию
        /// <remarks>Сортировка: от родительского к дочернему</remarks>
        /// </summary>
        /// <param name="exception">Исключение</param>
        /// <param name="selector">Селектор</param>
        /// <returns>Коллекция объектов</returns>
        public static IEnumerable<TResult> Flat<TResult>(this Exception exception, Func<Exception,TResult> selector)
        {
            return exception.Flat()
                .Select(selector);
        }

        /// <summary>
        /// Представляет вложенные Exception с InnerExceptions в коллекцию
        /// <remarks>Сортировка: от дочернего к родительскому</remarks>
        /// </summary>
        /// <param name="exception">Исключение</param>
        /// <returns>Коллекция исключений</returns>
        public static IEnumerable<Exception> FlatReversed(this Exception exception)
        {
            return exception.Flat().Reverse();
        }

        /// <summary>
        /// Представляет вложенные Exception с InnerExceptions в коллекцию
        /// <remarks>Сортировка: от родительского к дочернему</remarks>
        /// </summary>
        /// <param name="exception">Исключение</param>
        /// <param name="selector">Селектор</param>
        /// <returns>Коллекция объектов</returns>
        public static IEnumerable<TResult> FlatReversed<TResult>(this Exception exception, Func<Exception, TResult> selector)
        {
            return exception.FlatReversed()
                .Select(selector);
        }

        /// <summary>
        /// Выбрасывание исключения
        /// </summary>
        /// <param name="exception"></param>
        public static void Throw(this Exception exception)
        {
            throw exception;
        }
    }
}
