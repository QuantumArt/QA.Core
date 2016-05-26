using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace QA.Core.Web.Html
{
    /// <summary>
    /// Расширения ссылок
    /// </summary>
    public static class ActionLinkExtension
    {
        /// <summary>
        /// Ссылка, не кодирует title
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="helper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString ActionLink<TModel>(
            this HtmlControlFactory<TModel> helper,
            string linkText,
            string actionName,
            string controllerName,
            object routeValues,
            object htmlAttributes)
        {
            return ActionLink<TModel>(
                helper,
                linkText,
                actionName,
                controllerName,
                new RouteValueDictionary(routeValues),
                (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// Ссылка, не кодирует title
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="helper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString ActionLink<TModel>(
            this HtmlControlFactory<TModel> helper,
            string linkText,
            string actionName,
            string controllerName,
            RouteValueDictionary routeValues,
            IDictionary<string, object> htmlAttributes)
        {
            if (string.IsNullOrEmpty(linkText))
            {
                throw new ArgumentException("linkText", "linkText");
            }

            return MvcHtmlString.Create(
                GenerateLink(
                    helper.HtmlHelper.ViewContext.RequestContext,
                    helper.HtmlHelper.RouteCollection,
                    linkText,
                    null,
                    actionName,
                    controllerName,
                    routeValues,
                    htmlAttributes));
        }

        /// <summary>
        /// Создает ссылку, не кодирует title
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="routeCollection"></param>
        /// <param name="linkText"></param>
        /// <param name="routeName"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        private static string GenerateLink(
            RequestContext requestContext,
            RouteCollection routeCollection,
            string linkText,
            string routeName,
            string actionName,
            string controllerName,
            RouteValueDictionary routeValues,
            IDictionary<string, object> htmlAttributes)
        {
            return GenerateLink(
                requestContext,
                routeCollection,
                linkText,
                routeName,
                actionName,
                controllerName,
                null,
                null,
                null,
                routeValues,
                htmlAttributes);
        }

        /// <summary>
        /// Создает ссылку, не кодирует title
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="routeCollection"></param>
        /// <param name="linkText"></param>
        /// <param name="routeName"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="protocol"></param>
        /// <param name="hostName"></param>
        /// <param name="fragment"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        private static string GenerateLink(
            RequestContext requestContext,
            RouteCollection routeCollection,
            string linkText,
            string routeName,
            string actionName,
            string controllerName,
            string protocol,
            string hostName,
            string fragment,
            RouteValueDictionary routeValues,
            IDictionary<string, object> htmlAttributes)
        {
            return GenerateLinkInternal(
                requestContext,
                routeCollection,
                linkText,
                routeName,
                actionName,
                controllerName,
                protocol,
                hostName,
                fragment,
                routeValues,
                htmlAttributes,
                true);
        }

        /// <summary>
        /// Создает ссылку, не кодирует title
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="routeCollection"></param>
        /// <param name="linkText"></param>
        /// <param name="routeName"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="protocol"></param>
        /// <param name="hostName"></param>
        /// <param name="fragment"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="includeImplicitMvcValues"></param>
        /// <returns></returns>
        private static string GenerateLinkInternal(
            RequestContext requestContext,
            RouteCollection routeCollection,
            string linkText,
            string routeName,
            string actionName,
            string controllerName,
            string protocol,
            string hostName,
            string fragment,
            RouteValueDictionary routeValues,
            IDictionary<string, object> htmlAttributes,
            bool includeImplicitMvcValues)
        {
            string str;
            string str1 = UrlHelper.GenerateUrl(
                routeName,
                actionName,
                controllerName,
                protocol,
                hostName,
                fragment,
                routeValues,
                routeCollection,
                requestContext,
                includeImplicitMvcValues);

            var tagBuilder = new TagBuilder("a");
            var tagBuilder1 = tagBuilder;
            str = (!string.IsNullOrEmpty(linkText) ? linkText : string.Empty);
            tagBuilder1.InnerHtml = str;
            var tagBuilder2 = tagBuilder;
            tagBuilder2.MergeAttributes<string, object>(htmlAttributes);
            tagBuilder2.MergeAttribute("href", str1);
            return tagBuilder2.ToString(TagRenderMode.Normal);
        }
    }
}
