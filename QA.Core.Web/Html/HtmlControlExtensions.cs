using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI;
using System.Web.WebPages;

namespace QA.Core.Web.Html
{
    /// <summary>
    /// Расширения для html контролов
    /// </summary>
    public static class HtmlControlExtensions
    {
        private static readonly object _o = new object();

        #region Render tags

        /// <summary>
        /// Отрисовывает открывающий тэг
        /// </summary>
        /// <typeparam name="TModel">Тип модели</typeparam>
        /// <param name="helper">Фабрика</param>
        /// <param name="tagType">Тип тэга</param>
        /// <param name="isDisabled"></param>
        /// <param name="htmlAttributes">Атрибуты</param>
        /// <returns></returns>
        public static HtmlTagControl<TModel> BeginHtmlTag<TModel>(
            this HtmlControlFactory<TModel> helper,
            HtmlTextWriterTag tagType,
            bool isDisabled,
            object htmlAttributes)
        {
            var tag = new HtmlTagControl<TModel>(helper.HtmlHelper.ViewContext)
                .SetTagType(tagType)
                .SetHtmlAttributes(htmlAttributes);

            tag.Factory = helper;

            if (isDisabled)
            {
                tag = tag.SetDisabled();
            }

            helper.HtmlHelper.ViewContext.Writer.Write(
                RenderTag(tagType, htmlAttributes, null, isDisabled, TagRenderMode.StartTag));

            return tag;
        }

        /// <summary>
        /// Отрисовывает открывающий тэг
        /// </summary>
        /// <typeparam name="TModel">Тип модели</typeparam>
        /// <typeparam name="TProperty">Тип свойства</typeparam>
        /// <param name="helper">Фабрика</param>
        /// <param name="tagType">Тип тэга</param>
        /// <param name="expression">Выражение для значения</param>
        /// <param name="isDisabled"></param>
        /// <param name="htmlAttributes">Атрибуты</param>
        /// <returns></returns>
        public static HtmlTagControl<TModel> BeginHtmlTagFor<TModel, TProperty>(
            this HtmlControlFactory<TModel> helper,
            HtmlTextWriterTag tagType,
            Expression<Func<TModel, TProperty>> expression,
            bool isDisabled,
            object htmlAttributes)
        {
            var valueGetter = expression.Compile();
            var value = valueGetter(helper.HtmlHelper.ViewData.Model);

            var tag = new HtmlTagControl<TModel>(helper.HtmlHelper.ViewContext)
                .SetValue(value == null ? string.Empty : value.ToString())
                .SetTagType(tagType)
                .SetHtmlAttributes(htmlAttributes);

            tag.Factory = helper;

            if (isDisabled)
            {
                tag = tag.SetDisabled();
            }

            helper.HtmlHelper.ViewContext.Writer.Write(
                RenderTag(tagType, htmlAttributes, value, isDisabled, TagRenderMode.StartTag));

            return tag;
        }

        /// <summary>
        /// Отрисовывает полный тэг
        /// </summary>
        /// <typeparam name="TModel">Тип модели</typeparam>
        /// <param name="helper">Фабрика</param>
        /// <param name="tagType">Тип тэга</param>
        /// <param name="htmlAttributes">Атрибуты</param>
        /// <returns></returns>
        public static HtmlTagControl HtmlTag<TModel>(
            this HtmlControlFactory<TModel> helper,
            HtmlTextWriterTag tagType,
            object htmlAttributes)
        {
            var tag = new HtmlTagControl(helper.HtmlHelper.ViewContext)
                .SetTagType(tagType)
                .SetHtmlAttributes(htmlAttributes);

            helper.HtmlHelper.ViewContext.Writer.Write(
                RenderTag(tagType, htmlAttributes, null, false, TagRenderMode.Normal));

            return tag;
        }

        /// <summary>
        /// Отрисовывает полный тэг
        /// </summary>
        /// <typeparam name="TModel">Тип модели</typeparam>
        /// <typeparam name="TProperty">Тип свойства</typeparam>
        /// <param name="helper">Фабрика</param>
        /// <param name="tagType">Тип тэга</param>
        /// <param name="expression">Выражение для значения</param>
        /// <param name="htmlAttributes">Атрибуты</param>
        /// <returns></returns>
        public static HtmlTagControl<TModel> HtmlTagFor<TModel, TProperty>(
           this HtmlControlFactory<TModel> helper,
           HtmlTextWriterTag tagType,
           Expression<Func<TModel, TProperty>> expression,
           object htmlAttributes)
        {
            var valueGetter = expression.Compile();
            var value = valueGetter(helper.HtmlHelper.ViewData.Model);

            var tag = new HtmlTagControl<TModel>(helper.HtmlHelper.ViewContext)
                .SetValue(value == null ? string.Empty : value.ToString())
                .SetTagType(tagType)
                .SetHtmlAttributes(htmlAttributes);

            helper.HtmlHelper.ViewContext.Writer.Write(
                RenderTag(tagType, htmlAttributes, value, false, TagRenderMode.Normal));

            return tag;
        }

