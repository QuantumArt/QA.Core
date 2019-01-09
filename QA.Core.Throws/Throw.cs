// Owners: Karlov Nikolay, Abretov Alexey

using System;
using System.Configuration;
using System.Diagnostics;

namespace QA.Core
{
    /// <summary>
    /// Throws handled exceptions.
    /// </summary>
    public static partial class Throws
    {
        #region Methods

        /// <summary>
        /// Throws the specified exception.
        /// </summary>
        /// <param name="exception">The exception to be thrown.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        private static void ThrowInternal(Exception exception, Func<Exception, Exception> modifier = null)
        {
            if (exception == null)
                return;

            Exception ex = null;
            if (modifier != null)
                ex = modifier(exception);

            /* We should never try and suppress an exception at this point, so make sure the original
             * exception is thrown if the modifier function returns null. */
            if (ex == null)
                ex = exception;

            throw ex;
        }

        /// <summary>
        /// Throws a <see cref="Exception" /> with the specified message.
        /// </summary>
        /// <param name="message">The message to throw.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void Exception(string message, Func<Exception, Exception> modifier = null)
        {
            ThrowInternal(new Exception(message), modifier);
        }

        /// <summary>
        /// Throws a <see cref="NotSupportedException" /> with the specified message.
        /// </summary>
        /// <param name="message">The message to throw.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void NotSupported(string message, Func<Exception, Exception> modifier = null)
        {
            ThrowInternal(new NotSupportedException(message), modifier);
        }

        /// <summary>
        /// Throws a <see cref="InvalidOperationException" /> with the specified message.
        /// </summary>
        /// <param name="message">The message to throw.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void InvalidOperation(string message, Func<Exception, Exception> modifier = null)
        {
            ThrowInternal(new InvalidOperationException(message), modifier);
        }

        /// <summary>
        /// Throws a <see cref="NotSupportedException" /> with the specified message.
        /// </summary>
        /// <param name="message">The message to throw.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void NotImplemented(string message, Func<Exception, Exception> modifier = null)
        {
            ThrowInternal(new NotImplementedException(message ?? Throw_Messages.MethodNotImplemented), modifier);
        }
       
        #endregion
    }
}
