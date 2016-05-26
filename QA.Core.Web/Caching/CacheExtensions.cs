using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;

namespace QA.Core.Web
{
    using System.Data.SqlClient;
    using Cache = System.Web.Caching.Cache;

    /// <summary>
    /// Расширения ASP.NET кеша для поддержки версионного тегирования
    /// </summary>
    public static class CacheExtensions
    {
        /// <summary>
        /// Получить объект из кеша (или положить в кеш) с зависимостями от тегов
        /// </summary>
        /// <typeparam name="T">тип</typeparam>
        /// <param name="cache">кеш</param>
        /// <param name="key">ключ</param>
        /// <param name="lock">объект синхронизации</param>
        /// <param name="factory">получени объекта из источника данных</param>
        /// <param name="tags">версионные теги</param>
        /// <returns></returns>
        public static T Get<T>(this Cache cache, string key,
            object @lock, Func<T> factory,
            DateTime absoluteExpiration, params string[] tags)
        {
            object value;
            if ((value = cache.Get(key)) == null)
            {
                lock (@lock)
                {
                    var dependency = tags.Length > 0 ? cache.CreateTagDependency(tags) : null;
                    if ((value = cache.Get(key)) == null)
                    {
                        value = factory();
                        if (value != null)
                        {
                            cache.Insert(key, value, dependency,
                                absoluteExpiration, Cache.NoSlidingExpiration,
                                CacheItemPriority.Normal, null);
                        }
                    }
                }
            }
            return value == null ?
                default(T) : (T)value;
        }

        /// <summary>
        /// Создавние версионного тега
        /// </summary>
        /// <param name="cache">кеш</param>
        /// <param name="lock">объект для синхронизации</param>
        /// <param name="tags">список тегов</param>
        /// <returns></returns>
        public static CacheDependency CreateTagDependency(this Cache cache, object @lock, params string[] tags)
        {
            lock (@lock)
            {
                return CreateTagDependency(cache, tags);
            }
        }

        public static CacheDependency CreateTagDependency(this Cache cache, params string[] tags)
        {
            if (tags == null || tags.Length < 1)
                return null;

            long version = DateTime.UtcNow.Ticks;
            for (int i = 0; i < tags.Length; ++i)
            {
                cache.Add("_tag:" + tags[i], version, null,
                  DateTime.MaxValue, Cache.NoSlidingExpiration,
                  CacheItemPriority.NotRemovable, null);
            }
            return new CacheDependency(null, tags.Select(s =>
             "_tag:" + s).ToArray());
        }

        /// <summary>
        /// Явная инвалидация тегов.
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="tags">список тегов, которые устарели</param>
        public static void Invalidate(this Cache cache, params string[] tags)
        {
            long version = DateTime.UtcNow.Ticks;
            for (int i = 0; i < tags.Length; ++i)
            {
                cache.Insert("_tag:" + tags[i], version, null,
                  DateTime.MaxValue, Cache.NoSlidingExpiration,
                  CacheItemPriority.NotRemovable, null);
            }
        }
    }

}
