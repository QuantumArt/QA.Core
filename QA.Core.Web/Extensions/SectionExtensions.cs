// Owners: Karlov Nikolay, Abretov Alexey

using System;
using System.Web.WebPages;
using System.Web.Mvc.Html;
using System.Web.Mvc;
namespace QA.Core.Web
{
    /// <summary>
    /// Расширения для разметки razor
    /// </summary>
    public static class SectionExtension
    {
        private static readonly object _o = new object();

        /// <summary>
        /// Рендеринг частичного представления с изменяемым контентом
        /// </summary>
        /// <param name="page"></param>
        /// <param name="partialViewName"></param>
        /// <param name="defaultContent"></param>
        /// <returns></returns>
        public static MvcHtmlString Partial(this HtmlHelper page, string partialViewName, Func<object, HelperResult> defaultContent)
        {
            return page.Partial(partialViewName, MvcHtmlString.Create(defaultContent(_o).ToHtmlString()));
        }

        /// <summary>
        /// Позволяет регистрировать секции с предустановленным контентом по умолчанию
        /// </summary>
        /// <param name="page"></param>
        /// <param name="sectionName">название секции</param>
        /// <param name="defaultContent">контент по умолчанию</param>
        /// <returns></returns>
        public static HelperResult RenderSection(this WebPageBase page,
                            string sectionName,
                            Func<object, HelperResult> defaultContent)
        {
            if (page.IsSectionDefined(sectionName))
            {
                return page.RenderSection(sectionName);
            }
            else
            {
                return defaultContent(_o);
            }
        }

        /// <summary>
        /// Переопределение секции
        /// </summary>
        /// <param name="page"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public static HelperResult RedefineSection(this WebPageBase page,
                           string sectionName)
        {
            return RedefineSection(page, sectionName, defaultContent: null);
        }

        /// <summary>
        /// Переопределение секции
        /// </summary>
        /// <param name="page"></param>
        /// <param name="sectionName"></param>
        /// <param name="defaultContent"></param>
        /// <returns></returns>
        public static HelperResult RedefineSection(this WebPageBase page,
                                string sectionName,
                                Func<object, HelperResult> defaultContent)
        {
            if (page.IsSectionDefined(sectionName))
            {
                page.DefineSection(sectionName,
                                   () => page.Write(page.RenderSection(sectionName)));
            }
            else if (defaultContent != null)
            {
                page.DefineSection(sectionName,
                                   () => page.Write(defaultContent(_o)));
            }
            return new HelperResult(_ => { });
        }
    }

}