        /// <summary>
        /// Отрисовывает закрывающий тэг
        /// </summary>
        /// <param name="viewContext">Контекст представления</param>
        /// <param name="tagType">Тип тэга</param>
        public static void EndHtmlTag(
            ViewContext viewContext,
            HtmlTextWriterTag tagType)
        {
            viewContext.Writer.Write(RenderTag(tagType, null, null, false, TagRenderMode.EndTag));
        }

        /// <summary>
        /// Отрисовывает тэг
        /// </summary>
        /// <param name="tagType">Тип тэга</param>
        /// <param name="htmlAttributes">Атрибуты</param>
        /// <param name="val">Значение</param>
        /// <param name="isDisabled"></param>
        /// <param name="renderMode">Режим отрисовки тэга</param>
        /// <returns></returns>
        public static MvcHtmlString RenderTag(
            HtmlTextWriterTag tagType,
            object htmlAttributes,
            object val,
            bool isDisabled,
            TagRenderMode renderMode)
        {
            var tag = new TagBuilder(tagType.ToString().ToLower());

            if (renderMode != TagRenderMode.EndTag)
            {
				tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));

                if (val != null)
                {
                    tag.MergeAttribute("value", val.ToString(), true);
                }

                if (isDisabled)
                {
                    tag.MergeAttribute("disabled", "disabled", true);
                }
            }

