using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using QA.Core.Logger;

namespace QA.Core.Cache
{
    /// <summary>
    /// Реализует провайдер кеширования данных
    /// </summary>
    public class VersionedCacheProviderBase : IVersionedCacheProvider2
    {
        private readonly MemoryCache _cache;

        private readonly ILogger _logger;

        private static readonly string DeprecatedCacheKey = "<>__deprecated__cache_element_";

        private readonly ConcurrentDictionary<string, object> _lockers = new ConcurrentDictionary<string, object>();

        private const int TryenterTimeoutMs = 7000;

        public VersionedCacheProviderBase(ILogger logger)
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            _logger = logger;
        }

        /// <summary>
        /// Получает данные из кеша по ключу
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        public virtual object Get(string key) => string.IsNullOrEmpty(key) ? null : _cache.Get(key);

        /// <summary>
        /// Записывает данные в кеш
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="data">Данные</param>
        /// <param name="cacheTime">Время кеширования в секундах</param>
        public virtual void Set(string key, object data, int cacheTime)
        {
            Set(key, data, TimeSpan.FromSeconds(cacheTime));
        }

        /// <summary>
        /// Записывает данные в кеш
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="data">Данные</param>
        /// <param name="expiration">Время кеширования</param>
        public virtual void Set(string key, object data, TimeSpan expiration)
        {
            if (string.IsNullOrEmpty(key))
            {
                return;
            }

            var policy = new MemoryCacheEntryOptions();
            policy.AbsoluteExpiration = DateTimeOffset.Now + expiration;

            _cache.Set(key, data, policy);
        }

        /// <summary>
        /// Проверяет наличие данных в кеше
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        public virtual bool IsSet(string key)
        {
            return Get(key) != null;
        }

        public virtual bool TryGetValue(string key, out object result)
        {
            result = _cache.Get(key);
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
            _cache.Remove(key);

        }

        /// <summary>
        /// Освобождаем ресурсы
        /// </summary>
        public virtual void Dispose()
        {
        }



        #region IVersionedCacheProvider Members

        public void Add(object data, string key, string[] tags, TimeSpan expiration)
        {
            Add(data, key, tags, expiration, false, CacheItemPriority.Normal);
        }

        private void Add(object data, string key, string[] tags, TimeSpan expiration, bool useSlidingExpiration, CacheItemPriority priority)
        {
            var policy = new MemoryCacheEntryOptions {Priority = priority};
            if (useSlidingExpiration)
            {
                policy.SlidingExpiration = expiration;
            }
            else
            {
                policy.AbsoluteExpiration = DateTimeOffset.Now + expiration;
            }


            if (tags != null && tags.Length > 0)
            {
                foreach (var item in tags)
                {
                    var src = AddTag(item);
                    policy.AddExpirationToken(new CancellationChangeToken(src.Token));
                }
            }

            _cache.Set(key, data, policy);
        }



        public object Get(string key, string[] tags)
        {
            return Get(key);
        }

        public void InvalidateByTag(InvalidationMode mode, string tag)
        {
            Throws.IfArgumentNullOrEmpty(tag, _ => tag);
            _cache.Remove(tag);
        }

        public void InvalidateByTags(InvalidationMode mode, params string[] tags)
        {
            foreach (var tag in tags)
            {
                _cache.Remove(tag);
            }
        }

        public void Invalidate(string key, string[] tags)
        {
            Invalidate(key);
        }

        #endregion

        /// <summary>
        /// Автовосстановление кеш-тега
        /// </summary>
        private static void EvictionTagCallback(object key, object value, EvictionReason reason, object state)
        {
            (value as CancellationTokenSource)?.Cancel();

            var strkey = key as string;
            if (strkey != null)
            {
                ((VersionedCacheProviderBase)state).AddTag(strkey);
            }
        }

        private CancellationTokenSource AddTag(string item)
        {
            var result = _cache.Get(item) as CancellationTokenSource;
            if (result == null)
            {
                result = new CancellationTokenSource();
                var options = new MemoryCacheEntryOptions()
                {
                    Priority = CacheItemPriority.NeverRemove,
                    AbsoluteExpiration = DateTimeOffset.MaxValue,
                };
                options.RegisterPostEvictionCallback(EvictionTagCallback, this);
                _cache.Set(item, result, options);
            }
            return result;
        }

        internal static string CalculateDeprecatedKey(string key)
        {
            return DeprecatedCacheKey + key;
        }

        public T GetOrAdd<T>(string key, TimeSpan expiration, Func<T> getData)
        {
            return GetOrAdd(key, null, expiration, getData);
        }

        public T GetOrAdd<T>(string key, string[] tags, TimeSpan expiration, Func<T> getData,
            bool useSlidingExpiration = false, CacheItemPriority priority = CacheItemPriority.Normal)
        {
            object result = Get(key, tags);

            if (result == null)
            {
                object localLocker = _lockers.GetOrAdd(key, new object());

                bool lockTaken = false;
                try
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();

                    Monitor.TryEnter(localLocker, TryenterTimeoutMs, ref lockTaken);

                    if (lockTaken)
                    {
                        result = Get(key, tags);

                        var time1 = sw.ElapsedMilliseconds;

                        if (result == null)
                        {
                            result = getData();
                            sw.Stop();
                            var time2 = sw.ElapsedMilliseconds;

                            CheckPerformance(key, time1, time2);

                            if (result != null)
                            {
                                Add(result, key, tags, expiration, useSlidingExpiration, priority);
                            }
                        }
                    }
                    else
                    {
                        var time1 = sw.ElapsedMilliseconds;
                        _logger.Log(() => $"Долгое нахождение в ожидании обновления кэша {time1} ms, ключ: {key} ", EventLevel.Warning);

                        result = getData();

                        sw.Stop();
                        var time2 = sw.ElapsedMilliseconds;

                        CheckPerformance(key, time1, time2, false);

                        if (result != null)
                        {
                            Add(result, key, tags, expiration);
                        }
                    }
                }
                finally
                {
                    if (lockTaken)
                    {
                        Monitor.Exit(localLocker);
                    }
                }
            }
            return (T) result;
        }


        private void CheckPerformance(string key, long time1, long time2, bool reportTime1 = true)
        {
            var elapsed = time2 - time1;
            if (elapsed > 5000)
            {
                _logger.Log(() =>
                    $"Долгое получение данных время: {elapsed} мс, ключ: {key}, time1: {time1}, time2: {time2}", EventLevel.Warning);
            }
            if (reportTime1 && time1 > 1000)
            {
                _logger.Log(() => $"Долгая проверка кеша: {time1} мс, ключ: {key}", EventLevel.Warning);
            }
        }
    }
}
