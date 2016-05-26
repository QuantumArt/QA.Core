// Owners: Karlov Nikolay, Abretov Alexey
using System.Web.Mvc;

namespace QA.Core.Web
{
    /// <summary>
    /// Релультат, который перенаправляет на другую страницу в случае ajax-запроса
    /// </summary>
    public class AjaxRedirectResult : RedirectResult
    {
        public AjaxRedirectResult(string url, ControllerContext controllerContext)
            : base(url)
        {
            ExecuteResult(controllerContext);
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                JavaScriptResult result = new JavaScriptResult()
                {
                    Script = string.Format(redirectionScript,  UrlHelper.GenerateContentUrl(this.Url, context.HttpContext))
                };

                result.ExecuteResult(context);
            }
            else
            {
                base.ExecuteResult(context);
            }
        }

        // скрипт, который меняет текущую страницу
        const string redirectionScript = "try{{history.pushState(null,null,window.location.href);}}catch(err){{}}window.location.replace('{0}');";
    }
}

