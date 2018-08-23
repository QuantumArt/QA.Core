using System.Web;
using System.Web.Mvc;
#pragma warning disable 1591

namespace QA.Core.Web
{
    /// <summary>
    /// Проверка авторизации
    /// </summary>
    public class RequireCustomActionAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return AdministrationAuthorizationHelper.IsAuthorize();
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new ContentResult();
                filterContext.HttpContext.Response.StatusCode = 401;
            }
            else
            {
                filterContext.Result = new HttpUnauthorizedResult("Access to the page is restricted as it is a QP8 custom action.");
            }
        }
    }
}
