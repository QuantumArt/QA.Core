using System.Web.Mvc;
using System.Web.WebPages;
using QA.Core.Web.Extensions;

namespace QA.Core.Web
{
    public static class HtmlExtentsionsInitializer
    {
        public static DateTimeExtensions<TModel> DateTim1e<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            return new DateTimeExtensions<TModel>(htmlHelper);
        }

        public static OverlayExtention Overlay(this HtmlHelper htmlHelper)
        {
            return new OverlayExtention(htmlHelper);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static HtmlHelper GetPageHelper(this System.Web.WebPages.Html.HtmlHelper html)
        {
            return ((System.Web.Mvc.WebViewPage)WebPageContext.Current.Page).Html;
        }

        public static System.Web.Mvc.WebViewPage GetMvcPage(this System.Web.WebPages.Html.HtmlHelper html)
        {
            return ((System.Web.Mvc.WebViewPage)WebPageContext.Current.Page);
        }
    }
}
