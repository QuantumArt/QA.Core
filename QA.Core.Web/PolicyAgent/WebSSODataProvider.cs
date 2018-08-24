// Owners: Karlov Nikolay, Abretov Alexey
using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using QA.Core.Logger;
#pragma warning disable 1591


namespace QA.Core.Web
{
    /// <summary>
    /// Потавщик данных от полиси-агента
    /// <remarks>Получаент данные от хендлера</remarks>
    /// </summary>
    public class WebSSODataProvider : IWebSSODataProvider
    {
        const string SessionKey = "E93A3F99-B9A3-4840-BEEA-6698A91A391B";
        const string SessionCookie = "86778850-73EB-4704-9D12-5420D2B3C48C";
        const string EmbeddedHeaderKey = "PA-Embedded-State";
        const string HandlerHeaderKey = "PA-Handler-State";
        const string MonitorRequestIdKey = "monitorRequestId";

        public Lazy<ILogger> Logger = new Lazy<ILogger>(() => ObjectFactoryBase.ClientLogger);

        /// <summary>
        /// Получение данных пользователя-абонента МТС
        /// </summary>
        /// <remarks>Передает cookie</remarks>
        /// <param name="context">Контекст запроса</param>
        /// <param name="saveInSession"></param>
        /// <exception cref="System.Exception">ВСе типы исключений при работе с HttpWebRequest</exception>
        /// <returns></returns>
        public WebSSOData GetUserData(HttpContextBase context, bool saveInSession)
        {
            string url = null;
            string originalUrl = null;
            string requestId = Guid.NewGuid().ToString();
            string clientRequestId = context.Request.QueryString[MonitorRequestIdKey];
            requestId = string.IsNullOrEmpty(clientRequestId) ? requestId : clientRequestId;

            try
            {
                WebSSOData result = null;
                CookieCollection cookies = null;
                HttpSessionState session = null;

                if (saveInSession)
                {
                    result = (WebSSOData)context.Session[SessionKey];
                    cookies = (CookieCollection)context.Session[SessionCookie];
                    session = context.ApplicationInstance.Session;
                }

                if (result == null || string.IsNullOrEmpty(result.Login))
                {
                    result = new WebSSOData();

                    var urlHelper = new UrlHelper(context.Request.RequestContext);

                    // добавляем querystring
                    url = urlHelper.ContentAbsolute(string.Format(WebSSOConfiguration.AddressToHandler, requestId, DateTime.Now.ToBinary()));

                    originalUrl = context.Request.Url.ToString();

                    var client = (HttpWebRequest)WebRequest.Create(url);

                    client.CookieContainer = new CookieContainer();

                    foreach (var cookieKey in context.Request.Cookies.AllKeys)
                    {
                        var cookie = context.Request.Cookies[cookieKey];

                        var newCookie = new Cookie(cookie.Name, cookie.Value)
                        {
                            Domain = cookie.Domain ?? context.Request.Url.Host,
                            Secure = cookie.Secure,
                            Path = cookie.Path
                        };

                        client.CookieContainer.Add(newCookie);
                    }

                    client.Method = WebSSOConfiguration.ClientMethod;
                    client.Accept = "text/html, application/xhtml+xml, */*";
                    client.Timeout = WebSSOConfiguration.ClientTimeout;
                    client.ContentType = "text/html";

                    // формируем userAgent для обращения к локальному PA-хендлеру
                    client.UserAgent = string.Format(WebSSOConfiguration.WebSSOUserAgentFormat, Environment.MachineName);

                    using (var response = (HttpWebResponse)client.GetResponse())
                    {
                        result.Login = RemoveEndingCS(response.Headers[WebSSOConfiguration.WebSSOLoginKey]);
                        result.WebSSOAuthenticationKey = RemoveEndingCS(response.Headers[WebSSOConfiguration.WebSSOAuthenticatedKey]);
                        result.DisplayName = RemoveEndingCS(response.Headers[WebSSOConfiguration.WebSSODisplayNameKey]);

                        cookies = response.Cookies;

                        if (saveInSession)
                        {
                            session[SessionKey] = result;
                            session[SessionCookie] = cookies;
                        }
                    }

                    if (session != null)
                    {
                        Logger.Value.Info(() => requestId + "WebSSO получен из хендлера: " +
                            session.SessionID + "данные: " +
                            ObjectDumper.DumpObject(result) + " " +
                            originalUrl + " " + url);
                    }

                    // добавим заголовок, показывающий обрабатывается ли текущее приложение РА.
                    context.Response.AddHeader(HandlerHeaderKey, (!string.IsNullOrEmpty(result.Login)).ToString());
                }
                else
                {
                    Logger.Value.Info(() => requestId +  " WebSSO получен из сессии: " + session.SessionID + "данные: " + ObjectDumper.DumpObject(result));
                }

                if (cookies != null)
                {
                    foreach (var cookie in cookies.Cast<Cookie>())
                    {
                        context.Response.Cookies.Add(new HttpCookie(cookie.Name, cookie.Value)
                        {
                            Domain = cookie.Domain ?? string.Empty,
                            Secure = cookie.Secure,
                            Path = cookie.Path,
                            Expires = cookie.Expires
                        });
                    }
                }
                else
                {
                    Logger.Value.ErrorException(requestId + " пустые cookie", new Exception(ObjectDumper.DumpObject(result)));
                }

                // добавим заголовок, показывающий обрабатывается ли текущее приложение РА.
                context.Response.AddHeader(EmbeddedHeaderKey, (!string.IsNullOrEmpty(context
                    .Request
                    .ServerVariables[WebSSOConfiguration.WebSSOAuthenticatedKey ?? string.Empty]))
                    .ToString());

                return result;
            }
            catch (Exception ex)
            {
                Logger.Value.ErrorException(
                        requestId + string.Format(" Ошибка во время получения информации от хендлера РА {0} {1}",
                        url,
                        originalUrl),
                    ex);

                return new WebSSOData { IsFailed = true };
            }
        }

        public string RemoveEndingCS(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            if (str.EndsWith(";"))
            {
                str = str.Substring(0, str.Length - 1);
            }

            return str;
        }
    }
}
