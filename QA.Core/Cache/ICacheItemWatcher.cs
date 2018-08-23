using System;
#pragma warning disable 1591

namespace QA.Core.Cache
{
    public interface ICacheItemWatcher
    {
        void AttachTracker(QA.Core.Data.CacheItemTracker tracker);
        void TrackChanges();
    }
}
