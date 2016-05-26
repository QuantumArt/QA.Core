﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;

namespace QA.Core.Data
{
    /// <summary>
    /// Класс, отслеживающий изменения в БД
    /// </summary>
    public class QPCacheItemWatcher : IDisposable
    {
        private static readonly object _locker = new object();
        private readonly Timer _timer;
        private readonly ConnectionStringSettings _connectionString;
        private Dictionary<int, ContentModification> _modifications = new Dictionary<int, ContentModification>();
        private Dictionary<string, TableModification> _tableModifications = new Dictionary<string, TableModification>();
        private readonly ConcurrentBag<CacheItemTracker> _trackers;
        private readonly IContentInvalidator _invalidator;
        private readonly InvalidationMode _mode;
        private readonly string _cmdText = @"SELECT [CONTENT_ID], [LIVE_MODIFIED], [STAGE_MODIFIED] FROM [CONTENT_MODIFICATION] WITH (NOLOCK)";

        private volatile bool _isBusy = false;
        private readonly TimeSpan _pollPeriod;
        private readonly TimeSpan _dueTime;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode">режим работы</param>
        /// <param name="pollPeriod">интервал опроса бд</param>
        /// <param name="invalidator">объект, инвалидирующий кеш</param>
        /// <param name="connectionName">имя строки подключения</param>
        /// <param name="dueTime">отложенный запуск (ms)</param>
        public QPCacheItemWatcher(InvalidationMode mode, TimeSpan pollPeriod, IContentInvalidator invalidator, string connectionName = "qp_database", int dueTime = 0)
        {
            Throws.IfArgumentNull(_ => connectionName);
            Throws.IfArgumentNull(_ => invalidator);

            _dueTime = TimeSpan.FromMilliseconds(dueTime);
            _pollPeriod = pollPeriod; 
            _timer = new Timer(OnTick, null, 0, Timeout.Infinite);
            _connectionString = ConfigurationManager.ConnectionStrings[connectionName];
            _trackers = new ConcurrentBag<CacheItemTracker>();
            _mode = mode;
            _invalidator = invalidator;
            Throws.IfArgumentNull(_ => _connectionString);
        }

        /// <summary>
        /// Запуск таймера
        /// </summary>
        public void Start()
        {
            _timer.Change(_dueTime, _pollPeriod);  
        }

        /// <summary>
        /// Подключение пользовательского анализатора изменений в БД
        /// </summary>
        /// <param name="tracker"></param>
        public void AttachTracker(CacheItemTracker tracker)
        {
            Throws.IfArgumentNull(tracker, _ => tracker);

            _trackers.Add(tracker);
        }

        /// <summary>
        /// Обработчик таймера
        /// </summary>
        /// <param name="state"></param>
        protected virtual void OnTick(object state)
        {
            if (_isBusy)
                return;

            lock (_locker)
            {
                if (_isBusy)
                    return;

                try
                {
                    _isBusy = true;
                    Dictionary<int, ContentModification> newValues = new Dictionary<int, ContentModification>();
                    Dictionary<string, TableModification> tableChanges = new Dictionary<string, TableModification>();

                    using (SqlConnection con = new SqlConnection(_connectionString.ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(_cmdText, con))
                        {
                            cmd.CommandType = CommandType.Text;
                            con.Open();

                            try
                            {
                                // производим запрос - без этого не будет работать dependency
                                using (var reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        var item = new ContentModification
                                        {
                                            ContentId = Convert.ToInt32(reader["CONTENT_ID"]),
                                            LiveModified = Convert.ToDateTime(reader["LIVE_MODIFIED"]),
                                            StageModified = Convert.ToDateTime(reader["STAGE_MODIFIED"])
                                        };

                                        newValues[item.ContentId] = item;
                                    }
                                }
                            }
                            finally
                            {
                                con.Close();
                            }
                        }
                    }

                    var trackers = _trackers.ToList();

                    foreach (var tracker in trackers)
                    {
                        tracker.TrackChanges(tableChanges);
                    }

                    if (_modifications != null && _modifications.Count > 0)
                    {
                        List<int> items = new List<int>();
                        // check for updates
                        Compare(_modifications, newValues, items);

                        // invalidate
                        if (items.Count > 0)
                        {
                            _invalidator.InvalidateIds(_mode, items.ToArray());
                            ObjectFactoryBase.Logger.Debug(_ => ("Invalidating a set of ids " + string.Join(", ", items)));
                        }
                    }

                    if (_tableModifications != null && _tableModifications.Count > 0)
                    {
                        List<string> items = new List<string>();
                        // check for updates
                        Compare(_tableModifications, tableChanges, items);

                        // invalidate
                        if (items.Count > 0)
                        {
                            _invalidator.InvalidateTables(_mode, items.ToArray());
                            ObjectFactoryBase.Logger.Debug(_ => ("Invalidating a set of tables " + string.Join(", ", items)));
                        }
                    }
                    
                    if (newValues != null && newValues.Count > 0)
                    {
                        _modifications = newValues;
                    }

                    if (tableChanges != null && tableChanges.Count > 0)
                    {
                        _tableModifications = tableChanges;
                    }

                    newValues = null;
                    tableChanges = null;
                }
                catch (Exception ex)
                {
                    ObjectFactoryBase.Logger.ErrorException("qp watcher error", ex);
                    ObjectFactoryBase.Logger.Error("qp watcher error StackTrace: {0}", ex.StackTrace);
                }
                finally
                {
                    _isBusy = false;
                }
            }
        }

        private void Compare<T, V>(Dictionary<T, V> modifications,
            Dictionary<T, V> newValues,
            List<T> idsToUpdate)
                where V : TableModification
        {
            foreach (var item in newValues)
            {
                if (!modifications.ContainsKey(item.Key))
                {
                    idsToUpdate.Add(item.Key);
                    continue;
                }

                var old = modifications[item.Key];

                if ((_mode == InvalidationMode.Live || _mode == InvalidationMode.All) 
                    && (old.LiveModified < item.Value.LiveModified))
                {
                    idsToUpdate.Add(item.Key);
                }

                if ((_mode == InvalidationMode.Stage || _mode == InvalidationMode.All) 
                    && (old.StageModified < item.Value.StageModified))
                {
                    idsToUpdate.Add(item.Key);
                }
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_timer != null)
                _timer.Dispose();
        }

        #endregion

        /// <summary>
        /// Класс, описывающий последнее изменение
        /// </summary>
        protected class ContentModification : TableModification
        {
            public int ContentId { get; set; }
        //    public DateTime LiveModified { get; set; }
        //    public DateTime StageModified { get; set; }
        }
    }
}
