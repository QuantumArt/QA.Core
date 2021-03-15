#if !NETSTANDARD
using System;
using System.Data.Linq.Mapping;

namespace QA.Core.Data
{
    /// <summary>
    /// Поставщик маппинга сущностей Linq2Sql на БД
    /// </summary>
    public interface IXmlMappingResolver
    {
        /// <summary>
        /// Получить текущий маппинг
        /// </summary>
        /// <returns></returns>
        XmlMappingSource GetCurrentMapping();

        /// <summary>
        /// Получить маппинг с учетом режима доступа к данным live/stage
        /// </summary>
        /// <param name="isStage"></param>
        /// <returns></returns>
        XmlMappingSource GetMapping(bool isStage);

        /// <summary>
        /// Возвращает имя SQL-таблицы по типам контекста и класса контента
        /// </summary>
        /// <param name="contentType">Тип контента</param>
        /// <param name="contextType">Тип контекста</param>
        /// <returns>имя таблицы</returns>
        string GetTableName(Type contextType, Type contentType);

        /// <summary>
        /// Возвращает имя SQL-таблицы по типам контекста и класса контента
        /// </summary>
        /// <typeparam name="TContext">Тип контекста</typeparam>
        /// <typeparam name="TContent">Тип контента</typeparam>
        /// <returns>имя таблицы</returns>
        string GetTableName<TContext, TContent>();
    }
}
#endif
