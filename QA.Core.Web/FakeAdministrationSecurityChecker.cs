using System.Web;

namespace QA.Core.Web
{
    /// <summary>
    /// Проверка авторизации для администрирования для тестов
    /// </summary>
    public class FakeAdministrationSecurityChecker : IAdministrationSecurityChecker
    {
        /// <summary>
        /// Проверяет, авторизован ли пользователь
        /// </summary>
        /// <param name="context">текущий контекст</param>
        /// <returns></returns>
        public bool CheckAuthorization(HttpContextBase context)
        {
            return true;
        }
    }
}
