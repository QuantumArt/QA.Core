// Owners: Karlov Nikolay

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using QA.Core.Web;

namespace QA.Core.Web
{
    /// <summary>
    /// Кеширование страницы с поддержкой некешируемых областей (Donut caching).
    /// Является потокобезопасным.
    /// </summary>
    public class ResultCacheAttribute : ActionFilterAttribute, IReplacementStorage
    {
        #region Переменные запроса

        /// <summary>
        /// Текущий контроллер
        /// </summary>
        private static RequestLocal<ControllerBase> CurrentController = new RequestLocal<ControllerBase>();

        /// <summary>
        /// Текущий элемент кеша
        /// </summary>
        private static RequestLocal<DonutCacheItem> CurrentDonutItem = new RequestLocal<DonutCacheItem>();

        /// <summary>
        /// Флаг, определяющий, применялись ли автозамены потока ответа
        /// </summary>
        private static RequestLocal<bool> IsFilterApplied = new RequestLocal<bool>();

        /// <summary>
        /// Флаг, определяющий, применялись ли замены
        /// </summary>
        private static RequestLocal<bool> IsCacheItemUsed = new RequestLocal<bool>();

        /// <summary>
        /// Текущий ключ кеша
        /// </summary>
        private static RequestLocal<string> CacheKey = new RequestLocal<string>();

        #endregion

        #region Параметры

        public bool VaryByUserName { get; set; }

        public string VaryByCustom { get; set; }

        public string VaryByParam { get; set; }

        /// <summary>
        /// Время хранения в кеше (в секундах)
        /// </summary>
        public int Duration
        {
            get;
            set;
        }
        #endregion

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            CurrentController.Value = filterContext.Controller;
            var cacheKey = GenerateCacheKey(filterContext);
            CacheKey.Value = cacheKey;


            // применяем фильтр
            if (!CacheExtension.IsCachingApplied &&
                filterContext.RequestContext.HttpContext.Response.Filter != null &&
                (filterContext.RequestContext.HttpContext.Response.Filter is CacheFilterStream == false))
            {
                // если применение возможно, и еще не применен
                filterContext.RequestContext.HttpContext.Response.Filter =
                    new CacheFilterStream(filterContext.RequestContext.HttpContext.Response.Filter, this);

                IsFilterApplied.Value = true;
            }
            if (filterContext.HttpContext.Cache[cacheKey] != null)
            {
                CurrentDonutItem.Value = (DonutCacheItem)filterContext.HttpContext.Cache[cacheKey];

                filterContext.Result = new ContentResult
                {
                    Content = CurrentDonutItem.Value.Result,
                    ContentType = "text/html"
                };
                CacheExtension.IsCachingApplied = true;
                IsCacheItemUsed.Value = true;
            }
            else
            {
                CurrentDonutItem.Value = null;
                base.OnActionExecuting(filterContext);
            }
        }
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            if (!IsCacheItemUsed.Value && filterContext.Controller.ControllerContext.IsChildAction && !IsFilterApplied.Value)
            {
                // TODO: произвести замену в ручную.
                // Мы сюда попадаем, если применен [ResultCache] для child Actions
                StringBuilder sb = new StringBuilder();

                using (var writer = new StringWriter(sb))
                {
                    var viewResult = filterContext.Result as ViewResultBase;
                    if (viewResult != null)
                    {
                        var view = ViewEngines.Engines.FindPartialView(filterContext.Controller.ControllerContext,
                            string.IsNullOrEmpty(viewResult.ViewName) ? (string)filterContext.RouteData.Values["action"] : viewResult.ViewName);

                        ViewContext viewContext = new ViewContext(filterContext.Controller.ControllerContext,
                            view.View,
                            viewResult.ViewData,
                            viewResult.TempData,
                            writer);

                        view.View.Render(viewContext, writer);
                    }
                }

                var subResult = sb.ToString();
                var item = new DonutCacheItem(subResult, new List<ReplacementBase>());
                ((IReplacementStorage)this).SetCache(item);
            }

