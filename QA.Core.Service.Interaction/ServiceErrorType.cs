// Owners: Alexey Abretov, Nikolay Karlov

namespace QA.Core.Service.Interaction
{
    /// <summary>
    /// Тип ошибки сервиса
    /// </summary>
    public enum ServiceErrorType
    {
        /// <summary>
        /// Исключение
        /// </summary>
        Exception = 1,

        /// <summary>
        /// Сообщение бизнес-логики
        /// </summary>
        BusinessLogicMessage = 2
    }
}
