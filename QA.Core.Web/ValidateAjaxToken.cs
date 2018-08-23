using System;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
#pragma warning disable 1591

namespace QA.Core.Web
{
    /// <summary>
    /// Аттрибут для проверки токена AntiForgery в заголовке запроса
    /// </summary>
    public class ValidateAjaxToken : FilterAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// Ключ в заголовке запроса и cookie по умолчанию
        /// </summary>
        public const string DefaultHeaderKey = @"__RequestVerificationToken";

        #region Fields
        private string _headerKey;
        private bool _isCookieLess;

        #endregion

        #region Properties
        /// <summary>
        /// Ключ в заголовке запроса и cookie
        /// </summary>
        public string HeaderKey
        {
            get { return _headerKey; }
            set { _headerKey = value; }
        }

        public bool DoNotThrow { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// конструктор
        /// </summary>
        public ValidateAjaxToken() : this(false) { }

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="isCookieLess"></param>
        public ValidateAjaxToken(bool isCookieLess)
        {
            _isCookieLess = isCookieLess;
            _headerKey = DefaultHeaderKey;
        }
        #endregion

        #region IAuthorizationFilter Members
        void IAuthorizationFilter.OnAuthorization(AuthorizationContext filterContext)
        {
            ValidateRequestHeader(filterContext.HttpContext);
        }
        #endregion

        #region Private members
        void ValidateRequestHeader(HttpContextBase ctx)
        {
            string cookieToken = "";
            string formToken = "";

            formToken = ctx.Request.Headers[_headerKey] ?? string.Empty;

            if (_isCookieLess)
            {
                var tokens = formToken.Split(':')
                    .ToArray();

                if (tokens.Length == 2)
                {
                    cookieToken = tokens[0];
                    formToken = tokens[1];
                }
            }
            else
            {
                var cookie = ctx.Request.Cookies.Get(_headerKey);

                if (cookie != null && !string.IsNullOrWhiteSpace(cookie.Value))
                {
                    cookieToken = cookie.Value;
                }
            }


            try
            {
                AntiForgery.Validate(cookieToken, formToken);
                ctx.Items[DefaultHeaderKey] = "valid";
            }
            catch (Exception ex)
            {
                ctx.Items[DefaultHeaderKey] = "invalid";

                if (!DoNotThrow)
                {
                    throw ex;
                }
            }
        }
        #endregion
    }

    public static class ValidateAjaxControllerExtensions
    {
        /// <summary>
        /// Проверить валидность ajax-токена
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static bool IsAjaxTokenValid(this ControllerBase controller)
        {
            return (string)controller.ControllerContext
                .HttpContext
                .Items[ValidateAjaxToken.DefaultHeaderKey] == "valid";
        }
    }
}
