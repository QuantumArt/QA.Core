using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace QA.Core.Web
{
    /// <summary>
    /// Html-хелпер для вставки antiforgery tokens 
    /// </summary>
    public static class AntiForgeryExtensions
    {
        /// <summary>
        /// Ключ
        /// </summary>
        public const string DefaultHeaderKey = @"__RequestVerificationToken";

        /// <summary>
        /// Получить токен безопасности для текущего запроса
        /// </summary>
        /// <param name="html">хелпер</param>
        /// <param name="isCookieLess">использовать ли cookie</param>
        /// <param name="headerKey">опциональный ключ</param>
        /// <returns></returns>
        public static string GetAntiForgeryToken(this HtmlHelper html, bool isCookieLess = false, string headerKey = null)
        {
            string result = string.Empty;
            var oldCookieToken = string.Empty;
            var newCookieToken = string.Empty;
            var key = headerKey ?? ValidateAjaxToken.DefaultHeaderKey;

            if (!TryGetToken(ref oldCookieToken, key, html.ViewContext.HttpContext.Request.Cookies))
            {
                if (!TryGetToken(ref oldCookieToken, key, html.ViewContext.HttpContext.Response.Cookies))
                {
                }
            }

            AntiForgery.GetTokens(oldCookieToken, out newCookieToken, out result);

            if (!string.IsNullOrWhiteSpace(newCookieToken))
            {
                html.ViewContext.HttpContext.Response.SetCookie(new HttpCookie(key, newCookieToken)
                {
                    Shareable = true
                });
            }

            if (isCookieLess)
            {
                result = string.Format("{0}:{1}", newCookieToken ?? oldCookieToken, result);
            }

            return result;
        }

        private static bool TryGetToken(ref string cookieToken, string key, HttpCookieCollection collection)
        {
            var cookie = collection.Get(key);

            if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
            {
                cookieToken = cookie.Value;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Получить текущий ключ
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string GetAntiForgeryHeaderKey(this HtmlHelper html)
        {
            return ValidateAjaxToken.DefaultHeaderKey;
        }
    }
}
