// Owners: Alexey Abretov, Nikolay Karlov

using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
#pragma warning disable 1591

namespace QA.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="String" /> type.
    /// </summary>
    public static class StringExtensions
    {
        #region Methods

        /// <summary>
        /// Formats the given string using the specified culture and arguments.
        /// </summary>
        /// <param name="string">The string to format.</param>
        /// <param name="arguments">The arguments used to format the string.</param>
        /// <exception cref="ArgumentNullException">If the input string, or any arguments are null.</exception>
        /// <exception cref="FormatException">If the input string is invalid, or the index of a format item is less than zero, or greater than or equal to the length of the args array.</exception>
        public static string Format(this string @string, params object[] arguments)
        {
            return Format(@string, CultureInfo.CurrentUICulture, arguments);
        }

        /// <summary>
        /// Formats the given string using the specified culture and arguments.
        /// </summary>
        /// <param name="string">The string to format.</param>
        /// <param name="culture">The culture used to format the string.</param>
        /// <param name="arguments">The arguments used to format the string.</param>
        /// <exception cref="ArgumentNullException">If the input string, or any arguments are null.</exception>
        /// <exception cref="FormatException">If the input string is invalid, or the index of a format item is less than zero, or greater than or equal to the length of the args array.</exception>
        public static string Format(this string @string, CultureInfo culture, params object[] arguments)
        {
            return string.Format(culture, @string, arguments);
        }

        /// <summary>
        /// Сериализация объекта в формат JSON
        /// </summary>
        /// <param name="obj">Объект для сериализации</param>
        public static string ToJsonString<T>(this T obj) where T : class
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, obj);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        public static string[] SplitString(this string @string, params char[] separator)
        {
            return @string.Split(separator).Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToArray();
        }

        public static IEnumerable<Int32> CastAsInt(this IEnumerable<string> source, bool skipOnError = false)
        {
            return source.Select(x => Int32.Parse(x));
        }

        #endregion
    }
}
