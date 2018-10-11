using System;
using System.Runtime.Caching;

namespace QA.Core.Cache
{
    /// <summary>
    /// Реализует провайдер кеширования данных
    /// </summary>
    public class VersionedCustomerCacheProvider : VersionedCacheProvider3
    {
        public VersionedCustomerCacheProvider(string customerCode) : base(
                customerCode == null ? MemoryCache.Default : new MemoryCache(customerCode),
                false
            )
        {

        }
    }
}
