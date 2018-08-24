using Quantumart.QPublishing.Info;
using Quantumart.QPublishing.Database;

namespace QA.Core.Data.QP
{
    /// <summary>
    /// Реализация элемента контента
    /// </summary>
    public class QpContentItem : IQpContentItem
    {
        /// <summary>
        /// Создание записи
        /// </summary>
        /// <param name="contentId">Ид. контента</param>
        /// <param name="cnn">Подключение к БД</param>
        /// <returns></returns>
        public ContentItem New(int contentId, IQpDbConnector cnn)
        {
            return ContentItem.New(contentId, cnn.Connector as DBConnector);
        }

        /// <summary>
        /// Чтение записи
        /// </summary>
        /// <param name="id">Ид. записи</param>
        /// <param name="cnn">Подключение к БД</param>
        /// <returns></returns>
        public ContentItem Read(int id, IQpDbConnector cnn)
        {
            return ContentItem.Read(id, cnn.Connector as DBConnector);
        }

        /// <summary>
        /// Удаление записи
        /// </summary>
        /// <param name="id">Ид. записи</param>
        /// <param name="cnn">Подключение к БД</param>
        public void Remove(int id, IQpDbConnector cnn)
        {
            ContentItem.Remove(id, cnn.Connector as DBConnector);
        }
    }
}
