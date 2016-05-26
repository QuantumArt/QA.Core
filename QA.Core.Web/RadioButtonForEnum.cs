// Owners: Alexey Abretov, Nikolay Karlov

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace QA.Core.Web
{
    /// <summary>
    /// Расширяет html-хелперы
    /// </summary>
    public static class HtmlExtensions
    {
        public static MvcHtmlString RadioButtonForEnum<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            TProperty[] excludeList,
            Dictionary<string, object> htmlAttributes = null,
            string labelWrapperFormat = null, string elementWrapperFormat = null) where TProperty : struct, IConvertible
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            Type enumType = metaData.ModelType;
            var sb = new StringBuilder();

            if (string.IsNullOrEmpty(elementWrapperFormat))
            {
                elementWrapperFormat = "{0}";
            }
            else
            {
                if (!elementWrapperFormat.Contains('{') && !elementWrapperFormat.Contains('}') && !elementWrapperFormat.Contains('<') && !elementWrapperFormat.Contains('>'))
                {
                    elementWrapperFormat = string.Format("<{0}>", elementWrapperFormat) + "{0}" +
                        string.Format("</{0}>", elementWrapperFormat);
                }
            }

            IEnumerable<TProperty> values = Enum.GetValues(enumType).Cast<TProperty>();

            values
                .Where(w => excludeList != null ? !excludeList.Contains(w) : true)
                .ToList()
                .ForEach(f =>
                {
                    PrepareHtml(htmlHelper, expression, metaData, f, sb, htmlAttributes, labelWrapperFormat, elementWrapperFormat);
                });

            return MvcHtmlString.Create(sb.ToString());
        }

        /// <summary>
        /// Создает разметку для одного конкретного значения
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="_htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="value">значение</param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString RadioButtonForSingleEnum<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            TProperty value,
            Dictionary<string, object> htmlAttributes = null,
            string labelWrapperFormat = null) where TProperty : struct, IConvertible
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            Type enumType = metaData.ModelType;
            var sb = new StringBuilder();
            string elementFormat = "{0}";
            var items = Enum.GetValues(enumType)
                .Cast<TProperty>()
                .Where(x => x.Equals(value)).ToList();

            if (items.Count > 0)
            {
                var f = items[0];

                PrepareHtml(htmlHelper, expression, metaData, f, sb, htmlAttributes, labelWrapperFormat, elementFormat);
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        private static void PrepareHtml<TModel, TProperty>(
            HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            ModelMetadata metaData,
            TProperty value,
            StringBuilder builder,
            Dictionary<string, object> htmlAttributes, string labelWrapperFormat, string elementFormat)
        {
            var attributes = new Dictionary<string, object>();

            string id = string.Empty;

            if (!string.IsNullOrEmpty(htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix))
            {
                 id = string.Format(
                    "{0}_{1}_{2}",
                    htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix,
                    metaData.PropertyName,
                    value.ToString());
            }
            else
            {
                id = string.Format(
                    "{0}_{1}",
                    metaData.PropertyName,
                    value.ToString());
            }

            attributes["id"] = id;

            // объединяем с аттрибутами из верстки
            if (htmlAttributes != null)
            {
                foreach (var key in htmlAttributes.Keys)
                {
                    attributes[key] = htmlAttributes[key];
                }
            }

            var radio = htmlHelper.RadioButtonFor(expression, value.ToString(), attributes).ToHtmlString();

            if (string.IsNullOrEmpty(labelWrapperFormat))
            {
                labelWrapperFormat = "{2}<label for=\"{0}\">{1}</label>";
            }

            builder.AppendFormat(elementFormat, string.Format(
                labelWrapperFormat,
                id,
                HttpUtility.HtmlEncode((value as Enum).GetLocalizedDescription()),
                radio));
        }

        public static MvcHtmlString WrapWith(this MvcHtmlString source, string elementWrapperFormat, string className = null)
        {
            if (source == null)
            {
                return null;
            }

            if (!elementWrapperFormat.Contains('{') && !elementWrapperFormat.Contains('}') && !elementWrapperFormat.Contains('<') && !elementWrapperFormat.Contains('>'))
            {
                var tb = new TagBuilder(elementWrapperFormat);
                
                tb.AddCssClass(className ?? string.Empty);

                tb.InnerHtml =  source.ToString();

                return MvcHtmlString.Create(tb.ToString());
            }

            return new MvcHtmlString(string.Format(elementWrapperFormat, source.ToString()));
        }
    }
}
