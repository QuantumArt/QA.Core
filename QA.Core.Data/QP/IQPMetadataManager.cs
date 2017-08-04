using System.Collections.Generic;
using Quantumart.QPublishing.Info;
using Quantumart.QPublishing.Database;

namespace QA.Core.Data.QP
{
    /// <summary>
    /// Контракт менеджера метаданных QP
    /// </summary>
    public interface IQPMetadataManager
    {
        /// <summary>
        /// Возвращает список атрибутов контента
        /// </summary>
        /// <param name="siteName">Имя сайта</param>
        /// <param name="contentName">Имя контента</param>
        /// <returns></returns>
        IEnumerable<ContentAttribute> GetContentAttributes(string siteName, string contentName);

        /// <summary>
        /// Возвращает список атрибутов контента
        /// </summary>
        /// <param name="contentId">Идентификатор контента</param>
        /// <returns></returns>
        IEnumerable<ContentAttribute> GetContentAttributes(int contentId);

        /// <summary>
        /// Возвращает атрибут контента
        /// </summary>
        /// <param name="siteName">Имя сайта</param>
        /// <param name="contentName">Имя контента</param>
        /// <param name="fieldName">Имя поля</param>
        /// <returns></returns>
        ContentAttribute GetContentAttribute(string siteName, string contentName, string fieldName);

        /// <summary>
        /// Возвращает идентификатор контента
        /// </summary>
        /// <param name="siteName">Имя сайта</param>
        /// <param name="contentName">Имя контента</param>
        /// <returns></returns>
        int GetContentId(string siteName, string contentName);

        /// <summary>
        /// Возвращает имя контента
        /// </summary>
        /// <param name="contentId">Идентификатор контента</param>
        /// <returns></returns>
        string GetContentName(int contentId);

        /// <summary>
        /// Возвращает идентификатор сайта
        /// </summary>
        /// <param name="siteName">Название сайта</param>
        /// <returns></returns>
        int GetSiteId(string siteName);

        /// <summary>
        /// Подключение к QP
        /// </summary>
        DBConnector DbConnection { get; }

        /// <summary>
        /// Строка подключения
        /// </summary>
        string ConnectionString { get; }
    }
}