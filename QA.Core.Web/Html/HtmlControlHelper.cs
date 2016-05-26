using System.Text;
using System.Web.Mvc;
using System.Web.UI;

namespace QA.Core.Web.Html
{
    /// <summary>
    /// Методы для контролов
    /// </summary>
    public class HtmlControlHelper
    {
        /// <summary>
        /// Генерируте html-строку
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static MvcHtmlString GetHtmlString(
            HtmlTagControl control)
        {
            var sb = new StringBuilder();

            if (control.TagType == HtmlTextWriterTag.Unknown)
            {
                sb.Append(control.InnerHtml);

                if (control.Controls != null & control.Controls.Count > 0)
                {
                    foreach (var c in control.Controls)
                    {
                        sb.Append(c.GetHtmlString());
                    }
                }
            }
            else
            {
                bool hasClosedTag = true;

                if (control.TagType == HtmlTextWriterTag.Img ||
                    control.TagType == HtmlTextWriterTag.Input)
                {
                    hasClosedTag = false;
                }

                var start = HtmlControlExtensions.RenderTag(
                    control, hasClosedTag ? TagRenderMode.StartTag : TagRenderMode.SelfClosing);
                var end = HtmlControlExtensions.RenderTag(
                    control, TagRenderMode.EndTag);

                sb.Append(start);
                sb.Append(control.InnerHtml);

                if (control.Controls != null & control.Controls.Count > 0)
                {
                    foreach (var c in control.Controls)
                    {
                        sb.Append(c.GetHtmlString());
                    }
                }

                if (hasClosedTag)
                {
                    sb.Append(end);
                }
            }

            return MvcHtmlString.Create(sb.ToString());
        }
    }
}
