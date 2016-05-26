using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace QA.Core.Tests
{
    [TestClass]
    public class CachingTests
    {
        private static readonly MemoryCache cache = MemoryCache.Default;

        [TestMethod]
        [TestCategory("Caching")]
        public void TestMethod1()
        {
            var obj = "teset";
            var t1 = GetOrAdd("test1", () => "val1", 120, "tag1", "tag2");
            var t2 = GetOrAdd("test1", () => "val2", 120, "tag1", "tag2");

            GetOrAdd("tag1", () => DateTime.Now + "_1", 120);
            GetOrAdd("tag2", () => DateTime.Now + "_2", 120);


            var t3 = GetOrAdd("test1", () => "val3", 120, "tag1", "tag2");
            var t4 = GetOrAdd("test1", () => "val4", 120, "tag1", "tag2");

            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.Now + TimeSpan.FromSeconds(250)
            };

            cache.Set("tag1", "_11", policy);

            var t5 = GetOrAdd("test1", () => "val4", 120, "tag1", "tag2");
        }


        [TestMethod]
        [TestCategory("Caching")]
        public void Test_cache_item_evicting_then_deprecated()
        {
            object val = "123123";
            string key = "key1";
            var dependencies = new string[] { "tag1", "tag2" };

            cache.Set("tag1", "", new CacheItemPolicy { AbsoluteExpiration = DateTime.Now.AddDays(1) });
            cache.Set("tag2", "", new CacheItemPolicy { AbsoluteExpiration = DateTime.Now.AddDays(1) });

            Add(key, val, dependencies);

            cache.Set("tag1", "", new CacheItemPolicy { AbsoluteExpiration = DateTime.Now.AddDays(1) });

            Assert.IsNull(cache.Get(key));

            Assert.IsNotNull(cache.Get("__deprecated__cache_element_" + key));
        }



//        [TestMethod]
        [TestCategory("Caching")]
        public void Test_cache_item_evicting_then_deprecated_multithread()
        {
            object val = "123123";
            string key = "key1";
            var dependencies = new string[] { "tag1", "tag2" };

            cache.Set("tag1", "", new CacheItemPolicy { AbsoluteExpiration = DateTime.Now.AddDays(1) });
            cache.Set("tag2", "", new CacheItemPolicy { AbsoluteExpiration = DateTime.Now.AddDays(1) });

            List<Task> tasks = new List<Task>();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    while (true)
                    {
                        var value = GetOrAdd("key123", () =>
                                {
                                    Thread.Sleep(2500);
                                    return DateTime.Now.ToLongTimeString();
                                }, "tag1", "tag2");

                    }
                }));


            }

            tasks.Add(Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(10000);
                    Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!Invalidate!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    cache.Set("tag1", "", new CacheItemPolicy { AbsoluteExpiration = DateTime.Now.AddDays(1) });
                }
            }));

            Task.WhenAll(tasks).Wait();


            Assert.IsNull(cache.Get(key));

            Assert.IsNotNull(cache.Get("__deprecated__cache_element_" + key));
        }


        static readonly object _locker = new object();
        private static object GetOrAdd(string key, Func<object> getData, params string[] dependencies)
        {
            var result = cache.Get(key);

            if (result == null)
            {
                bool lockTaken = false;
                try
                {
                    Monitor.TryEnter(_locker, ref lockTaken);
                    if (lockTaken)
                    {
                        result = cache.Get(key);
                        if (result != null)
                        {
                            Debug.WriteLine("get actual with double lock " + result);

                            return result;
                        }
                        result = getData();
                        if (result != null)
                        {
                            Debug.WriteLine("****************** get from delegate " + result);
                            Add(key, result, dependencies);
                        }

                    }
                    else
                    {
                        // пытаемся взять устаревший объект
                        result = cache.Get("__deprecated__cache_element_" + key);
                        if (result != null)
                        {
                            Debug.WriteLine("get deprecated" + result);

                            return result;
                        }

                        /* 
                         * пока не делаем
                        result = cache.Get(key);
                        if (result != null)
                            return result;
                         * 
                         * */

                        result = getData();
                        if (result != null)
                        {
                            Debug.WriteLine("****************** get from delegate ** outside lock ** " + result);
                            Add(key, result, dependencies);
                            return result;
                        }
                    }
                }
                finally
                {
                    if (lockTaken)
                        Monitor.Exit(_locker);
                }
            }

            Debug.WriteLine("get actual" + result);

            return result;
        }

        private static void Add(string key, object val, string[] dependencies)
        {
            var ci = new CacheItem(key) { Value = val };

            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.Now + TimeSpan.FromSeconds(100),

            };

            policy.RemovedCallback = (args) =>
                {
                    if (args.CacheItem != null)
                    {
                        if (args.RemovedReason == CacheEntryRemovedReason.ChangeMonitorChanged
                            || args.RemovedReason == CacheEntryRemovedReason.Expired)
                        {
                            var value = args.CacheItem.Value;
                            if (value != null)
                            {
                                args.Source.Set(new CacheItem("__deprecated__cache_element_" + args.CacheItem.Key) { Value = value },
                                    new CacheItemPolicy { AbsoluteExpiration = DateTime.Now.AddSeconds(30) });
                            }
                        }
                    }
                };

            if (dependencies != null && dependencies.Length > 0)
            {
                policy.ChangeMonitors.Add(
                    MemoryCache.Default.CreateCacheEntryChangeMonitor(dependencies)
                );
            }

            cache.Add(ci, policy);
        }

        #region helpers
        private static void OnChangedCallback(object state)
        {
            var keys = (IEnumerable<string>)state;
            if (keys != null)
                Console.WriteLine(string.Join("|", keys.ToArray()));
        }

        private object GetOrAdd(string key, Func<object> aquire, int seconds, params string[] dependencies)
        {
            if (cache.Contains(key)) // here cache is MemoryCache.Default
            {
                return cache.Get(key);
            }

            var result = aquire();
            AddToCache(key, result, seconds, dependencies);

            return result;
        }

        private void AddToCache(string key, object value, int seconds, params string[] dependencies)
        {
            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.Now + TimeSpan.FromSeconds(seconds)
            };

            if (dependencies != null && dependencies.Length > 0)
            {
                policy.ChangeMonitors.Add(
                    MemoryCache.Default.CreateCacheEntryChangeMonitor(dependencies)
                );
            }

            cache.Add(key, value, policy);
        }
        #endregion

        [TestMethod]
        [TestCategory("Caching")]
        public void VersionedCacheProvider2_test()
        {
            var cache = new VersionedCacheProvider2();

            cache.Add("test1", "key1", new string[] { "tag1", "tag2" }, TimeSpan.FromMinutes(1));


            Assert.AreEqual("test1", cache.Get("key1"));
            cache.Add("test2", "key2", new string[] { "tag1", "tag2" }, TimeSpan.FromMinutes(1));
            cache.Add("test3", "key3", new string[] { "tag2" }, TimeSpan.FromMinutes(1));

            Assert.AreEqual("test1", cache.Get("key1"));
            Assert.AreEqual("test2", cache.Get("key2"));
            Assert.AreEqual("test3", cache.Get("key3"));

            cache.InvalidateByTag(InvalidationMode.All, "tag1");

            Assert.IsNull(cache.Get("key1"));
            Assert.IsNull(cache.Get("key2"));
            Assert.IsNotNull(cache.Get("key3"));

            Assert.IsNotNull(cache.Get("tag1"));
            Assert.IsNotNull(cache.Get("tag2"));
        }

        [TestMethod]
        [TestCategory("Caching")]
        public void VersionedCacheProvider3_test()
        {
            var cache = new VersionedCacheProvider3();

            cache.Add("test1", "key1", new string[] { "tag1", "tag2" }, TimeSpan.FromMinutes(1));


            Assert.AreEqual("test1", cache.Get("key1"));
            cache.Add("test2", "key2", new string[] { "tag1", "tag2" }, TimeSpan.FromMinutes(1));
            cache.Add("test3", "key3", new string[] { "tag2" }, TimeSpan.FromMinutes(1));

            Assert.AreEqual("test1", cache.Get("key1"));
            Assert.AreEqual("test2", cache.Get("key2"));
            Assert.AreEqual("test3", cache.Get("key3"));

            cache.InvalidateByTag(InvalidationMode.All, "tag1");

            Assert.IsNull(cache.Get("key1"));
            Assert.IsNull(cache.Get("key2"));
            Assert.IsNotNull(cache.Get("key3"));

            Assert.IsNotNull(cache.Get("tag1"));
            Assert.IsNotNull(cache.Get("tag2"));
        }

        [TestMethod]
        [TestCategory("Caching")]
        public void VersionedCacheProvider2_test1()
        {
            var cache = new VersionedCacheProvider2();

            cache.Add("test1", "key1", new string[] { "tag1", "tag2" }, TimeSpan.FromMinutes(1));

            var tag1 = cache.Get("tag1");

            cache.Add("test2", "key2", new string[] { "tag1", "tag2" }, TimeSpan.FromMinutes(1));

            var tag2 = cache.Get("tag1");

            Assert.AreEqual(tag1, tag2);
        }
        [TestMethod]
        [TestCategory("Caching")]
        public void VersionedCacheProvider3_test1()
        {
            var cache = new VersionedCacheProvider2();

            cache.Add("test1", "key1", new string[] { "tag1", "tag2" }, TimeSpan.FromMinutes(1));

            var tag1 = cache.Get("tag1");

            cache.Add("test2", "key2", new string[] { "tag1", "tag2" }, TimeSpan.FromMinutes(1));

            var tag2 = cache.Get("tag1");

            Assert.AreEqual(tag1, tag2);
        }
    }
}