            base.OnResultExecuting(filterContext);
        }

        /// <summary>
        /// Генерация ключа для кеша
        /// </summary>
        /// <param name="filterContext">контекст</param>
        /// <returns></returns>
        private string GenerateCacheKey(ControllerContext filterContext)
        {
            string url = "";
            foreach (var item in filterContext.RouteData.Values)
            {
                url += "." + item.Value;
            }

            if (VaryByUserName)
            {
                url += ".VaryByUserName-" + filterContext.HttpContext.User.Identity.Name;
            }

            var cacheKey = "ResultCache-" + url;

            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                cacheKey += "-ajax";
            }

            if (!string.IsNullOrEmpty(VaryByCustom))
            {
                var customString = filterContext.HttpContext.ApplicationInstance
                    .GetVaryByCustomString(HttpContext.Current, VaryByCustom);

                if (!string.IsNullOrEmpty(customString))
                {
                    cacheKey += ".VaryByCustom-" + customString;
                }
            }

            if (!string.IsNullOrEmpty(VaryByParam))
            {
                var @params = SplitVaryByParam(VaryByParam);
                NameValueCollection values = null;

                if (filterContext.RequestContext.HttpContext.Request.HttpMethod.ToLower() == "get")
                {
                    values = filterContext.RequestContext.HttpContext.Request.QueryString;
                }

                if (filterContext.RequestContext.HttpContext.Request.HttpMethod.ToLower() == "post")
                {
                    values = filterContext.RequestContext.HttpContext.Request.Form;
                }

                if ((values == null || values.Count == 0) && filterContext.RouteData.Values != null &&
                    filterContext.RouteData.Values.Count != 0)
                {
                    values = new NameValueCollection();
                    foreach (var item in filterContext.RouteData.Values)
                    {
                        values.Add(item.Key, item.Value.ToString());
                    }
                }

                cacheKey += ".VaryByParam-";

                if (@params != null)
                {
                    foreach (var p in @params)
                    {
                        if (values.AllKeys.Contains(p))
                        {
                            cacheKey += "." + p + "-" + values[p];
                        }
                    }
                }
                else
                {
                    foreach (var p in values.AllKeys)
                    {
                        cacheKey += "." + p + "-" + values[p];
                    }
                }
            }

            return cacheKey.ToLower();
        }

        private static IEnumerable<string> SplitVaryByParam(string varyByParam)
        {
            if (string.Equals(varyByParam, "none", StringComparison.OrdinalIgnoreCase))
                return Enumerable.Empty<string>();
            else if (string.Equals(varyByParam, "*", StringComparison.OrdinalIgnoreCase))
                return (IEnumerable<string>)null;
            else
                return Enumerable.Select(Enumerable.Where(Enumerable.Select((IEnumerable<string>)varyByParam.Split(new char[1]
            {
                ';'
            }), part => new
            {
                part = part,
                trimmed = part.Trim()
            }), param0 => !string.IsNullOrEmpty(param0.trimmed)), param0 => param0.trimmed);
        }

        #region IReplacementStorage Members

        /// <summary>
        /// Получение списка автозамен
        /// </summary>
        /// <returns></returns>
        List<ReplacementBase> IReplacementStorage.GetReplacements()
        {
            DonutCacheItem donutItem = CurrentDonutItem.Value;
            if (donutItem != null)
            {
                // если элемент был найден в кеше, 
                // берем его автозамены
                return donutItem.Replacements;
            }
            else
            {
                ControllerBase controller = CurrentController.Value;

                // получаем автозамены из контекста контроллера
                return CacheExtension.GetMarkupReplacements(controller)
                    .Cast<ReplacementBase>()
                    .Union(CacheExtension.GetActionReplacements(controller).Cast<ReplacementBase>())
                    .ToList();
            }
        }

        /// <summary>
        /// Добавление Html-разметки в кеш
        /// </summary>
        /// <param name="itemToSet"></param>
        void IReplacementStorage.SetCache(DonutCacheItem itemToSet)
        {
            HttpContext.Current.Cache.Add(CacheKey.Value,
               itemToSet,
               null,
               DateTime.Now.AddSeconds(Duration),
               System.Web.Caching.Cache.NoSlidingExpiration,
               CacheItemPriority.Default, null);
        }

        void IReplacementStorage.RenderAction(ActionReplacement actionReplacement, StringWriter writer)
        {
            var controller = CurrentController.Value;
            var viewContext = new ViewContext(controller.ControllerContext,
                                      new WebFormView(controller.ControllerContext, "tmp"),
                                      controller.ViewData, controller.TempData,
                                      writer);

            var htmlHelper = new HtmlHelper(viewContext, new ViewPage());

            writer.Write(htmlHelper
                .Action(actionReplacement.Action,
                        actionReplacement.Controller,
                        actionReplacement.RouteValues)
                    .ToString());
        }
        #endregion
    }
}
