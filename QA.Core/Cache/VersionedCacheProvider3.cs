// Owners: Alexey Abretov, Nikolay Karlov

using System;
using System.Collections.Concurrent;
using System.Runtime.Caching;

namespace QA.Core
{
    /// <summary>
    /// Реализует провайдер кеширования данных
    /// </summary>
    public class VersionedCacheProvider3 : CacheProvider, IVersionedCacheProvider
    {
        private readonly MemoryCache _cache;
        private static readonly int DeprecatedCachePeriod = 120;
        private static readonly string DeprecatedCacheKey = "<>__deprecated__cache_element_";

        public VersionedCacheProvider3()
        {
            _cache = MemoryCache.Default;
        }

        ///// <summary>
        ///// Для совместимости со старым кодом
        ///// </summary>
        ///// <param name="innerCache"></param>
        //public VersionedCacheProvider3(ICacheProvider innerCache)
        //{
        //    if (innerCache == null)
        //    {
        //        _cache = new MemoryCache("VersionedCacheProvider2" + Guid.NewGuid().ToString());
        //    }
        //    else if (innerCache is CacheProvider)
        //    {
        //        _cache = ((CacheProvider)innerCache).Cache;
        //    }
        //    else
        //    {
        //        throw new InvalidOperationException("VersionedCacheProvider2 supports only CacheProvider as an internal cache, but provided: " + innerCache.GetType());
        //    }
        //}

        /// <summary>
        /// Получает данные из кеша по ключу
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        public override object Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            return _cache[key];
        }

        /// <summary>
        /// Записывает данные в кеш
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="data">Данные</param>
        /// <param name="cacheTime">Время кеширования в секундах</param>
        public override void Set(string key, object data, int cacheTime)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromSeconds(cacheTime);
            policy.RemovedCallback = CacheItemRemovedCallBack;
            _cache.Set(new CacheItem(key, data), policy);
        }

        /// <summary>
        /// Записывает данные в кеш
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="data">Данные</param>
        /// <param name="expiration">Время кеширования (sliding expiration)</param>
        public override void Set(string key, object data, TimeSpan expiration)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + expiration;
            policy.RemovedCallback = CacheItemRemovedCallBack;
            _cache.Set(new CacheItem(key, data), policy);
        }

        /// <summary>
        /// Проверяет наличие данных в кеше
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        public override bool IsSet(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }

            return _cache.Contains(key);
        }

        public override bool TryGetValue(string key, out object result)
        {
            result = _cache[key];
            return result != null;
        }

        /// <summary>
        /// Очищает кеш
        /// </summary>
        /// <param name="key">Ключ</param>
        public override void Invalidate(string key)
        {

            if (string.IsNullOrEmpty(key))
            {
                return;
            }
            _cache.Remove(key);

        }

        /// <summary>
        /// Освобождаем ресурсы
        /// </summary>
        public override void Dispose()
        {
        }



        #region IVersionedCacheProvider Members

        public virtual void Add(object data, string key, string[] tags, TimeSpan expiration)
        {
            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.Now + expiration
            };

            policy.RemovedCallback = CacheItemRemovedCallBack;

            ConfigureDependency(tags, policy);

            _cache.Set(key, data, policy);
        }

        private void ConfigureDependency(string[] tags, CacheItemPolicy policy)
        {
            if (tags != null && tags.Length > 0)
            {
                var now = DateTime.Now;
                var tagExpiration = now.AddDays(10);

                foreach (var item in tags)
                {
                    AddTag(_cache, now, tagExpiration, item);
                }

                policy.ChangeMonitors.Add(
                   _cache.CreateCacheEntryChangeMonitor(tags)
                );
            }
        }



        public virtual object Get(string key, string[] tags)
        {
            return this.Get(key);
        }

        public virtual void InvalidateByTag(InvalidationMode mode, string tag)
        {
            Throws.IfArgumentNullOrEmpty(tag, _ => tag);
            _cache.Remove(tag);
        }

        public virtual void InvalidateByTags(InvalidationMode mode, params string[] tags)
        {
            foreach (var tag in tags)
            {
                _cache.Remove(tag);
            }
        }

        public virtual void Invalidate(string key, string[] tags)
        {
            this.Invalidate(key);
        }

        #endregion

        /// <summary>
        /// Автовосстановление кеш-тега
        /// </summary>
        /// <param name="arguments"></param>
        private static void CacheTagRemovedCallBack(CacheEntryRemovedArguments arguments)
        {
            var now = DateTime.Now;
            var tagExpiration = now.AddDays(1);

            if (arguments.CacheItem != null)
            {
                AddTag(arguments.Source, now, tagExpiration, arguments.CacheItem.Key);
            }
        }

        /// <summary>
        /// Автовосстановление устаревшего объекта. 
        /// Объект восстанавливается на короткое время для использования в других потоках, пока в одном из потоке происходит обновление его значения. 
        /// </summary>
        /// <param name="arguments"></param>
        private static void CacheItemRemovedCallBack(CacheEntryRemovedArguments args)
        {
            if (args.CacheItem != null)
            {
                if (args.RemovedReason == CacheEntryRemovedReason.ChangeMonitorChanged
                    || args.RemovedReason == CacheEntryRemovedReason.Expired)
                {
                    var value = args.CacheItem.Value;
                    if (value != null)
                    {
                        var key = args.CacheItem.Key;
                        args.Source.Set(new CacheItem(CalculateDeprecatedKey(key)) { Value = value },
                            new CacheItemPolicy { AbsoluteExpiration = DateTime.Now.AddSeconds(DeprecatedCachePeriod) });
                    }
                }
            }
        }

        internal static string CalculateDeprecatedKey(string key)
        {
            return DeprecatedCacheKey + key;
        }

        private static void AddTag(ObjectCache cache, DateTime now, DateTime tagExpiration, string item)
        {
            cache.AddOrGetExisting(item, now, new CacheItemPolicy
            {
                Priority = CacheItemPriority.NotRemovable,
                AbsoluteExpiration = tagExpiration,
                RemovedCallback = CacheTagRemovedCallBack
            });
        }
    }
}
