using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace QA.Core.Web
{
    /// <summary>
    /// Реализует список элементов из перечисления
    /// </summary>
    public static class EnumDropDownExtensions
    {
        /// <summary>
        /// Создает список элементов из перечисления
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="selectedValue"></param>
        /// <returns></returns>
        public static MvcHtmlString EnumDropDownList<TEnum>(
            this HtmlHelper htmlHelper,
            string name,
            TEnum selectedValue)
        {
            IEnumerable<TEnum> values = Enum.GetValues(typeof(TEnum))
                                            .Cast<TEnum>();

            IEnumerable<SelectListItem> items =
            from value in values
            select new SelectListItem
            {
                Text = value.ToString(),
                Value = value.ToString(),
                Selected = (value.Equals(selectedValue))
            };

            return htmlHelper.DropDownList(
                name,
                items);
        }

        /// <summary>
        /// Создает список элементов из перечисления
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TEnum>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            Type enumType = metadata.GetType();
            IEnumerable<TEnum> values = Enum.GetValues(enumType).Cast<TEnum>();

            IEnumerable<SelectListItem> items =
            from value in values
            select new SelectListItem
            {
                Value = value.ToString(),
                Selected = value.Equals(metadata.Model)
            };

            return htmlHelper.DropDownListFor(
                expression,
                items);
        }

        /// <summary>
        /// Создает список элементов из перечисления
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="name"></param>
        /// <param name="exclude"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString LocalizableEnumDropDownListFor<TModel, TEnum>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TEnum>> expression,
            string name,
            TEnum[] exclude,
            Dictionary<string, object> htmlAttributes = null) where TEnum : struct, IConvertible
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            Type enumType = metadata.ModelType;
            IEnumerable<TEnum> values = Enum.GetValues(enumType).Cast<TEnum>();

            IEnumerable<SelectListItem> items =
                from value in values
                where exclude != null ? !exclude.Contains(value) : true
                select new SelectListItem
                {
                    Text = (value as Enum).GetLocalizedDescription(),
                    Value = value.ToString(),
                    Selected = value.Equals(metadata.Model)
                };

            if (string.IsNullOrEmpty(name))
            {
                return htmlHelper.DropDownListFor(
                    expression,
                    items);
            }

            return htmlHelper.DropDownList(
                name,
                items,
                htmlAttributes);
        }
    }
}
