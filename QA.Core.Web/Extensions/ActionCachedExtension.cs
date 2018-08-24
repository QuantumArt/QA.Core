// Owners: Alexey Abretov, Nikolay Karlov
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace QA.Core.Web
{
    /// <summary>
    /// Расширения для кеширования ссылок
    /// </summary>
    public static class ActionCachedExtension
    {
        /// <summary>
        /// Создает и кеширует ссылку
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText">Текст ссылки</param>
        /// <param name="actionName">Наименование действия</param>
        /// <param name="controllerName">Наименование контроллера</param>
        /// <param name="routeValues">Параметры ссылки</param>
        /// <param name="htmlAttributes">Параметры html-элемента</param>
        /// <returns></returns>
        public static MvcHtmlString ActionCachedLink(
            this HtmlHelper htmlHelper,
            string linkText,
            string actionName,
            string controllerName,
            RouteValueDictionary routeValues,
            IDictionary<string, object> htmlAttributes)
        {
            if ((routeValues == null || routeValues.Count == 0)
                    & (htmlAttributes == null || htmlAttributes.Count == 0))
            {
                return ActionCachedLink(htmlHelper, linkText, actionName, controllerName);
            }

            string key = linkText + actionName + controllerName +
                (routeValues == null ? string.Empty : string.Join(string.Empty, routeValues.Keys.ToArray())) +
                (routeValues == null ? string.Empty : string.Join(string.Empty, routeValues.Values.Select(s => s.ToString()).ToArray())) +
                (htmlAttributes == null ? string.Empty : string.Join(string.Empty, htmlAttributes.Keys.ToArray())) +
                (htmlAttributes == null ? string.Empty : string.Join(string.Empty, htmlAttributes.Values.Select(s => s.ToString()).ToArray()));

            if (HttpContext.Current.Cache[key] == null)
            {
                HttpContext.Current.Cache.Add(
                    key,
                    LinkExtensions.ActionLink(htmlHelper, linkText, actionName, controllerName, routeValues, htmlAttributes),
                    null,
                    DateTime.MaxValue,
                    TimeSpan.Zero,
                    CacheItemPriority.Normal,
                    null);
            }

            MvcHtmlString action = HttpContext.Current.Cache[
                key] as MvcHtmlString;

            return action;
        }

        /// <summary>
        /// Создает и кеширует ссылку
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText">Текст ссылки</param>
        /// <param name="actionName">Наименование действия</param>
        /// <param name="controllerName">Наименование контроллера</param>
        /// <returns></returns>
        public static MvcHtmlString ActionCachedLink(
            this HtmlHelper htmlHelper,
            string linkText,
            string actionName,
            string controllerName)
        {
            MvcHtmlString action;
            string key = linkText + actionName + controllerName;

            if (HttpContext.Current.Cache[key] == null)
            {
                HttpContext.Current.Cache.Add(
                    key,
                    LinkExtensions.ActionLink(htmlHelper, linkText, actionName, controllerName),
                    null,
                    DateTime.MaxValue,
                    TimeSpan.Zero,
                    CacheItemPriority.Normal,
                    null);
            }

            action = HttpContext.Current.Cache[
                key] as MvcHtmlString;

            return action;
        }
    }
}
