using System.Web.Mvc;
#pragma warning disable 1591

namespace QA.Core.Web
{
    /// <summary>
    /// Атрибут только для ajax запросов
    /// </summary>
    public class AjaxOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = 404;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
