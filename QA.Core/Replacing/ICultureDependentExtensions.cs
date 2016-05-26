using QA.Core.Replacing;
using System.Collections.Generic;

namespace QA.Core
{
    /// <summary>
    /// Расширения для замен значенией, зависящих от установленного языка
    /// </summary>
    public static class ICultureDependentExtensions
    {
        /// <summary>
        /// Произвести замену зависимых от установленной культуры полей
        /// </summary>
        /// <param name="obj">объект</param>
        /// <param name="cultureKey">культура ("ru-ru", ...)</param>
        /// <returns></returns>
        public static object ProcessCultureReplacement(this ICultureDependent obj, string cultureKey)
        {
            return ReplacementManager.Current.Process((object)obj, cultureKey);
        }

        /// <summary>
        /// Произвести замену зависимых от установленной культуры полей
        /// </summary>
        /// <param name="obj">объект</param>
        /// <returns></returns>
        public static object ProcessCultureReplacement(this ICultureDependent obj)
        {
            return ReplacementManager.Current.Process((object)obj);
        }

        /// <summary>
        /// Произвести замену зависимых от установленной культуры полей в коллекции
        /// </summary>
        /// <param name="obj">коллекция объектов</param>
        /// <param name="cultureKey">культура ("ru-ru", ...)</param>
        /// <returns></returns>
        public static void ProcessCultureReplacement(this IEnumerable<ICultureDependent> obj, string cultureKey)
        {
            ReplacementManager.Current.ProcessCollection((IEnumerable<object>)obj, cultureKey);
        }

        /// <summary>
        /// Произвести замену зависимых от установленной культуры полей в коллекции.
        /// </summary>
        /// <param name="obj">коллекция объектов</param>      
        /// <returns></returns>
        public static void ProcessCultureReplacement(this IEnumerable<ICultureDependent> obj)
        {
            ReplacementManager.Current.ProcessCollection((IEnumerable<object>)obj);
        }
    }
}
