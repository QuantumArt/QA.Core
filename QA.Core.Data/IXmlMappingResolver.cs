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
    }
}
