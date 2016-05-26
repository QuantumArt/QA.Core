using System;
using Quantumart.QPublishing.Info;

namespace QA.Core.Data.QP
{
    /// <summary>
    /// Контракт элемента контента
    /// </summary>
    public interface IQpContentItem
    {
        /// <summary>
        /// Создание записи
        /// </summary>
        /// <param name="contentId">Ид. контента</param>
        /// <param name="cnn">Подключение к БД</param>
        /// <returns></returns>
        ContentItem New(int contentId, IQpDbConnector cnn);

        /// <summary>
        /// Чтение записи
        /// </summary>
        /// <param name="id">Ид. записи</param>
        /// <param name="cnn">Подключение к БД</param>
        /// <returns></returns>
        ContentItem Read(int id, IQpDbConnector cnn);

        /// <summary>
        /// Удаление записи
        /// </summary>
        /// <param name="id">Ид. записи</param>
        /// <param name="cnn">Подключение к БД</param>
        void Remove(int id, IQpDbConnector cnn);
    }
}
