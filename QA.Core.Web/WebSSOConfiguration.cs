// Owners: Alexey Abretov, Nikolay Karlov

using System.Configuration;
#pragma warning disable 1591

namespace QA.Core.Web
{
    /// <summary>
    /// Набор параметров голосовой почты
    /// </summary>
    public class WebSSOConfiguration
    {
        #region WebSSO

        /// <summary>
        /// Ссылка на вход
        /// </summary>
        public static string WebSSOLoginUrl
        {
            get
            {
                string val = ConfigurationManager.AppSettings["WebSSO.LoginUrl"];
                return val ?? string.Empty;
            }
        }

        /// <summary>
        /// Ссылка на выход
        /// </summary>
        public static string WebSSOLogoutUrl
        {
            get
            {
                string val = ConfigurationManager.AppSettings["WebSSO.LogoutUrl"];
                return val ?? string.Empty;
            }
        }

        /// <summary>
        /// Название серверной переменной-признака аутентификации в WebSSO
        /// </summary>
        public static string WebSSOAuthenticatedKey
        {
            get
            {
                string val = ConfigurationManager.AppSettings["WebSSO.AuthenticatedKey"];
                return val ?? string.Empty;
            }
        }

        /// <summary>
        /// Название серверной переменной WebSSO для логина (10 символов телефона)
        /// </summary>
        public static string WebSSOLoginKey
        {
            get
            {
                string val = ConfigurationManager.AppSettings["WebSSO.LoginKey"];
                return val ?? string.Empty;
            }
        }

        /// <summary>
        /// Название серверной переменной WebSSO для логина (10 символов телефона)
        /// </summary>
        public static string WebSSOUserAgentFormat
        {
            get
            {
                string val = ConfigurationManager.AppSettings["WebSSO.UserAgentFormat"];
                return val ?? "{0}";
            }
        }

        /// <summary>
        /// Название серверной переменной WebSSO для отображаемого имени
        /// </summary>
        public static string WebSSODisplayNameKey
        {
            get
            {
                string val = ConfigurationManager.AppSettings["WebSSO.DisplayNameKey"];
                return val ?? string.Empty;
            }
        }

        #endregion

        #region WebSSO ClientConfiguration

        public static string ClientMethod {
            get
            {
                string val = ConfigurationManager.AppSettings["WebSSO.ClientMethod"];
                return val ?? "GET";
            }
        }

        public static string AddressToHandler
        {
            get
            {
                string val = ConfigurationManager.AppSettings["WebSSO.AddressToHandler"];
                return val ?? "~/policy/policyhandler.ashx";
            }
        }

        public static string MSISDN
        {
            get
            {
                string val = ConfigurationManager.AppSettings["WebSSOHandler.MSISDN"];
                return val ?? "~/policy/policyhandler.ashx";
            }
        }

        public static string WebSSOKey
        {
            get
            {
                string val = ConfigurationManager.AppSettings["WebSSOHandler.WebSSOKey"];
                return val ?? "~/policy/policyhandler.ashx";
            }
        }

        public static bool UseDebugValues
        {
            get
            {
                bool result = false;

                if (bool.TryParse(ConfigurationManager.AppSettings["WebSSOHandler.UseDebugValues"] ?? string.Empty, out result))
                {
                    return result;
                }

                return result;
            }
        }

        public static int ClientTimeout
        {
            get
            {
                int result = default(int);

                if (int.TryParse(ConfigurationManager.AppSettings["WebSSO.ClientTimeout"] ?? string.Empty, out result))
                {
                    return result;
                }

                return 1000;
            }
        }
        #endregion
    }
}
