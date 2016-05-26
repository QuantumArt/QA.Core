using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QA.Core.Web
{
    public abstract class ContentInvalidator : IContentInvalidator
    {
        private readonly IVersionedCacheProvider _cacheProvider;

        public ContentInvalidator(IVersionedCacheProvider cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        public virtual void InvalidateIds(InvalidationMode mode, params int[] contentIds)
        {
            var names = ResolveKeys(contentIds);
            InvalidateKeys(mode, names);
        }

        public virtual void InvalidateKeys(InvalidationMode mode, params string[] keys)
        {
            ObjectFactoryBase.Logger.Debug(_ => ("Invalidating a set of keys " + string.Join(", ", keys)));

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
