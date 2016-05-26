using System;
using System.Configuration;
using System.Data;
using System.ServiceModel;
using System.Web;
using QA.Core.Service.Interaction;
using Quantumart.QPublishing.Database;
using Quantumart.QPublishing.OnScreen;

namespace QA.Core.Web.Qp
{
    /// <summary>
    /// Проверка авторизации для администрирования
    /// </summary>
    public class QPSecurityChecker : IAdministrationSecurityChecker
    {
        protected static readonly string AuthenticationKey = "QP_Beeline_AuthenticationKey";
        protected static readonly string UserLanguageFieldName = "LANGUAGE_ID";
        public static readonly string UserLanguageKey = "QP_User_Language";

        #region IAdministrationSecurityChecker Members

        /// <summary>
        /// Проверяет, авторизован ли пользователь
        /// </summary>
        /// <param name="context">текущий контекст</param>
        /// <returns></returns>
        public virtual bool CheckAuthorization(HttpContextBase context)
        {
            if (AdministrationAuthorizationHelper.IsEnabled)
            {
                if (context == null)
                {
                    return false;
                }

                //TODO: Wcf?
                //TODO: store in session by hostid
                if (context.Session[AuthenticationKey] != null)
                {
                    context.Items[DBConnector.LastModifiedByKey] = context.Session[DBConnector.LastModifiedByKey];

                    return true;
                }

                if (CurrentServiceToken == null)
                {
                    return false;
                }

                var dBConnector = new DBConnector(
                    ConfigurationManager.ConnectionStrings[
                        CurrentServiceToken.ConnectionName].ConnectionString);
                int userId = QScreen.AuthenticateForCustomTab(dBConnector);

                bool result = userId > 0; // CheckCustomTabAuthentication(dBConnector);

                if (result)
                {
                    try
                    {
                        var userInfo = GetUserInfo(userId, dBConnector);

                        if (userInfo != null && userInfo.Rows.Count > 0)
                        {
                            var lang = userInfo.Rows[0][UserLanguageFieldName].ToString();
                            int langId = 0;
                            int.TryParse(lang, out langId);

                            string langName = ((QpLanguage)Enum.Parse(typeof(QpLanguage), langId.ToString())).GetDescription();

                            context.Session[UserLanguageKey] = langName;
                        }
                    }
                    catch (Exception ex)
                    {
                        ObjectFactoryBase.Logger.ErrorException(ex.Message, ex);
                        context.Session[UserLanguageKey] = QpLanguage.Default.GetDescription();
                    }

                    context.Session[AuthenticationKey] = result;
                    context.Session[DBConnector.LastModifiedByKey] = userId;
                    context.Items[DBConnector.LastModifiedByKey] = userId;
                }

                return result;
            }

            return true;
        }

        private static DataTable GetUserInfo(int user_id, DBConnector dBConnector)
        {
            string str = string.Concat("select * from users where user_id = ", user_id.ToString());
            DataTable cachedData = dBConnector.GetCachedData(str);
            dBConnector = null;
            return cachedData;
        }

        #endregion

        #region Authenticate

        protected virtual ServiceToken CurrentServiceToken
        {
            get
            {
                ServiceToken token = null;

                //TODO: Xml WebService?
                if (OperationContext.Current != null)
                {
                    token = OperationContext.Current.IncomingMessageHeaders.GetHeader<ServiceToken>(
                        ServiceToken.ServiceTokenKey, ServiceToken.ServiceTokenNs);
                }

                if (token == null && HttpContext.Current != null)
                {
                    token = HttpContext.Current.Items[ServiceToken.ServiceTokenKey] as ServiceToken;
                }

                if (token == null && ObjectFactoryBase.CacheProvider != null)
                {
                    token = ObjectFactoryBase.CacheProvider.Get(ServiceToken.ServiceTokenKey) as ServiceToken;
                }

                return token;
            }
        }

        #endregion
    }
}
