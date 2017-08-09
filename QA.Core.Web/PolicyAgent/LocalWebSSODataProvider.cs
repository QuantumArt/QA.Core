// Owners: Karlov Nikolay, Abretov Alexey

using System;
using System.Web;
using QA.Core.Logger;

namespace QA.Core.Web
{
    /// <summary>
    /// Потавщик данных от полиси-агента
    /// <remarks>Получает данные непосредственно из ServerVariables запроса</remarks>
    /// </summary>
    public class LocalWebSSODataProvider : IWebSSODataProvider
    {
        const string EmbeddedHeaderKey = "PA-Embedded-State";
        const string HandlerHeaderKey = "PA-Handler-State";

        public Lazy<ILogger> Logger = new Lazy<ILogger>(() => ObjectFactoryBase.Resolve<ILogger>("Client"));

        /// <summary>
        /// Получение данных пользователя-абонента МТС
        /// </summary>
        /// <remarks>Передает cookie</remarks>
        /// <param name="context">Контекст запроса</param>
        /// <exception cref="System.Exception">ВСе типы исключений при работе с HttpWebRequest</exception>
        /// <returns></returns>
        public WebSSOData GetUserData(HttpContextBase context, bool saveInSession)
        {
            try
            {
                WebSSOData result = new WebSSOData { };

                result.Login = RemoveEndingCS(context.Request.ServerVariables[WebSSOConfiguration.WebSSOLoginKey]);
                result.WebSSOAuthenticationKey = RemoveEndingCS(context.Request.ServerVariables[WebSSOConfiguration.WebSSOAuthenticatedKey]);
                result.DisplayName = RemoveEndingCS(context.Request.ServerVariables[WebSSOConfiguration.WebSSODisplayNameKey]);

                // добавим заголовок, показывающий используется ли хендлер
                context.Response.AddHeader(HandlerHeaderKey, false.ToString());

                // добавим заголовок, показывающий обрабатывается ли текущее приложение РА.
                context.Response.AddHeader(EmbeddedHeaderKey, (!string.IsNullOrEmpty(result.Login)).ToString());

                return result;
            }
            catch (Exception ex)
            {
                Logger.Value.ErrorException("Ошибка во время получения информации от РА", ex);

                return new WebSSOData { IsFailed = true };
            }
        }

        /// <summary>
        /// Удаление ;
        /// </summary>
        private string RemoveEndingCS(string str)
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
