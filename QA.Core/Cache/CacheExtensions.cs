using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace QA.Core.Cache
{
    /// <summary>
    /// расширения для классов кэширования
    /// </summary>
    public static class CacheExtensions
    {
        private static ConcurrentDictionary<string, object> _lockers = new ConcurrentDictionary<string, object>();

        public static ConcurrentDictionary<string, object> Lockers
        {
            get
            {
                return _lockers;
            }
        }

        /// <summary>
        /// Потокобезопасно берет объект из кэша, если его там нет, то вызывает функцию для получения данных
        /// и кладет результат в кэш
        /// </summary>
        /// <typeparam name="T">тип объектов в кэше</typeparam>
        /// <param name="provider">провайдер кэша</param>
        /// <param name="key">тэг, в общем случае представляет имя класса сервиса + имя метода + список параметров</param>
        /// <param name="expiration">время жизни в кэше</param>
        /// <param name="getData">функция для получения данных, если объектов кэше нет. нужно использовать анонимный делегат</param>
        /// <returns>закэшированне данные, если они присутствуют в кэше или результат выполнения функции</returns>
        public static T GetOrAdd<T>(this ICacheProvider provider, string key, TimeSpan expiration, Func<T> getData)
        {
            object result = provider.Get(key);
            if (result == null)
            {
                object localLocker = _lockers.GetOrAdd(key, new object());

                bool lockTaken = false;
                try
                {
                    Monitor.TryEnter(localLocker, 5000, ref lockTaken);
                    if (lockTaken)
                    {
                        result = provider.Get(key);
                        if (result == null)
                        {
                            result = getData();
                            if (result != null)
                            {
                                provider.Set(key, result, expiration);
                            }
                        }
                    }
                    else
                    {
                        result = getData();
                    }
                }
                finally
                {
                    if (lockTaken)
                    {
                        Monitor.Exit(localLocker);
                    }
                }
            }
            return result == null ? default(T) : (T)result;
        }


        /// <summary>
        /// Потокобезопасно берет объект из кэша, если его там нет, то вызывает функцию для получения данных
        /// и кладет результат в кэш
        /// </summary>
        /// <typeparam name="T">тип объектов в кэше</typeparam>
        /// <param name="provider">провайдер кэша</param>
        /// <param name="key">тэг, в общем случае представляет имя класса сервиса + имя метода + список параметров</param>
        /// <param name="tags">список зависимых контентов</param>
        /// <param name="expiration">время жизни в кэше</param>
        /// <param name="getData">функция для получения данных, если объектов кэше нет. нужно использовать анонимный делегат</param>
        /// <returns>закэшированне данные, если они присутствуют в кэше или результат выполнения функции</returns>
        public static T GetOrAdd<T>(this IVersionedCacheProvider provider, string key, string[] tags, TimeSpan expiration, Func<T> getData)
        {
            object result = provider.Get(key, tags);
            if (result == null)
            {
                object localLocker = _lockers.GetOrAdd(
                    string.Concat(key, string.Join("_", tags)), new object());
                bool lockTaken = false;
                try
                {
                    Monitor.TryEnter(localLocker, 5000, ref lockTaken);
                    if (lockTaken)
                    {
                        result = provider.Get(key, tags);
                        if (result == null)
                        {
                            result = getData();
                            if (result != null)
                            {
                                provider.Add(result, key, tags, expiration);
                            }
                        }
                    }
                    else
                    {
                        result = getData();
                    }
                }
                finally
                {
                    if (lockTaken)
                    {
                        Monitor.Exit(localLocker);
                    }
                }
            }
            return result == null ? default(T) : (T)result;
        }
    }
}
