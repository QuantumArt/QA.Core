// Owners: Karlov Nikolay, Abretov Alexey

namespace QA.Core.Web
{
    /// <summary>
    /// Информация о пользователе
    /// </summary>
    public class WebSSOData
    {
        /// <summary>
        /// MSISDN
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Тип профиля
        /// </summary>
        public string WebSSOAuthenticationKey { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Признак того, что данные не получены или получены некорректно
        /// </summary>
        public bool IsFailed { get; set; }
    }
}
