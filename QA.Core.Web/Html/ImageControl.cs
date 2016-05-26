using System.Text;
using System.Web.Mvc;
using System.Web.UI;

namespace QA.Core.Web.Html
{
    /// <summary>
    /// Контрол картинка
    /// </summary>
    public class ImageControl : HtmlTagControl
    {
        /// <summary>
        /// Конструирует объект
        /// </summary>
        /// <param name="viewContext"></param>
        public ImageControl(
            ViewContext viewContext)
            : base(viewContext)
        {
            this
                .SetTagType(HtmlTextWriterTag.Img);
        }

        /// <summary>
        /// Url к картинке
        /// </summary>
        public string Src { get; set; }

        /// <summary>
        /// Альтернативная надпись
        /// </summary>
        public string Alt { get; set; }

        /// <summary>
        /// Всплывающая подсказка
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Возвращает Html
        /// </summary>
        /// <returns></returns>
        public override MvcHtmlString GetHtmlString()
        {
            this.SetHtmlAttributes(
                new { src = Src, alt = Alt, title = Title });

            var start = HtmlControlExtensions.RenderTag(
                this, TagRenderMode.SelfClosing);

            var sb = new StringBuilder();
            sb.Append(start);

            return MvcHtmlString.Create(sb.ToString());
        }
    }
}
