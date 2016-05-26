using System.Data;
using System.Data.SqlClient;
using Quantumart.QPublishing;
using Quantumart.QPublishing.Database;

namespace QA.Core.Data.QP
{
    /// <summary>
    /// Контракт подключения к БД Qp
    /// </summary>
    public interface IQpDbConnector
    {
        DBConnector DbConnector { get; }

        /// <summary>
        /// Реальный коннектор
        /// </summary>
        object Connector { get; }

        /// <summary>
        /// Возвращает данные контента
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="totalRecords">Общее количество строк</param>
        /// <returns></returns>
        DataTable GetContentData(ContentDataQueryObject query, ref long totalRecords);

        /// <summary>
        /// Возвращает данные контента
        /// </summary>
        /// <param name="command">Команда к БД</param>
        /// <returns></returns>
        DataTable GetRealData(SqlCommand command);

        /// <summary>
        /// Строка подключения
        /// </summary>
        string InstanceConnectionString { get; }

        /// <summary>
        /// Вовзращает имя контента
        /// </summary>
        /// <param name="contentId">Идентификатор контента</param>
        /// <returns></returns>
        string GetContentName(int contentId);

        /// <summary>
        /// Возвращает идентификаторы связанных записей
        /// </summary>
        /// <param name="fieldName">Имя поля</param>
        /// <param name="itemId">Ид. записи, для которой необходимо получить данные</param>
        /// <returns></returns>
        string GetContentItemLinkIDs(string fieldName, int itemId);

        /// <summary>
        /// Удаляет элемент из БД
        /// </summary>
        /// <param name="contentItemId"></param>
        void DeleteContentItem(int contentItemId);

        /// <summary>
        /// Возвращает идентификаторы связанных записей
        /// </summary>
        /// <param name="fieldName">Имя поля</param>
        /// <param name="values">Ид'ы записей, для которой необходимо получить данные</param>
        /// <returns></returns>
        string GetContentItemLinkIDs(string fieldName, string values);
    }
}
