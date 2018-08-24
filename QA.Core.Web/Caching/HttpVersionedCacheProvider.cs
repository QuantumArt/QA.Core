using System;
using System.Web;
using System.Web.Caching;
#pragma warning disable 1591


namespace QA.Core.Web
{
    using Cache = System.Web.Caching.Cache;
    public class HttpVersionedCacheProvider : ICacheProvider, IVersionedCacheProvider
    {
        #region ICacheProvider Members

        public object Get(string key)
        {
            return GetCache().Get(key);
        }

        public void Set(string key, object data, int cacheTime)
        {
            GetCache().Insert(key, data, null,
                DateTime.Now.AddSeconds(cacheTime),
                Cache.NoSlidingExpiration,
                CacheItemPriority.Normal, null);
        }

        public void Set(string key, object data, TimeSpan expiration)
        {
            GetCache().Insert(key, data, null,
                DateTime.Now + expiration,
                Cache.NoSlidingExpiration,
                CacheItemPriority.Normal, null);
        }

        public bool TryGetValue(string key, out object result)
        {
            result = GetCache().Get(key);

            return result != null;
        }

        public bool IsSet(string key)
        {
            return GetCache().Get(key) != null;
        }

        public void Invalidate(string key)
        {
            GetCache().Remove(key);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        #region IVersionedCacheProvider Members

        public void Add(object data, string key, string[] tags, TimeSpan expiration)
        {
            var cache = GetCache();

            cache.Insert(key, data, cache.CreateTagDependency(tags),
                DateTime.Now + expiration,
                Cache.NoSlidingExpiration,
                CacheItemPriority.Normal, null);
        }

        public object Get(string key, string[] tags)
        {
            return this.Get(key);
        }

        public void InvalidateByTag(InvalidationMode mode, string tag)
        {
            GetCache().Invalidate(tag);
        }

        public void InvalidateByTags(InvalidationMode mode, params string[] tags)
        {
            GetCache().Invalidate(tags);
        }

        public void Invalidate(string key, string[] tags)
        {
            this.Invalidate(key);
        }

        #endregion

        protected virtual System.Web.Caching.Cache GetCache()
        {
            var cache = HttpRuntime.Cache;
            Throws.IfArgumentNull(cache, _ => cache);
            return cache;
        }
    }
}
