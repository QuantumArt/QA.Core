// Owners: Alexey Abretov, Nikolay Karlov

using System;
using System.Collections.Concurrent;
using System.Runtime.Caching;

namespace QA.Core
{
    /// <summary>
    /// Реализует провайдер кеширования данных
    /// </summary>
    public class CacheProvider : ICacheProvider
    {
        private ObjectCache _cache;
        private object _syncRoot = new object();

        public CacheProvider()
        {
            _cache = new MemoryCache(Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Экземпляр хранилища
        /// </summary>
        private ObjectCache Cache
        {
            get
            {
                return _cache;
            }
        }

        /// <summary>
        /// Получает данные из кеша по ключу
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        public virtual object Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
           
            return Cache[key];
        }

        /// <summary>
        /// Записывает данные в кеш
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="data">Данные</param>
        /// <param name="cacheTime">Время кеширования в секундах</param>
        public virtual void Set(string key, object data, int cacheTime)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromSeconds(cacheTime);
            Cache.Set(new CacheItem(key, data), policy);
        }

        /// <summary>
        /// Записывает данные в кеш
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="data">Данные</param>
        /// <param name="expiration">Время кеширования (sliding expiration)</param>
        public virtual void Set(string key, object data, TimeSpan expiration)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + expiration;
            Cache.Set(new CacheItem(key, data), policy);
        }

        /// <summary>
        /// Проверяет наличие данных в кеше
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        public virtual bool IsSet(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            return Cache.Contains(key);
        }

        public virtual bool TryGetValue(string key, out object result)
        {
            result = Cache[key];
            return result != null;
        }

        /// <summary>
        /// Очищает кеш
        /// </summary>
        /// <param name="key">Ключ</param>
        public virtual void Invalidate(string key)
        {

            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            Cache.Remove(key);

        }

        /// <summary>
        /// Освобождаем ресурсы
        /// </summary>
        public virtual void Dispose()
        {
        }

       
    }
}
