// Owners: Alexey Abretov, Nikolay Karlov

using System.Globalization;
using System.Web.Mvc;

namespace QA.Core.Web
{
    /// <summary>
    /// Расширяет представление
    /// </summary>
    public static class ViewExtender
    {
        /// <summary>
        /// Возвращает значение локального ресурса
        /// </summary>
        /// <param name="page">Страница</param>
        /// <param name="key">Ключ ресурса</param>
        /// <returns>Значение ресурса ввиде строки</returns>
        public static string LocalResources(this WebViewPage page, string key)
        {
            return page.ViewContext.HttpContext.GetLocalResourceObject(
                page.VirtualPath, key) as string;
        }

        /// <summary>
        /// Возвращает значение глобального ресурса данного класса
        /// </summary>
        /// <param name="classKey">Название класса</param>
        /// <param name="resourceKey">Ключ ресурса</param>
        /// <returns></returns>
        public static object GetGlobalResource(this HtmlHelper htmlHelper, string classKey, string resourceKey)
        {
            return htmlHelper.ViewContext.HttpContext.GetGlobalResourceObject(classKey, resourceKey);
        }

        public static object GetGlobalResource(this HtmlHelper htmlHelper, string classKey, string resourceKey, CultureInfo culture)
        {
            return htmlHelper.ViewContext.HttpContext.GetGlobalResourceObject(classKey, resourceKey, culture);
        }

        public static object GetLocalResource(this HtmlHelper htmlHelper, string classKey, string resourceKey)
        {
            return htmlHelper.ViewContext.HttpContext.GetLocalResourceObject(classKey, resourceKey);
        }

        public static object GetLocalResource(this HtmlHelper htmlHelper, string classKey, string resourceKey, CultureInfo culture)
        {
            return htmlHelper.ViewContext.HttpContext.GetLocalResourceObject(classKey, resourceKey, culture);
        }
    }
}
