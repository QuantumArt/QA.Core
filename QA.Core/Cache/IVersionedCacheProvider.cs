using System;

namespace QA.Core
{
    /// <summary>
    /// Версионированное кеширование
    /// </summary>
    public interface IVersionedCacheProvider : ICacheProvider, IDisposable
    {
        /// <summary>
        /// Добавление данных в кэш
        /// </summary>
        /// <param name="data">данные</param>
        /// <param name="key">тэг, в общем случае представляет имя класса сервиса + имя метода + список параметров</param>
        /// <param name="tags">список зависимых контентов</param>
        /// <param name="expiration">время жизни в кэше</param>
        void Add(object data, string key, string[] tags, TimeSpan expiration);

        /// <summary>
        /// Получение данных их кэша
        /// </summary>
        /// <param name="key">тэг, в общем случае представляет имя класса сервиса + имя метода + список параметров</param>
        /// <param name="tags">список зависимых контентов</param>
        /// <returns>закэшированне данные, если они присутствуют в кэше</returns>
        object Get(string key, string[] tags);

        /// <summary>
        /// Сообщает, что данные этого тега (контента) обновились
        /// </summary>
        /// <param name="mode">где инвалидировать кэш</param>
        /// <param name="tag">тег</param>
        void InvalidateByTag(InvalidationMode mode, string tag);

        /// <summary>
        /// Сообщает, что данные этого тега (контента) обновились
        /// </summary>
        /// <param name="mode">где инвалидировать кэш</param>
        /// <param name="tags">тег</param>
       void InvalidateByTags(InvalidationMode mode, params string[] tags);

        /// <summary>
        /// Удаляет из кеша элемент, у которого данный ключ и набор зависимых тегов.
        /// </summary>
        /// <param name="key">тэг, в общем случае представляет имя класса сервиса + имя метода + список параметров</param>
        /// <param name="tags">список зависимых контентов</param>
        void Invalidate(string key, string[] tags);

        
    }
}
