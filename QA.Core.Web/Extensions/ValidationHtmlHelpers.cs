// Owners: Karlov Nikolay, Abretov Alexey

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;

namespace QA.Core.Web
{
    /// <summary>
    /// Методы-расширения, касающиеся отображения сообщений о валидации
    /// </summary>
    public static class ValidationHtmlHelpers
    {
        public const string TempDataViewDataKey = "TempDataViewDataKey";
        /// <summary>
        /// Создает элемент для отображения сообщений о валидации.
        /// Сами сообщения не добавляются (если не указан innerHtml)
        /// </summary>
        /// <param name="elementTagName">название тега</param>
        /// <param name="htmlAttributes">html-аттрибуты</param>
        /// <param name="innerHtml">текст или Html-разметка внутри блока</param>
        /// <param name="validationAttribute">название html-аттрибута, который будет выставляться у блока</param>
        /// <returns></returns>
        public static MvcHtmlString ValidationBlockFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            string tagName = null, string className = null,
            string innerHtml = null,
            Dictionary<string, string> htmlAttributes = null,
            string validationAttribute = "validation-for")
        {
            var propertyPath = htmlHelper.ViewContext
                .ViewData.TemplateInfo
                .GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));

            return ValidationHelper<TModel>(htmlHelper, propertyPath, tagName, className, innerHtml, htmlAttributes, validationAttribute);
        }

        public static MvcHtmlString ValidationBlock<TModel>(this HtmlHelper<TModel> htmlHelper,
           string propertyPath,
           string tagName = null, string className = null,
           string innerHtml = null,
           Dictionary<string, string> htmlAttributes = null,
           string validationAttribute = "validation-for")
        {
            return ValidationHelper<TModel>(htmlHelper, propertyPath, tagName, className, innerHtml, htmlAttributes, validationAttribute);
        }

        /// <summary>
        /// Кастомизированное сообщение о валидации
        /// </summary>
        /// <param name="className">html-класс/классы</param>
        /// <param name="alternativeText">альтернативный текст. Если null или пусто, то сообщение берется из ModelState</param>
        /// <param name="elementTagName">название тега</param>
        /// <param name="hideOnValidFields">скрывать ли элемент при отсутсвии ошибок и/или сообщений</param>
        /// <param name="htmlAttributes">html-аттрибуты</param>
        /// <param name="innerHtml">текст или Html-разметка внутри блока</param>
        /// <param name="validationAttribute">название html-аттрибута, который будет выставляться у блока</param>
        /// <returns></returns>
        public static MvcHtmlString ValidationFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            string elementTagName = null, string className = null,
            bool hideOnValidFields = true,
            string alternativeText = null,
            Dictionary<string, string> htmlAttributes = null,
            string validationAttribute = "validation-for")
        {
            string text = null;
            string tagName = elementTagName;

            if (string.IsNullOrEmpty(tagName))
            {
                tagName = "div";
            }

            var modelName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));

            if (!string.IsNullOrEmpty(alternativeText))
            {
                return ValidationHelper(htmlHelper, modelName, tagName, className, alternativeText, htmlAttributes, validationAttribute);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(modelName))
                {
                    ModelState modelState = htmlHelper.ViewData.ModelState[modelName];

                    ModelErrorCollection modelErrors = (modelState == null) ? null : modelState.Errors;
                    ModelError modelError = ((modelErrors == null) || (modelErrors.Count == 0)) ? null : modelErrors[0];

                    if (modelError != null)
                    {
                        text = modelError.ErrorMessage;

                        return ValidationBlockFor(htmlHelper, expression, tagName, className, text, htmlAttributes, validationAttribute);
                    }
                }

            }

            if (!hideOnValidFields)
            {
                return ValidationBlockFor(htmlHelper, expression, tagName, className, text, htmlAttributes, validationAttribute);
            }

            return null;
        }

        /// <summary>
        /// Проверяет верно ли заполнено свойство модели
        /// </summary>
        /// <param name="_htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsValid<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            var modelName = ExpressionHelper.GetExpressionText(expression);

            if (!string.IsNullOrWhiteSpace(modelName))
            {
                ModelState modelState = htmlHelper.ViewData.ModelState[modelName];

                ModelErrorCollection modelErrors = (modelState == null) ? null : modelState.Errors;
                ModelError modelError = ((modelErrors == null) || (modelErrors.Count == 0)) ? null : modelErrors[0];

                return (modelError == null);
            }

            return true;
        }

        /// <summary>
        /// Размещение скрытого поля для хранения временной информации
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static MvcHtmlString TempDataStorage(this HtmlHelper helper)
        {
            if (helper.ViewData.ContainsKey(TempDataViewDataKey))
            {
                var tb = new TagBuilder("input");

                tb.Attributes["type"] = "hidden";
                tb.Attributes["name"] = TempDataViewDataKey;
                tb.GenerateId(TempDataViewDataKey);
                tb.Attributes["value"] = helper.ViewData[TempDataViewDataKey] as string;

                return MvcHtmlString.Create(tb.ToString(TagRenderMode.Normal));
            }

            return null;
        }

        #region Private members
        private static MvcHtmlString ValidationHelper<TModel>(HtmlHelper<TModel> htmlHelper,
            string propertyPath, string tagName, string className, string innerHtml, Dictionary<string, string> htmlAttributes, string validationAttribute)
        {
            StringBuilder sb = new StringBuilder();

            var elementTagName = tagName;
            if (string.IsNullOrEmpty(elementTagName))
            {
                elementTagName = "div";
            }

            TagBuilder tb = new TagBuilder(elementTagName);

            if (htmlAttributes != null)
            {
                tb.MergeAttributes(htmlAttributes);
            }

            if (innerHtml == null)
            {
                tb.InnerHtml = "&nbsp;";
            }
            else
            {
                tb.InnerHtml = innerHtml;
            }

            if (!string.IsNullOrEmpty(className))
            {
                tb.AddCssClass(className);
            }

            tb.Attributes[validationAttribute] = propertyPath;

            return MvcHtmlString.Create(tb.ToString(TagRenderMode.Normal));
        } 
        #endregion
    }
}
