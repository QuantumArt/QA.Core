// Owners: Karlov Nikolay, Abretov Alexey


namespace QA.Core.Web
{
    /// <summary>
    /// Класс ошибки (используется для информирования пользователя через JsonServerResult)
    /// </summary>
    public class JsonError
    {
        /// <summary>
        /// Сообщение
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
