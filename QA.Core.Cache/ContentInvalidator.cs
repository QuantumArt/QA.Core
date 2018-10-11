using QA.Core.Logger;

#pragma warning disable 1591

namespace QA.Core.Cache
{
    public abstract class ContentInvalidator : IContentInvalidator
    {
        private readonly IVersionedCacheProvider _cacheProvider;
        private readonly ILogger _logger;

        public ContentInvalidator(IVersionedCacheProvider cacheProvider, ILogger logger)
        {
            _cacheProvider = cacheProvider;
            _logger = logger;
        }

        public virtual void InvalidateIds(InvalidationMode mode, params int[] contentIds)
        {
            var names = ResolveKeys(contentIds);
            InvalidateKeys(mode, names);
        }

        public virtual void InvalidateKeys(InvalidationMode mode, params string[] keys)
        {
            _logger.Debug(() => ("Invalidating a set of keys " + string.Join(", ", keys)));

            if (keys == null || keys.Length == 0)
                return;

            _cacheProvider.InvalidateByTags(mode, keys);
        }

        public virtual void InvalidateTables(InvalidationMode mode, params string[] tableNames)
        {
            var keys = ResolveTableNames(tableNames);
            InvalidateKeys(mode, keys);
        }

        protected abstract string[] ResolveKeys(int[] contentIds);
        protected abstract string[] ResolveTableNames(string[] tableNames);
    }
}
