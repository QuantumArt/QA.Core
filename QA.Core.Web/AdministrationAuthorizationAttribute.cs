using System.Web;
using System.Web.Mvc;

namespace QA.Core.Web
{
    /// <summary>
    /// Проверка авторизации
    /// </summary>
    public class AdministrationAuthorizationAttribute : AuthorizeAttribute
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
                filterContext.Result = new HttpUnauthorizedResult("401");
            }
        }
    }
}
