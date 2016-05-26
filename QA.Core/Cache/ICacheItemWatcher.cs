using System;
namespace QA.Core.Cache
{
    public interface ICacheItemWatcher
    {
        void AttachTracker(QA.Core.Data.CacheItemTracker tracker);
        void TrackChanges();
    }
}
