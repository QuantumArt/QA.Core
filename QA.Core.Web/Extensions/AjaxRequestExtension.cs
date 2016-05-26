// Owners: Karlov Nikolay, Abretov Alexey
using System.Web;

namespace QA.Core.Web
{
    /// <summary>
    /// Расширение класса HttpRequest
    /// </summary>
    public static class AjaxRequestExtension
    {
        /// <summary>
        /// определяет является ли запрос ajax-запросом
        /// </summary>
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            return (request.Headers["X-Requested-With"] != null && request.Headers["X-Requested-With"] == "XMLHttpRequest");
        }
    }
}
