using System;
using System.Collections.Concurrent;
using System.Text;

namespace QA.Core
{
    /// <summary>
    /// Кеш-провайдер с поддержкой версионированного тегирования
    /// </summary>
    public class VersionedCacheProvider : IVersionedCacheProvider, ICacheProvider
    {
        private object _locker = new object();
        private ILogger _logger;
        private ICacheProvider _innerCache;
        private TimeSpan _tagLifeTime;
        private readonly ConcurrentDictionary<Type, object> lockers = new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// Создает экземпляр класса.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="innerCache"></param>
        public VersionedCacheProvider(ILogger logger, ICacheProvider innerCache)
        {
            _logger = logger;
            _innerCache = innerCache;

            // hardcode detected
            // TODO: get from config
            _tagLifeTime = TimeSpan.FromDays(1);
        }

        private bool TryGetCache(out ICacheProvider cache)
        {
            cache = _innerCache;
            return _innerCache != null;
        }

        /// <summary>
        /// Добавление данных в кэш
        /// </summary>
        /// <param name="data">данные</param>
        /// <param name="key">тэг, в общем случае представляет имя класса сервиса + имя метода + список параметров</param>
        /// <param name="tags">список зависимых контентов</param>
        /// <param name="expiration">время жизни в кэше</param>
        public void Add(object data, string key, string[] tags, TimeSpan expiration)
        {
            try
            {
                ICacheProvider cache;
                if (TryGetCache(out cache))
                {
                    cache.Set(CalculateTag(key, tags), data, expiration);
                }
                else
                {
                    _logger.Error("An error has been occured while getting the datacache instance.");
                }
            }
            catch (Exception e)
            {
                _logger.ErrorException("An error has been occured while adding an item.", e);
            }
        }

        /// <summary>
        /// Получение данных их кэша
        /// </summary>
        /// <param name="key">тэг, в общем случае представляет имя класса сервиса + имя метода + список параметров</param>
        /// <param name="tags">список зависимых контентов</param>
        /// <returns>закэшированне данные, если они присутствуют в кэше</returns>
        public object Get(string key, string[] tags)
        {
            try
            {
                ICacheProvider cache;
                if (TryGetCache(out cache))
                {
                    return cache.Get(CalculateTag(key, tags));
                }
                else
                {
                    _logger.Error("An error has been occured while getting the datacache instance.");
                }
            }
            catch (Exception e)
            {
                _logger.ErrorException("An error has been occured while getting the item.", e);
            }

            return null;
        }

        /// <summary>
        /// Сообщает, что данные этого тега (контента) обновились
        /// </summary>
        /// <param name="tag">тег</param>
        public void InvalidateByTag(InvalidationMode mode, string tag)
        {
            InvalidateByTags(mode, tag);
        }

        /// <summary>
        /// Сообщает, что данные этого тега (контента) обновились
        /// </summary>
        /// <param name="tag">тег</param>
        public void InvalidateByTags(InvalidationMode mode, params string[] tags)
        {
            try
            {
                ICacheProvider cache;
                if (TryGetCache(out cache))
                {
                    foreach (var tag in tags)
                    {
                        cache.Set(tag, DateTime.Now, _tagLifeTime);
                    }
                }
                else
                {
                    _logger.Error("An error has been occured while getting the datacache instance.");
                }
            }
            catch (Exception e)
            {
                _logger.ErrorException("An error has been occured while invalidationg the item.", e);
            }
        }

        /// <summary>
        /// Удаляет объект из кеша
        /// </summary>
        /// <param name="key">тэг, в общем случае представляет имя класса сервиса + имя метода + список параметров</param>
        /// <param name="tags">список зависимых контентов</param>
        public void Invalidate(string key, string[] tags)
        {
            try
            {
                ICacheProvider cache;
                if (TryGetCache(out cache))
                {
                    var fullKey = CalculateTag(key, tags);
                    cache.Invalidate(fullKey);
                }
                else
                {
                    _logger.Error("An error has been occured while getting the datacache instance.");
                }
            }
            catch (Exception e)
            {
                _logger.ErrorException("An error has been occured while invalidationg the item.", e);
            }
        }

        /// <summary>
        /// Вычисление ключа кеша с учетом версий тегов
        /// </summary>
        /// <param name="key"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        private string CalculateTag(string key, string[] tags)
        {
            ICacheProvider cache;
            if (TryGetCache(out cache))
            {
                StringBuilder result = new StringBuilder(key);
                result.Append("-");
                foreach (string name in tags)
                {
                    DateTime? dt = DateTime.Now;
                    try
                    {
                       
                        dt = (DateTime?)cache.Get(name);
                        if (!dt.HasValue)
                        {
                            dt = DateTime.Now;
                            cache.Set(name, dt, _tagLifeTime);
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.ErrorException("An error has been occured while the tag.", e);
                    }
                    result.Append("-");
                    result.Append(name);
                    result.Append(dt.Value.ToString("MMddHHmmss"));
                }
                return result.ToString();
            }
            else
            {
                _logger.Error("An error has been occured while getting the datacache instance.");
                return string.Empty;
            }
        }

        object ICacheProvider.Get(string key)
        {
            return _innerCache.Get(key);
        }

        void ICacheProvider.Set(string key, object data, int cacheTime)
        {
            _innerCache.Set(key, data, cacheTime);
        }

        void ICacheProvider.Set(string key, object data, TimeSpan expiration)
        {
            _innerCache.Set(key, data, expiration);
        }

        bool ICacheProvider.TryGetValue(string key, out object result)
        {
            return _innerCache.TryGetValue(key, out result);
        }

        bool ICacheProvider.IsSet(string key)
        {
            return _innerCache.IsSet(key);
        }

        void ICacheProvider.Invalidate(string key)
        {
            _innerCache.Invalidate(key);
        }


        void IDisposable.Dispose()
        {
            _innerCache.Dispose();
        }

        
    }
}
