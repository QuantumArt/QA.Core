// Owners: Karlov Nikolay, Abretov Alexey
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using QA.Core.Logger;

namespace QA.Core.Web
{
    /// <summary>
    /// Установка текущей культуры
    /// Сначала берется инфрмация о культуре из выбранного источника.
    /// Если в источнике нет информации о культуре, 
    /// то эта информация берется из браузера.
    /// </summary>
    public class SetCultureAttribute : FilterAttribute, IActionFilter, 
        IInitilizeFilter // позволяет выполнять действия в момент инициализации контроллера
    {
        #region Enums

        /// <summary>
        /// Указывает откуда брать информацию о культуре
        /// Если культура нигде не указана, берется из браузера
        /// </summary>
        public enum CultureLocation
        {
            None = 0,
            /// <summary>
            /// Use when the culture code is saved in a cookie.  
            /// When using be sure to specify the CookieName property.
            /// </summary>
            Cookie = 1,
            /// <summary>
            /// Use when the culture code is specified in the query string.  
            /// When using be sure to specify the QueryStringParamName property.
            /// </summary>
            QueryString = 2,
            /// <summary>
            /// Use when the culture code is saved in session state.  
            /// When using be sure to specify the SessionParamName property.
            /// </summary>
            Session = 4,
            /// <summary>
            /// Use when the culture code is specified in the URL.  
            /// This assume a format of "{language}/{country}".
            /// When using be sure to specify the CountryActionParamName and 
            /// LanguageActionParamName properties.
            /// </summary>
            URL = 16
        }

        #endregion Enums

        #region Properties

        /// <summary>
        /// Определяет, когда будет вызываться установка культуры: при инициализации или в OnActionExecuting
        /// </summary>
        public bool ShouldSetOnInitialize { get; set; }

        /// <summary>
        /// The name of the cookie containing the culture code.  Specify this value when CultureStore is set to Cookie.
        /// </summary>
        public string CookieName { get; set; }
        /// <summary>
        /// The name of the action parameter containing the country code.  Specify this value when CultureStore is set to URL.
        /// </summary>
        public string CountryActionParamName { get; set; }
        /// <summary>
        /// The CultureLocation where the culture code is to be read from.  This is required to be set.
        /// </summary>
        public CultureLocation CultureStore { get; set; }
        /// <summary>
        /// The name of the action parameter containing the language code.  Specify this value when CultureStore is set to URL.
        /// </summary>
        public string LanguageActionParamName { get; set; }
        /// <summary>
        /// The name of the query string parameter containing the country code.  Specify this value when CultureStore is set to QueryString.
        /// </summary>
        public string QueryStringParamName { get; set; }
        /// <summary>
        /// The name of the session parameter containing the country code.  Specify this value when CultureStore is set to Session.
        /// </summary>
        public string SessionParamName { get; set; }

        #endregion Properties

        #region IActionFilter implementation

        public virtual void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (CultureStore == CultureLocation.Cookie)
            {
                filterContext.RequestContext
                    .HttpContext
                    .Response
                    .Cookies.Add(new HttpCookie(CookieName)
                    {
                        Value = Thread.CurrentThread.CurrentUICulture.ToString()
                    });
            }
        }
        
        public virtual void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!ShouldSetOnInitialize)
            {
                ResolveCulture(filterContext); 
            }
        }
       

        #endregion IActionFilter implementation

        #region IInitilizeFilter implementation
        public virtual void OnInitialized(ControllerContext context)
        {
            if (ShouldSetOnInitialize)
            {
                ResolveCulture(context);
            }
        }

        public virtual void OnInitializing(System.Web.Routing.RequestContext context)
        {

        }
        #endregion

        protected virtual string GetCultureCode(ControllerContext filterContext)
        {
            //Everything but CultureLocation.URL requires a valid HttpContext
            if (CultureStore != CultureLocation.URL)
            {
                if (filterContext.RequestContext.HttpContext == null)
                    return string.Empty;
            }

            string cultureCode = string.Empty;

            if (CultureStore == CultureLocation.Cookie)
            {
                if (filterContext.RequestContext.HttpContext.Request.Cookies[CookieName] != null
                    && filterContext.RequestContext.HttpContext.Request.Cookies[CookieName].Value != string.Empty)
                {
                    cultureCode = filterContext.RequestContext.HttpContext.Request.Cookies[CookieName].Value;
                }

                return cultureCode;
            }

            if (CultureStore == CultureLocation.QueryString)
            {
                cultureCode = filterContext.RequestContext.HttpContext.Request[QueryStringParamName];
                return cultureCode ?? string.Empty;
            }

            if (CultureStore == CultureLocation.Session)
            {
                if (filterContext.RequestContext.HttpContext.Session[SessionParamName] != null
                    && filterContext.RequestContext.HttpContext.Session[SessionParamName].ToString() != string.Empty)
                {
                    cultureCode = filterContext.RequestContext.HttpContext.Session[SessionParamName].ToString();
                }

                return cultureCode;
            }

            if (filterContext is ActionExecutingContext)
            {
                var actionContext = (ActionExecutingContext)filterContext;
                //if URL it is expected the URL path will contain the culture 
                if (CultureStore == CultureLocation.URL)
                {
                    if (actionContext.ActionParameters[LanguageActionParamName] != null && actionContext.ActionParameters[CountryActionParamName] != null
                        && actionContext.ActionParameters[LanguageActionParamName].ToString() != string.Empty && actionContext.ActionParameters[CountryActionParamName].ToString() != string.Empty
                        )
                    {
                        string language = actionContext.ActionParameters[LanguageActionParamName].ToString();
                        string country = actionContext.ActionParameters[CountryActionParamName].ToString();
                        cultureCode = language + "-" + country;
                    }

                    return cultureCode;
                } 
            }

            return cultureCode ?? string.Empty;
        }      

        private void ResolveCulture(ControllerContext filterContext)
        {
            if (CultureStore == CultureLocation.None)
                return;

            CultureInfo culture = null;

            string cultureCode = GetCultureCode(filterContext);

            if (string.IsNullOrEmpty(cultureCode))
            {
                culture = LocalizationHelper.ResolveCultureFromBrowser();
            }
            else
            {
                try
                {
#if DEBUG
                    if (!cultureCode.Equals("en-US",  StringComparison.OrdinalIgnoreCase) && !cultureCode.Equals("ru-RU",  StringComparison.OrdinalIgnoreCase))
                    {
                        cultureCode = "ru-RU";
                    }
#endif

                    culture = new CultureInfo(cultureCode);
                    Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;
                }
                catch (Exception ex)
                {
                    var logger = ObjectFactoryBase.Resolve<ILogger>();
                    if (logger != null)
                    {
                        logger.ErrorException("Неверный код cookie для культуры: " +  cultureCode, ex);
                    }

                    //throw;
                }
            }
        }
    }
}
