// Owners: Alexey Abretov, Nikolay Karlov

using System;
using System.Linq;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Routing;

namespace QA.Core.Web
{
    /// <summary>
    /// Реализует методы для создания и кеширования действий
    /// </summary>
    public class UrlHelperCached : UrlHelper
    {
        /// <summary>
        /// Конструирует объект по параметрам
        /// </summary>
        /// <param name="requestContext">Контекст запроса</param>
        public UrlHelperCached(
            RequestContext requestContext)
            : base(requestContext) { }

        /// <summary>
        /// Конструирует объект по параметрам
        /// </summary>
        /// <param name="requestContext">Контекст запроса</param>
        /// <param name="routeCollection">Коллекция route</param>
        public UrlHelperCached(
            RequestContext requestContext,
            RouteCollection routeCollection)
            : base(requestContext, routeCollection) { }

        /// <summary>
        /// Создает и кеширует действие
        /// </summary>
        /// <param name="actionName">Наименование действия</param>
        /// <param name="controllerName">Натменование контроллера</param>
        /// <returns>Html-элемента действия</returns>
        public new string Action(string actionName, string controllerName)
        {
            if (RequestContext.HttpContext.Cache[actionName + controllerName] == null)
            {
                RequestContext.HttpContext.Cache.Add(
                    actionName + controllerName,
                    base.Action(actionName, controllerName),
                    null,
                    DateTime.MaxValue,
                    TimeSpan.Zero,
                    CacheItemPriority.Normal,
                    null);
            }

            string action = RequestContext.HttpContext.Cache[
                actionName + controllerName].ToString();

            return action;
        }

        /// <summary>
        /// Создает и кеширует действие
        /// </summary>
        /// <param name="actionName">Наименование действия</param>
        /// <param name="controllerName">Натменование контроллера</param>
        /// <param name="routeValues">Дополнительные параметры действия</param>
        /// <returns>Html-элемента действия</returns>
        public new string Action(string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            if (routeValues == null || routeValues.Count == 0)
            {
                return Action(actionName, controllerName);
            }

            string key = actionName + controllerName +
                routeValues == null ? string.Empty : string.Join(string.Empty, routeValues.Keys.ToArray()) +
                routeValues == null ? string.Empty : string.Join(string.Empty, routeValues.Values.Select(s => s.ToString()).ToArray());

            if (RequestContext.HttpContext.Cache[key] == null)
            {
                RequestContext.HttpContext.Cache.Add(
                    key,
                    base.Action(actionName, controllerName, routeValues),
                    null,
                    DateTime.MaxValue,
                    TimeSpan.Zero,
                    CacheItemPriority.Normal,
                    null);
            }

            string action = RequestContext.HttpContext.Cache[
                key].ToString();

            return action;
        }
    }
}
