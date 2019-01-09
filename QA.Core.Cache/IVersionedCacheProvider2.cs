using System;
using Microsoft.Extensions.Caching.Memory;

namespace QA.Core.Cache
{
    public interface IVersionedCacheProvider2 : IVersionedCacheProvider
    {
        T GetOrAdd<T>(string key, string[] tags, TimeSpan expiration, Func<T> getData, bool slidingExpiration, CacheItemPriority priority);

        T GetOrAdd<T>(string key, TimeSpan expiration, Func<T> getData);
    }

}
