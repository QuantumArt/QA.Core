// Owners: Karlov Nikolay, Abretov Alexey

using System;
using System.Diagnostics;
using System.IO;
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
        /// Throws an <see cref="ArgumentNullException" /> if the specified argument is null.
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void IfArgumentNull<T>(Expression<Func<object, T>> expression, Func<Exception, Exception> modifier = null)
        {
            var argument = expression.Compile().Invoke(null);
            IfArgumentNull<T>(argument, expression, modifier);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException" /> if the specified argument is null.
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void IfArgumentNull<T>(T argument, Expression<Func<object, T>> expression, Func<Exception, Exception> modifier = null)
        {
            if (object.Equals(argument, default(T)))
            {
                ThrowInternal(
                    new ArgumentNullException(ExpressionExtensions.GetPropertyName(expression)),
                    modifier);
            }
        }
              

        /// <summary>
        /// Throws an <see cref="ArgumentNullException" /> if the specified argument is null.
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void IfArgumentNot<T>(bool condition, Expression<Func<object, T>> expression, Func<Exception, Exception> modifier = null)
        {
            IfArgumentNot(() => condition, expression, modifier);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException" /> if the specified argument is null.
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void IfArgumentNot<T>(Func<bool> condition, Expression<Func<object, T>> expression, Func<Exception, Exception> modifier = null)
        {
            Throws.IfArgumentNull(condition, _ => condition);
            if (!condition())
            {
                ThrowInternal(
                    new ArgumentNullException(ExpressionExtensions.GetPropertyName(expression)),
                    modifier);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException" /> if the specified argument is null.
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void IfArgumentNull(object argument, string argumentName, Func<Exception, Exception> modifier = null)
        {
            if (argument == null)
                ThrowInternal(
                    new ArgumentNullException(argumentName),
                    modifier);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException" /> if the specified argument is null.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="argument">The argument to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void IfArgumentNull(
            string message,
            object argument,
            string argumentName,
            Func<Exception, Exception> modifier = null)
        {
            if (argument == null)
                ThrowInternal(
                    new ArgumentNullException(argumentName, message),
                    modifier);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException" /> if the specified argument is null.
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void IfIntArgumentNull(int argument, string argumentName, Func<Exception, Exception> modifier = null)
        {
            if (argument <= 0)
                ThrowInternal(
                    new ArgumentNullException(argumentName),
                    modifier);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException" /> if the specified argument is null.
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void IfLongArgumentNull(long argument, string argumentName, Func<Exception, Exception> modifier = null)
        {
            if (argument <= 0)
                ThrowInternal(
                    new ArgumentNullException(argumentName),
                    modifier);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException" /> if the specified argument is null.
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void IfArrayArgumentNullOrEmpty<T>(T[] argument, Expression<Func<object, T[]>> expression, Func<Exception, Exception> modifier = null)
        {
            if (argument == null || argument.Length == 0)
                ThrowInternal(
                    new ArgumentNullException(ExpressionExtensions.GetPropertyName(expression)),
                    modifier);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException" /> if the specified argument is null.
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void IfArrayArgumentNullOrEmpty<T>(T[] argument, string argumentName, Func<Exception, Exception> modifier = null)
        {
            if (argument == null || argument.Length == 0)
                ThrowInternal(
                    new ArgumentNullException(argumentName),
                    modifier);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException" /> if the specified argument is null or equal to <see cref="String.Empty" />.
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void IfArgumentNullOrEmpty<T>(string argument, Expression<Func<object, T>> expression, Func<Exception, Exception> modifier = null)
        {
            if (string.IsNullOrEmpty(argument))
                ThrowInternal(
                    new ArgumentNullException(ExpressionExtensions.GetPropertyName(expression)),
                    modifier);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException" /> if the specified argument is null or equal to <see cref="String.Empty" />.
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void IfArgumentNullOrEmpty(string argument, string argumentName, Func<Exception, Exception> modifier = null)
        {
            if (string.IsNullOrEmpty(argument))
                ThrowInternal(
                    new ArgumentNullException(argumentName),
                    modifier);
        }

        /// <summary>
        /// Выбрасывает исключение ArgumentNullException, если имя файла пустое. FileNotFoundException, если файл отсутсвует.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="modifier"></param>
        [DebuggerStepThrough]
        public static void IfFileNotExists(string fileName, Func<Exception, Exception> modifier = null)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                ThrowInternal(
                    new ArgumentNullException(fileName),
                    modifier);
            }

            if (!File.Exists(fileName))
            {
                ThrowInternal(
                    new FileNotFoundException(string.Format(
                        Throw_Messages.FileNotFound, fileName)),
                    modifier);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException" /> if the specified argument is null or equal to <see cref="String.Empty" />.
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void IfArgumentIsNotEqual<T>(T argument, Func<T, bool> condition, Expression<Func<object, T>> expression, Func<Exception, Exception> modifier = null)
        {
            if (!condition(argument))
            {
                var argName = ExpressionExtensions.GetPropertyName(expression);
                ThrowInternal(
                    new ArgumentException(
                        string.Format(Throw_Messages.ArgumentHasInvalidValueExceptionFormat, argName), argName),
                    modifier);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException" /> if the specified argument is null or equal to <see cref="String.Empty" />.
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void IfArgumentIsNotEqual<T>(T argument, Func<T, bool> condition, string argumentName, Func<Exception, Exception> modifier = null)
        {
            if (!condition(argument))
            {
                ThrowInternal(
                    new ArgumentException(
                        string.Format(Throw_Messages.ArgumentHasInvalidValueExceptionFormat, argumentName), argumentName),
                    modifier);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException" /> if the specified argument is null or equal to <see cref="String.Empty" />.
        /// </summary>
        /// <param name="argument">The argument to check.</param>
        /// <param name="argumentName">The name of the argument.</param>
        /// <param name="modifier">A modifier delegate used to modify the exception before being thrown.</param>
        [DebuggerStepThrough]
        public static void IfArgumentIsNotEqual<T>(T argument, Func<T, bool> condition, Func<Exception, Exception> modifier = null)
        {
            if (!condition(argument))
            {
                ThrowInternal(
                    new ArgumentException(
                        string.Format(Throw_Messages.ArgumentHasInvalidValueExceptionFormat, string.Empty)),
                    modifier);
            }
        }

        #endregion
    }
}
