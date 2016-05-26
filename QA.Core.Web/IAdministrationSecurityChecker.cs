using System.Web;

namespace QA.Core.Web
{
    /// <summary>
    /// Проверка авторизации для администрирования
    /// </summary>
    public interface IAdministrationSecurityChecker
    {
        /// <summary>
        /// Проверяет, авторизован ли пользователь
        /// </summary>
        /// <param name="context">текущий контекст</param>
        /// <returns></returns>
        bool CheckAuthorization(HttpContextBase context);
    }
}
