// Owners: Karlov Nikolay, Abretov Alexey

using System;
using System.Globalization;
using System.Web;
using System.Diagnostics;

namespace QA.Core.Web
{
    /// <summary>
    /// Класс-помощник
    /// </summary>
    public class LocalizationHelper
    {
        /// <summary>
        /// Возвращает культуру, установленную в браузере
        /// </summary>
        /// <returns></returns>
        public static CultureInfo ResolveCultureFromBrowser()
        {
            string[] languages = HttpContext.Current.Request.UserLanguages;

            if (languages == null || languages.Length == 0)
                return null;

            try
            {
                string language = languages[0].ToLowerInvariant().Trim();
                return CultureInfo.CreateSpecificCulture(language);
            }
            catch (ArgumentException ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
