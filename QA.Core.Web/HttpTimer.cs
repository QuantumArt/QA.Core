using System;
using System.Web;
using QA.Core.Logger;
#pragma warning disable 1591

namespace QA.Core.Web
{
    using System.Web.Caching;

    /// <summary>
    /// Класс, осуществляющий работу таймера для http-приложения. Не забирает поток из пула приложения.
    /// </summary>
    public class HttpTimer : IDisposable
    {
        private Config _config;
        private static readonly object _syncLock = new object();
        private volatile bool _isStarted;
        private string _key;

        public HttpTimer(Config config)
        {
            Throws.IfArgumentNull(config, _ => config);
            _config = config;
            _key = "HttpTimer_" + Guid.NewGuid();
        }

        public void Start()
        {
            if (_isStarted)
                return;

            lock (_syncLock)
            {
                if (_isStarted)
                    return;

                AddItem();
            }
        }

        private void AddItem()
        {
            var cache = GetCache();
            Throws.IfNot(cache != null, "Cache is null. This class cannot be used outside the http environment.");

            cache.Insert(_key, DateTime.Now, null,
                Cache.NoAbsoluteExpiration,
                _config._period,
                CacheItemPriority.NotRemovable, OnRemove);
        }

        public void Stop()
        {
            if (!_isStarted)
            {
                return;
            }
            lock (_syncLock)
            {
                if (!_isStarted)
                    return;

                var cache = GetCache();
                if (cache != null)
                {
                    cache.Remove(_key);
                }

            }
        }

        protected virtual void OnRemove(string key, object value, CacheItemRemovedReason reason)
        {
            if (key != _key)
                return;

            if (reason == CacheItemRemovedReason.DependencyChanged || reason == CacheItemRemovedReason.Removed)
            {
                // объект был удален пользовательской логикой
                return;
            }

            try
            {
                _config._callback(_config._state);
            }
            catch (Exception ex)
            {
                ObjectFactoryBase
                    .Resolve<ILogger>()
                    .ErrorException("Http timer exception", ex);
            }

            AddItem();
        }

        protected virtual Cache GetCache()
        {
            try
            {
                return HttpRuntime.Cache;
            }
            catch
            {
                return null;
            }
        }

        #region IDisposable Members
        public void Dispose()
        {
            Stop();
        }
        #endregion

        #region Nested class
        public class Config
        {
            internal Action<object> _callback;
            internal TimeSpan _period;
            internal object _state;
            /// <summary>
            /// Настройки таймера
            /// </summary>
            /// <param name="period">период срабатывания</param>
            /// <param name="callback">метод, который будет вызываться</param>
            /// <param name="state">произвольный объект, можно null</param>
            public Config(TimeSpan period, Action<object> callback, object state)
            {
                Throws.IfArgumentNull(period, _ => period);
                Throws.IfArgumentNull(callback, _ => callback);

                _period = period;
                _callback = callback;
                _state = state;
            }
        }
        #endregion
    }
}
