using System.Web.Mvc;

namespace QA.Core.Web.Html
{
    /// <summary>
    /// Контрол текст
    /// </summary>
    public class RawTextControl : HtmlTagControl
    {
        /// <summary>
        /// Конструирует объект
        /// </summary>
        /// <param name="viewContext"></param>
        public RawTextControl(
            ViewContext viewContext)
            : base(viewContext)
        {
        }

        /// <summary>
        /// Текст
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Возвращает html-строку
        /// </summary>
        /// <returns></returns>
        public override MvcHtmlString GetHtmlString()
        {
            return MvcHtmlString.Create(Text);
        }
    }
}
