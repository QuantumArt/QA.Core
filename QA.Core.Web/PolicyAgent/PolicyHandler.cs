// Owners: Karlov Nikolay, Abretov Alexey

using System;
using System.Configuration;
using System.Linq;
using System.Web;
#pragma warning disable 1591

namespace QA.Core.Web
{
    public class PolicyHandler : IHttpHandler
    {
        public static bool IsServerNameHeaderEnabled
        {
            get
            {
                string val = ConfigurationManager.AppSettings["ServerHeaders.IsEnabled"];

                if (string.IsNullOrEmpty(val) || string.IsNullOrWhiteSpace(val))
                {
                    return false;
                }

                bool result;
                if (Boolean.TryParse(val, out result))
                {
                    return result;
                }

                return false;
            }
        }

        /// <summary>
        /// Ключ header'a с именем текущего сервера
        /// </summary>
        public static string ServerHeadersKey
        {
            get
            {
                string val = ConfigurationManager.AppSettings["ServerHeaders.Key"];

                return val ?? "x-webnet";
            }
        }

        /// <summary>
        /// Хэндлер для получения информации от полиси
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            // Просто добавляем хедеры полиси
            var webSSOAuthenticatedKey = WebSSOConfiguration.WebSSOAuthenticatedKey;
            var webSSODisplayNameKey = WebSSOConfiguration.WebSSODisplayNameKey;
            var webSSOLoginKey = WebSSOConfiguration.WebSSOLoginKey;

            try
            {
                if (!string.IsNullOrEmpty(webSSOAuthenticatedKey) && !string.IsNullOrEmpty(webSSODisplayNameKey) && !string.IsNullOrEmpty(webSSOLoginKey))
                {
                    if (!WebSSOConfiguration.UseDebugValues)
                    {
                        context.Response.AddHeader(webSSOAuthenticatedKey, context.Request.ServerVariables[webSSOAuthenticatedKey] ?? string.Empty);
                        context.Response.AddHeader(webSSODisplayNameKey, context.Request.ServerVariables[webSSODisplayNameKey] ?? string.Empty);
                        context.Response.AddHeader(webSSOLoginKey, context.Request.ServerVariables[webSSOLoginKey] ?? string.Empty);

                        context.Response.Write("Succeess. Get information from headers.");
                    }
                    else
                    {
                        context.Response.AddHeader(webSSOAuthenticatedKey, WebSSOConfiguration.WebSSOKey ?? string.Empty);
                        context.Response.AddHeader(webSSODisplayNameKey, WebSSOConfiguration.MSISDN ?? string.Empty);
                        context.Response.AddHeader(webSSOLoginKey, WebSSOConfiguration.MSISDN ?? string.Empty);

                        context.Response.Write("Succeess. Get information from headers. Debug mode.");
                    }
                }
                else
                {
                    context.Response.Write("Fault. Check configuration");
                }
            }
            catch (Exception ex)
            {
                context.Response.Write(string.Join("<br />", ex.Flat().Select(m => m.Message)));
            }

            if (IsServerNameHeaderEnabled)
            {
                try
                {
                    if (!context.Response.Headers.AllKeys.Contains(ServerHeadersKey))
                    {
                        context.Response.AddHeader(ServerHeadersKey, Environment.MachineName);
                    }
                }
                catch (Exception ex)
                {
                    context.Response.Write(string.Join("<br />", ex.Flat().Select(m => m.Message)));
                }
            }
        }

        #endregion
    }
}
