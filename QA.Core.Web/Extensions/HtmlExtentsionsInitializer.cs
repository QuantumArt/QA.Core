using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Web.WebPages;
using QA.Core.Web.Extensions;
#pragma warning disable 1591


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

        public static HtmlHelper GetPageHelper(this System.Web.WebPages.Html.HtmlHelper html)
        {
            return ((System.Web.Mvc.WebViewPage)WebPageContext.Current.Page).Html;
        }

        public static WebViewPage GetMvcPage(this System.Web.WebPages.Html.HtmlHelper html)
        {
            return ((WebViewPage)WebPageContext.Current.Page);
        }

        public static string GetJson(this HtmlHelper helper, object model, bool encode = false)
        {
            string result = "";

            if (model != null)
            {
                result = JsonConvert.SerializeObject(model);

                if (encode && result != null)
                {
                    result = result.Replace('\"', '\'');
                    result = HttpUtility.JavaScriptStringEncode(result);
                }
            }

           return result;
        }
    }
}
