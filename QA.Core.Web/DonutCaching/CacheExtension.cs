// Owners: Karlov Nikolay, Abretov Alexey

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.WebPages;
using System.Web.Routing;
using System.Web;
#pragma warning disable 1591

namespace QA.Core.Web
{
    /// <summary>
    /// Класс-роасширение HtmlHelper для работы с кешем
    /// </summary>
    public static class CacheExtension
    {
        public const string MarkupReplacement = "2FFE84F9-A70A-4481-99AF-5DE6A709F678";
        public const string ActionReplacement = "873DD803-5603-47C4-9F5C-AFDC29F4E725";
        public const string IsCachingAppliedKey = "0E205307-2C26-4BE1-9221-9303814219E6";

        internal static bool IsCachingApplied
        {
            get
            {
                var val = HttpContext.Current.Items[IsCachingAppliedKey];
                if (object.Equals(val, true))
                {
                    return true;
                }

                return false;
            }
            set { HttpContext.Current.Items[IsCachingAppliedKey] = value; }
        }

        /// <summary>
        /// Создает некешируемую аттрибутом ResultCacheAttribute область разметки Razor
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="action">Лямбда-выражение для разметки Razor. Не может включать методы-расширения,
        /// обращающиеся к Stream (BeginForm, RenderPartial,...)</param>
        /// <returns></returns>
        public static MvcHtmlString NoCache(this HtmlHelper helper, Func<object, HelperResult> action)
        {
            if (action != null)
            {
                var markupViewDataKey = GenerateViewDataKey(helper.ViewContext.Controller, MarkupReplacement);

                var replacements = GetMarkupReplacements(helper.ViewContext.Controller);

                var key = CreatePlaceholderKey(helper, MarkupReplacement);

                replacements.Add(new MarkupReplacement(key, action));

                return MvcHtmlString.Create(key);
            }
            else
            {
                return MvcHtmlString.Empty;
            }
        }

        /// <summary>
        /// Некешируемый Action
        /// </summary>
        /// <returns></returns>
        public static MvcHtmlString NotCachedAction(this HtmlHelper helper, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            var placeHolder = CreatePlaceholderKey(helper, ActionReplacement);

            var settings = new ActionReplacement
            {
                Key = placeHolder,
                Action = actionName,
                Controller = controllerName,
                RouteValues = routeValues
            };

            var actionViewDataKey = GenerateViewDataKey(helper.ViewContext.Controller, ActionReplacement);

            var replacements = GetActionReplacements(helper.ViewContext.Controller);

            replacements.Add(settings);

            return MvcHtmlString.Create(placeHolder);
        }

        /// <summary>
        /// Генерация некешируемого действия контроллера
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public static MvcHtmlString NoCachedAction(this HtmlHelper helper, string actionName, string controllerName, object routeValues)
        {
            return NoCachedAction(helper, actionName, controllerName, new RouteValueDictionary(routeValues));
        }

        #region Helpers
        /// <summary>
        /// Получаем автозамены из контекста контроллера
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        internal static List<MarkupReplacement> GetMarkupReplacements(ControllerBase controller)
        {
            // получаем автозамены из контекста контроллера
            return GetReplacements<MarkupReplacement>(controller, MarkupReplacement);
        }

        /// <summary>
        /// Получаем автозамены из контекста контроллера
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        internal static List<ActionReplacement> GetActionReplacements(ControllerBase controller)
        {
            // получаем автозамены из контекста контроллера
            return GetReplacements<ActionReplacement>(controller, ActionReplacement);
        }

        /// <summary>
        /// Ключ коллекции автозамен в ViewData
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GenerateViewDataKey(ControllerBase controller, string key)
        {
            return key ?? throw new ArgumentNullException(nameof(key));
        }

        /// <summary>
        /// Ключ автозамены
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string CreatePlaceholderKey(HtmlHelper helper, string key)
        {
            return "<!--" + key + "-" + Guid.NewGuid().ToString() + "-->";
        }

        /// <summary>
        /// Получение списка замен
        /// </summary>
        private static List<T> GetReplacements<T>(ControllerBase controller, string constKey)
            where T : ReplacementBase
        {
            var key = GenerateViewDataKey(controller, constKey);
            var result = (List<T>)controller.ControllerContext.HttpContext.Items[key];

            if (result == null)
            {
                result = new List<T>();
                controller.ControllerContext.HttpContext.Items[key] = result;
            }
            return result;
        }
        #endregion
    }
}
