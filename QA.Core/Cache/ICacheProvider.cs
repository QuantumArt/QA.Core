// Owners: Alexey Abretov, Nikolay Karlov, Garik Kuprianov

using System;

namespace QA.Core
{
    /// <summary>
    /// Описывает провайдер кеширования данных
    /// </summary>
    public interface ICacheProvider : IDisposable
    {
        /// <summary>
        /// Получает данные из кеша по ключу
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        object Get(string key);

        /// <summary>
        /// Записывает данные в кеш
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="data">Данные</param>
        /// <param name="cacheTime">Время кеширования в секундах</param>
        void Set(string key, object data, int cacheTime);

        /// <summary>
        /// Записывает данные в кеш
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="data">Данные</param>
        /// <param name="expiration">Время кеширования</param>
        void Set(string key, object data, TimeSpan expiration);

        /// <summary>
        /// Проверяет существование объекта и в случае существования возвращает данные.
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="result">дангные</param>
        /// <returns></returns>
        bool TryGetValue(string key, out object result);
        /// <summary>
        /// Проверяет наличие данных в кеше
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        bool IsSet(string key);

        /// <summary>
        /// Очищает кеш
        /// </summary>
        /// <param name="key">Ключ</param>
        void Invalidate(string key);
               
    }
}
