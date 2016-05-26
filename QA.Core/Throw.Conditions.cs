// Owners: Karlov Nikolay, Abretov Alexey

using System;
using System.Diagnostics;
using System.Linq.Expressions;
using QA.Core.Linq;

namespace QA.Core
{
    /// <summary>
    /// Расширения для exception 
    /// </summary>
    public static partial class Throws
    {
        #region Methods

        /// <summary>
        /// Throws an <see cref="ArgumentException" /> if the specified argument is null.
        /// </summary>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void IfNot<T>(Func<bool> condition, Expression<Func<object, T>> expression, string message = null, Func<Exception, Exception> modifier = null)
        {
            if (!condition())
            {
                var argName = ExpressionExtensions.GetPropertyName(expression);

                message = message ??
                        string.Format(Throw_Messages.ArgumentHasInvalidValueExceptionFormat, argName);

                ThrowInternal(new ArgumentException(message, argName), modifier);
            }
        }

        [DebuggerStepThrough]
        public static void IfNot<T>(bool condition, Expression<Func<object, T>> expression, string message = null, Func<Exception, Exception> modifier = null)
        {
            IfNot(() => condition, expression, message, modifier);
        }

        /// <summary>
        /// Throws an <see cref="Exception" /> if the specified argument is null.
        /// </summary>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void IfNot(Func<bool> condition, string message, Func<Exception, Exception> modifier = null)
        {
            if (condition != null)
            {
                IfNot(condition(), message, modifier);
            }
        }

        [DebuggerStepThrough]
        public static void IfNot(bool condition, string message, Func<Exception, Exception> modifier = null)
        {
            if (!condition)
            {
                ThrowInternal(new Exception(message), modifier);
            }
        }

        /// <summary>
        /// Выкидывает исключение, если не выполняется условие.
        /// Отложенное получение текста ошибки
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        /// <param name="modifier"></param>
        [DebuggerStepThrough]
        public static void IfNot(bool condition, Func<string> message, Func<Exception, Exception> modifier = null)
        {
            if (!condition)
            {
                ThrowInternal(new Exception(message()), modifier);
            }
        }

        #endregion
    }
}
