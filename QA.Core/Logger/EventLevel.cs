
namespace QA.Core.Logger
{
    /// <summary>
    /// Уровень критичности сообщения
    /// </summary>
    public enum EventLevel
    {
        /// <summary>
        /// Уровень трассировки
        /// </summary>
        Trace = 0,

        /// <summary>
        /// Уровень отладки
        /// </summary>
        Debug = 1,

        /// <summary>
        /// Уровень информации
        /// </summary>
        Info = 2, 

        /// <summary>
        /// Уровень предупреждения
        /// </summary>
        Warning = 3, 

        /// <summary>
        /// Уровень ошибки
        /// </summary>
        Error = 4, 

        /// <summary>
        /// Уровень фатальной ошибки
        /// </summary>
        Fatal = 5,

    }
}