            return MvcHtmlString.Create(tag.ToString(renderMode));
        }

        /// <summary>
        /// Отрисовывает тэг
        /// </summary>
        /// <param name="control">Контрол на основании, которого отрисовывается тэг</param>
        /// <param name="renderMode">Режим отрисовки тэга</param>
        /// <returns></returns>
        public static MvcHtmlString RenderTag(
            HtmlTagControl control,
            TagRenderMode renderMode)
        {
            var tag = new TagBuilder(control.TagType.ToString().ToLower());

            if (renderMode != TagRenderMode.EndTag)
            {
                if (control.HtmlAttributes is IDictionary<string, object>)
                {
                    tag.MergeAttributes(new RouteValueDictionary((IDictionary<string, object>)control.HtmlAttributes));
                }
                else
                {
                    tag.MergeAttributes(new RouteValueDictionary(control.HtmlAttributes));
                }
            }

            if (!string.IsNullOrEmpty(control.Value))
            {
                tag.MergeAttribute("value", control.Value.ToString(), true);
            }

            if (control.IsDisabled)
            {
                tag.MergeAttribute("disabled", "disabled", true);
            }

            if (!string.IsNullOrEmpty(control.Type))
            {
                tag.MergeAttribute("type", control.Type, true);
            }

            if (!string.IsNullOrEmpty(control.Id))
            {
                tag.MergeAttribute("id", control.Id, true);
            }

            if (!string.IsNullOrEmpty(control.Name))
            {
                tag.MergeAttribute("name", control.Name, true);
            }

            if (!string.IsNullOrEmpty(control.CssClass))
            {
                tag.MergeAttribute("class", control.CssClass, true);
            }

            return MvcHtmlString.Create(tag.ToString(renderMode));
        }

        #endregion

        #region Controls settings

        #region Add

        /// <summary>
        /// Добавляет подчиненный контрол
        /// </summary>
        /// <typeparam name="T">Тип контрола</typeparam>
        /// <param name="control">Контрол</param>
        /// <param name="tagType">Тип тэга добавляемого контрола</param>
        /// <param name="htmlAttributes">Атрибуты</param>
        /// <returns></returns>
        public static T Add<T>(
            this T control,
            HtmlTextWriterTag tagType,
            object htmlAttributes) where T : HtmlTagControl
        {
            control
                .Controls
                .Add(new HtmlTagControl(control.ViewContext)
                    .SetTagType(tagType)
                    .SetHtmlAttributes(htmlAttributes));

            return control;
        }

        /// <summary>
        /// Добавляет подчиненный контрол
        /// </summary>
        /// <typeparam name="T">Тип контрола</typeparam>
        /// <param name="control">Контрол</param>
        /// <param name="innerControl">Добавляемый контрол</param>
        /// <returns></returns>
        public static T Add<T>(
            this T control,
            HtmlTagControl innerControl) where T : HtmlTagControl
        {
            control
                .Controls
                .Add(innerControl);

            return control;
        }

        /// <summary>
        /// Добавляет массив контролов
        /// </summary>
        /// <typeparam name="T">Тип контрола</typeparam>
        /// <param name="control">Контрол</param>
        /// <param name="innerControls">Добавляемые контролы</param>
        /// <returns></returns>
        public static T AddRange<T>(
            this T control,
            HtmlTagControl[] innerControls) where T : HtmlTagControl
        {
            control
                .Controls
                .AddRange(innerControls);

            return control;
        }

        #endregion

        /// <summary>
        /// Устанавливает признак недоступности контрола
        /// </summary>
        /// <typeparam name="T">Тип контрола</typeparam>
        /// <param name="control">Контрол</param>
        /// <returns></returns>
        public static T SetDisabled<T>(
            this T control) where T : HtmlTagControl
        {
            control.IsDisabled = true;

            return control;
        }

        /// <summary>
        /// Устанавливает значение контрола
        /// </summary>
        /// <typeparam name="T">Тип контрола</typeparam>
        /// <param name="control">Контрол</param>
        /// <param name="val">Значение</param>
        /// <returns></returns>
        public static T SetValue<T>(
            this T control,
            string val) where T : HtmlTagControl
        {
            control.Value = val;

            return control;
        }

        /// <summary>
        /// Устанавливает внутренний html контрола
        /// </summary>
        /// <typeparam name="T">Тип контрола</typeparam>
        /// <param name="control">Контрол</param>
        /// <param name="val">Значение</param>
        /// <returns></returns>
        public static T SetInnerHtml<T>(
            this T control,
            string val) where T : HtmlTagControl
        {
            control.InnerHtml = val;

            return control;
        }

        /// <summary>
        /// Устанавливает внутренний html контрола
        /// </summary>
        /// <typeparam name="T">Тип контрола</typeparam>
        /// <param name="control">Контрол</param>
        /// <param name="action">Значение</param>
        /// <returns></returns>
        public static T SetInnerHtml<T>(
           this T control,
           Func<object, HelperResult> action) where T : HtmlTagControl
        {
            control.InnerHtml = action(_o).ToHtmlString();

            return control;
        }

        /// <summary>
        /// Устанавливает тип тэга Input
        /// </summary>
        /// <typeparam name="T">Тип контрола</typeparam>
        /// <param name="control">Контрол</param>
        /// <param name="type">Тип</param>
        /// <returns></returns>
        public static T SetType<T>(
            this T control,
            string type) where T : HtmlTagControl
        {
            control.Type = type;

            return control;
        }

        /// <summary>
        /// Устанавливает тип тэга контрола
        /// </summary>
        /// <typeparam name="T">Тип контрола</typeparam>
        /// <param name="control">Контрол</param>
        /// <param name="type">Тип тэга</param>
        /// <returns></returns>
        public static T SetTagType<T>(
            this T control,
            HtmlTextWriterTag type) where T : HtmlTagControl
        {
            control.TagType = type;

            return control;
        }

	    /// <summary>
	    /// Устанавливает дополнительные атрибуты контрола
	    /// </summary>
	    /// <typeparam name="T">Тип контрола</typeparam>
	    /// <param name="control">Контрол</param>
	    /// <param name="htmlAttributes">Атрибуты</param>
	    /// <param name="replaceUnderscopes">автоматом заменять в именах подчеркивание на дефис</param>
	    /// <returns></returns>
	    public static T SetHtmlAttributes<T>(this T control, object htmlAttributes, bool replaceUnderscopes = false) where T : HtmlTagControl
        {
	        if (replaceUnderscopes)
	        {
		        var attrsDic = htmlAttributes as IDictionary<string, object> ?? new RouteValueDictionary(htmlAttributes);

				var invalidKeys = attrsDic.Where(x => x.Key.Contains("_")).ToArray();

		        if (invalidKeys.Any())
		        {
					foreach (KeyValuePair<string, object> invalidKey in invalidKeys)
					{
						attrsDic.Remove(invalidKey.Key);

						attrsDic[invalidKey.Key.Replace('_', '-')] = invalidKey.Value;
					}

					htmlAttributes = attrsDic;
		        }
	        }

            control.HtmlAttributes = htmlAttributes;

            return control;
        }

        /// <summary>
        /// Устанавливает идентификатор и наименование контрола
        /// </summary>
        /// <typeparam name="T">Тип контрола</typeparam>
        /// <param name="control">Контрол</param>
        /// <param name="id">Идентификатор и наименование</param>
        /// <returns></returns>
        public static T SetId<T>(
            this T control,
            string id) where T : HtmlTagControl
        {
            if (string.IsNullOrEmpty(control.Name))
            {
                control.Name = id;
            }

            control.Id = id;

            return control;
        }

        /// <summary>
        /// Устанавливает наименование контрола
        /// </summary>
        /// <typeparam name="T">Тип контрола</typeparam>
        /// <param name="control">Контрол</param>
        /// <param name="name">Наименование</param>
        /// <returns></returns>
        public static T SetName<T>(
            this T control,
            string name) where T : HtmlTagControl
        {
            control.Name = name;

            return control;
        }

        /// <summary>
        /// Устанавливает класс стиля
        /// </summary>
        /// <typeparam name="T">Тип контрола</typeparam>
        /// <param name="control">Контрол</param>
        /// <param name="className">Имя класса</param>
        /// <returns></returns>
        public static T SetClass<T>(
            this T control,
            string className) where T : HtmlTagControl
        {
            control.CssClass = className;

            return control;
        }

        /// <summary>
        /// Добавляет класс стиля
        /// </summary>
        /// <typeparam name="T">Тип контрола</typeparam>
        /// <param name="control">Контрол</param>
        /// <param name="className">Имя класса</param>
        /// <returns></returns>
        public static T AddClass<T>(
           this T control,
           string className) where T : HtmlTagControl
        {
            control.CssClass = string.Join(" ", control.CssClass, className)
                .Trim();

            return control;
        }

        #endregion
    }
}
