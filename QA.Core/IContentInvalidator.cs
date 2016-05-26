using System;
namespace QA.Core
{
    public interface IContentInvalidator
    {
        void InvalidateIds(InvalidationMode mode, params int[] contentIds);
        void InvalidateTables(InvalidationMode mode, params string[] tableNames);
        void InvalidateKeys(InvalidationMode mode, params string[] keys);
    }
}
