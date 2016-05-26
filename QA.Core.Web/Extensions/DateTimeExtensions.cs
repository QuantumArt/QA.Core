using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace QA.Core.Web.Extensions
{
    /// <summary>
    /// Расширения разметки для редактора DateTime
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class DateTimeExtensions<TModel>
    {
        private HtmlHelper<TModel> _htmlHelper;
        private const string DefaultFormatTwoZero = "00";
        private const string DefaultFormatFourZero = "0000";

        public DateTimeExtensions(HtmlHelper<TModel> htmlHelper)
        {
            this._htmlHelper = htmlHelper;
        }

        /// <summary>
        /// Выпадающий список для выбора года
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="minYear"></param>
        /// <param name="maxYear"></param>
        /// <param name="localizedText"></param>
        /// <param name="optionLabel"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public MvcHtmlString YearDropdownListFor<TProperty>(Expression<Func<TModel, TProperty>> expression,
            int minYear, int maxYear, string localizedText, string optionLabel = null,
            object htmlAttributes = null, string format = null)
        {
            return GenericRangeDropDownList<TProperty>(
                expression, minYear, maxYear, localizedText, optionLabel, htmlAttributes, format ?? DefaultFormatFourZero);
        }

        /// <summary>
        /// Выпадающий список для выбора месяца
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="localizedText"></param>
        /// <param name="optionLabel"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="localizedCommaSeparatedList"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public MvcHtmlString MonthDropdownListFor<TProperty>(Expression<Func<TModel, TProperty>> expression,
            string localizedText, string optionLabel = null, object htmlAttributes = null,
            string localizedCommaSeparatedList = null, string format = null)
        {
            List<string> list = null;

            if (string.IsNullOrEmpty(localizedCommaSeparatedList))
            {
                list = DateTimeFormatInfo.CurrentInfo
                    .MonthGenitiveNames
                    .Select(x => x.ToLower())
                    .ToList();
            }
            else
            {
                localizedCommaSeparatedList.Split(',');
            }

            return DropDownHelper<TProperty>(
                expression, localizedText, optionLabel, htmlAttributes, list, format ?? DefaultFormatTwoZero);
        }

        /// <summary>
        /// Выпадающий список для выбора дня
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="localizedText"></param>
        /// <param name="optionLabel"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public MvcHtmlString DayDropdownListFor<TProperty>(Expression<Func<TModel, TProperty>> expression,
            string localizedText, string optionLabel = null, object htmlAttributes = null, string format = null)
        {
            return GenericRangeDropDownList<TProperty>(
                expression, 1, 31, localizedText, optionLabel, htmlAttributes, format ?? DefaultFormatTwoZero);
        }

        /// <summary>
        /// Выпадающий список для выбора часа
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="localizedText"></param>
        /// <param name="optionLabel"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public MvcHtmlString HourDropdownListFor<TProperty>(Expression<Func<TModel, TProperty>> expression,
            string localizedText, string optionLabel = null, object htmlAttributes = null, string format = null)
        {
            return GenericRangeDropDownList<TProperty>(
                expression, 0, 23, localizedText, optionLabel, htmlAttributes, format ?? DefaultFormatTwoZero);
        }

        /// <summary>
        /// Выпадающий список для выбора минут
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="localizedText"></param>
        /// <param name="optionLabel"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public MvcHtmlString MinDropdownListFor<TProperty>(Expression<Func<TModel, TProperty>> expression,
           string localizedText, string optionLabel = null, object htmlAttributes = null, string format = null)
        {
            return GenericRangeDropDownList<TProperty>(
                expression, 0, 59, localizedText, optionLabel, htmlAttributes, format ?? DefaultFormatTwoZero);
        }

        #region Private Members

        /// <summary>
        /// Возвращает значение
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        private TProperty GetValue<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            return expression.Compile()
              .Invoke(_htmlHelper.ViewData.Model);
        }

        /// <summary>
        /// Возвращает выпадающий список
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="localizedText"></param>
        /// <param name="optionLabel"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="list"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        private MvcHtmlString DropDownHelper<TProperty>(
            Expression<Func<TModel, TProperty>> expression,
            string localizedText,
            string optionLabel,
            object htmlAttributes,
            List<string> list,
            string format)
        {
            var items = new List<SelectListItem>();

            if (!string.IsNullOrEmpty(localizedText))
            {
                items.Insert(0, new SelectListItem { Value = "", Text = localizedText });
            }

            var value = string.Format("{0}", GetValue(expression));

            for (int i = 0; i < list.Count(); i++)
            {
                items.Add(new SelectListItem
                {
                    Value = (i + 1).ToString(format),
                    Text = list[i],
                    Selected = ((i + 1).ToString().Equals(value,
                         StringComparison.InvariantCultureIgnoreCase))
                });
            }

            return _htmlHelper.DropDownListFor(expression, items, optionLabel, htmlAttributes);
        }

        /// <summary>
        /// Возвращает выпадающий список для диапазона
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="localizedText"></param>
        /// <param name="optionLabel"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        private MvcHtmlString GenericRangeDropDownList<TProperty>(
            Expression<Func<TModel, TProperty>> expression,
            int minValue,
            int maxValue,
            string localizedText,
            string optionLabel,
            object htmlAttributes,
            string format)
        {
            var items = new List<SelectListItem>();

            if (!string.IsNullOrEmpty(localizedText))
            {
                items.Insert(0, new SelectListItem { Value = "", Text = localizedText });
            }

            var value = string.Format("{0}", GetValue(expression));

            for (int i = minValue; i <= maxValue; i++)
            {
                items.Add(new SelectListItem
                {
                    Value = i.ToString(format),
                    Text = i.ToString(format),
                    Selected = (i.ToString().Equals(value,
                         StringComparison.InvariantCultureIgnoreCase))
                });
            }

            return _htmlHelper.DropDownListFor(expression, items, optionLabel, htmlAttributes);
        }
        #endregion
    }
}
