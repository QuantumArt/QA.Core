using System.Configuration;
using System.Web;

namespace QA.Core.Web
{
    /// <summary>
    /// Предоставляет статические методы для авторизации
    /// </summary>
    public class AdministrationAuthorizationHelper
    {
        /// <summary>
        ///  Проверяет, авторизован ли пользователь
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static bool IsAuthorize()
        {
            var service = ObjectFactoryBase.Resolve<IAdministrationSecurityChecker>();
            return service.CheckAuthorization(HttpContext.Current.Request.RequestContext.HttpContext);
        }

        #region Config

        /// <summary>
        /// Название ключа AppSetting для активизации проверки
        /// </summary>
        protected const string QPSecurityCheckerIsEnabledKey = "QPSecurityChecker.IsEnabled";

        /// <summary>
        /// Признак включенной авторизации
        /// </summary>
        public static bool IsEnabled
        {
            get
            {
                bool result = false;

                if (bool.TryParse(ConfigurationManager.AppSettings[QPSecurityCheckerIsEnabledKey] ?? "true", out result))
                {
                    return result;
                }

                return false;
            }
        }


        #endregion
    }
}
