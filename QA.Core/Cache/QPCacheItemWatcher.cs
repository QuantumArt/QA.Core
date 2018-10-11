using System;
using System.Configuration;
using System.Threading;
using QA.Core.Cache;

#pragma warning disable 1591


namespace QA.Core.Data
{
    /// <summary>
    /// Класс, отслеживающий изменения в БД
    /// </summary>
    public class QPCacheItemWatcher : CacheItemWatcherBase
    {
        public QPCacheItemWatcher(InvalidationMode mode, IContentInvalidator invalidator, string connectionName = "qp_database")
            : this(mode, Timeout.InfiniteTimeSpan, invalidator, connectionName, 0, false)
        {
        }

        public static string GetConnectionString(string connectionName)
        {
            string connectionString;
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Access to connectionString caused an exception", ex);
            }

            Throws.IfArgumentNullOrEmpty(connectionString, nameof(connectionString));
            return connectionString;
        }

        public QPCacheItemWatcher(InvalidationMode mode, TimeSpan pollPeriod, IContentInvalidator invalidator,
            string connectionName = "qp_database",
            int dueTime = 0,
            bool useTimer = true,
            Func<Tuple<int[], string[]>, bool> onInvalidate = null)
            : base(mode, pollPeriod, invalidator, GetConnectionString(connectionName), ObjectFactoryBase.Logger,
                dueTime, useTimer, onInvalidate)
        {
        }

    }
}
