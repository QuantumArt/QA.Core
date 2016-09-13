// Owners: Alexey Abretov, Nikolay Karlov

using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web.Mvc;
using System.Linq;
using Microsoft.Practices.Unity;

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
            return UseResourceFormatter(page.ViewContext.HttpContext.GetLocalResourceObject(
                page.VirtualPath, key) as string);
        }

        /// <summary>
        /// Возвращает значение глобального ресурса данного класса
        /// </summary>
        /// <param name="classKey">Название класса</param>
        /// <param name="resourceKey">Ключ ресурса</param>
        /// <returns></returns>
        public static object GetGlobalResource(this HtmlHelper htmlHelper, string classKey, string resourceKey)
        {
            return UseResourceFormatter(htmlHelper.ViewContext.HttpContext.GetGlobalResourceObject(classKey, resourceKey) as string);
        }

        public static object GetGlobalResource(this HtmlHelper htmlHelper, string classKey, string resourceKey, CultureInfo culture)
        {
            return UseResourceFormatter(htmlHelper.ViewContext.HttpContext.GetGlobalResourceObject(classKey, resourceKey, culture) as string);
        }

        public static object GetLocalResource(this HtmlHelper htmlHelper, string classKey, string resourceKey)
        {
            return UseResourceFormatter(htmlHelper.ViewContext.HttpContext.GetLocalResourceObject(classKey, resourceKey) as string);
        }

        public static object GetLocalResource(this HtmlHelper htmlHelper, string classKey, string resourceKey, CultureInfo culture)
        {
            return UseResourceFormatter(htmlHelper.ViewContext.HttpContext.GetLocalResourceObject(classKey, resourceKey, culture) as string);
        }

        /// <summary>
        /// Преобразуем строку с ресурсом с помощью IResourceFormatter, если он зарегистрирован в unity. Иначе оставляем строку без изменений.
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        private static string UseResourceFormatter(string resource)
        {
            if (ObjectFactoryBase.DefaultContainer.IsRegistered<IResourceFormatter>())
            {
                var formatter = ObjectFactoryBase.DefaultContainer.Resolve<IResourceFormatter>();
                return formatter.Modify(resource);
            }
            
            return resource;
        }
    }
}
