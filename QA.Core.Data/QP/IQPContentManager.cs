using System.Collections.Generic;
using Quantumart.QPublishing.Info;

namespace QA.Core.Data.QP
{
    /// <summary>
    /// Контракт для работы с БД QP
    /// </summary>
    public interface IQPContentManager
    {
        #region Properties

        /// <summary>
        /// Подключение к QP
        /// </summary>
        IQpDbConnector DbConnection { get; }

        /// <summary>
        /// Запрос
        /// </summary>
        List<string> Includes { get; }

        /// <summary>
        /// Список связанных контентов
        /// </summary>
        ContentDataQueryObject Query { get; }

        #endregion

        #region Methods

        #region Prepare properties

        /// <summary>
        /// Устанавливает интервал кэширования
        /// </summary>
        /// <param name="cacheInterval"></param>
        /// <returns></returns>
        QPContentManager CacheInterval(double cacheInterval);

        /// <summary>
        /// Устанавливает подключение к QP
        /// </summary>
        /// <param name="connectionString">Строка подключения</param>
        /// <returns></returns>
        QPContentManager Connection(string connectionString);

        /// <summary>
        /// Устанавливает название контента
        /// </summary>
        /// <param name="contentName">Имя контента</param>
        /// <returns></returns>
        QPContentManager ContentName(string contentName);

        /// <summary>
        /// Устанавливает список полей
        /// </summary>
        /// <param name="fields">Поля через запятую</param>
        /// <returns></returns>
        QPContentManager Fields(string fields);

        /// <summary>
        /// Устанавливает список дополнительных контентов
        /// </summary>
        /// <param name="path">Имя поля</param>
        /// <returns></returns>
        QPContentManager Include(string path);

        /// <summary>
        /// Устанавливает признак кэширования
        /// </summary>
        /// <param name="isCacheResult">Признак кэширования</param>
        /// <returns></returns>
        QPContentManager IsCacheResult(bool isCacheResult);

        /// <summary>
        /// Устанавливает признак получения архивных записей
        /// </summary>
        /// <param name="isIncludeArchive"></param>
        /// <returns></returns>
        QPContentManager IsIncludeArchive(bool isIncludeArchive);

        /// <summary>
        /// Устанаваливает признак сброса кэша
        /// </summary>
        /// <param name="isResetCache"></param>
        /// <returns></returns>
        QPContentManager IsResetCache(bool isResetCache);

        /// <summary>
        /// Устанаваливает признак показа расщепленной версии записи
        /// </summary>
        /// <param name="isShowSplittedArticle"></param>
        /// <returns></returns>
        QPContentManager IsShowSplittedArticle(bool isShowSplittedArticle);

        /// <summary>
        /// Устанавливает использование клиентской выборки
        /// </summary>
        /// <param name="isUseClientSelection"></param>
        /// <returns></returns>
        QPContentManager IsUseClientSelection(bool isUseClientSelection);

        /// <summary>
        /// Устанаваливает использование расписания
        /// </summary>
        /// <param name="isUseSchedule"></param>
        /// <returns></returns>
        QPContentManager IsUseSchedule(bool isUseSchedule);

        /// <summary>
        /// Устанавливает сортировку
        /// </summary>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        QPContentManager OrderBy(string orderBy);

        /// <summary>
        /// Устанавливает размер страницы для выборки
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        QPContentManager PageSize(long pageSize);

        /// <summary>
        /// Устанавливает имя сайта
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        QPContentManager SiteName(string siteName);

        /// <summary>
        /// Устанавливает начальный индекс страницы
        /// </summary>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        QPContentManager StartIndex(long startIndex);

        /// <summary>
        /// Устанавливает статус записей для выборки
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        QPContentManager StatusName(ContentItemStatus status);

        /// <summary>
        /// Устанавливает фильтр выборки
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        QPContentManager Where(string where);

        #endregion

        #endregion

        /// <summary>
        /// Возвращает результат запроса
        /// </summary>
        /// <returns></returns>
        ContentResult Get();
    }
}
