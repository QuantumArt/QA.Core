using System.ComponentModel;

namespace QA.Core.Data.QP
{
    /// <summary>
    /// Статус записи контента
    /// </summary>
    public enum ContentItemStatus
    {
        /// <summary>
        /// Опубликована
        /// </summary>
        [Description("Published")]
        Published,

        /// <summary>
        /// Создана
        /// </summary>
        [Description("Created")]
        Created,
        
        /// <summary>
        /// Согласована
        /// </summary>
        [Description("Approved")]
        Approved,

        /// <summary>
        /// Нет статуса
        /// </summary>
        [Description("None")]
        None
    }
}
