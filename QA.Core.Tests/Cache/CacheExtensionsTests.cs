using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Caching;
using System.Threading;
using System.Threading.Tasks;
using QA.Core.Cache;
using QA.Core.Logger;

namespace QA.Core.Tests
{
    [TestClass]
    public class CacheExtensionsTests
    {
        public CacheExtensionsTests()
        {
            CacheExtensions._logger = new NullLogger();
            CacheExtensions._providerType = new Lazy<bool>(() => true);
            CacheExtensions._vProviderType = new Lazy<bool>(() => true);
        }
        private int counter = 0;
        [TestMethod]
        [TestCategory("Caching")]
        public void Async_test1()
        {
            var cacheProvider = new VersionedCacheProvider3();
            int count = 50;
            List<Task<string>> tasks = new List<Task<string>>();
            for (int i = 0; i < count; i++)
            {
                tasks.Add(GetTask(cacheProvider));
                Thread.Sleep(10);
                tasks.Add(GetTask(cacheProvider));
            }

            Task.WaitAll(tasks.ToArray());

            Assert.AreEqual(1, counter);

            var duplicates = tasks.Select(x => x.Result).Distinct().Count();

            Assert.AreEqual(1, duplicates);
        }

        //[TestMethod]
        //[TestCategory("Caching")]
        /// <summary>
        /// This test demonstrates deadlock caused by semaphoreslim reentrancy
        /// </summary>
        public void Async_reentrancy_test1()
        {
            var cacheProvider = new VersionedCacheProvider3();
            int count = 3;
            List<Task<string>> tasks = new List<Task<string>>();
            for (int i = 0; i < count; i++)
            {
                tasks.Add(GetTaskRecursive(cacheProvider));
                Thread.Sleep(10);
                tasks.Add(GetTaskRecursive(cacheProvider));
            }

            Task.WaitAll(tasks.ToArray());

            Assert.AreEqual(1, counter);

            var duplicates = tasks.Select(x => x.Result).Distinct().Count();

            Assert.AreEqual(1, duplicates);
        }


        private Task<string> GetTask(IVersionedCacheProvider cacheProvider)
        {
            return cacheProvider.GetOrAddAsync("testkey", TimeSpan.FromMinutes(1),
                            () => GetData(
                                    TimeSpan.FromMilliseconds(1000 + (new Random()).Next(100, 105)),
                                    "test" + (new Random()).Next()));
        }

        private Task<string> GetTaskRecursive(IVersionedCacheProvider cacheProvider)
        {
            return cacheProvider.GetOrAddAsync("testkey", TimeSpan.FromMinutes(1),
                            () => GetData(
                                    cacheProvider,
                                    TimeSpan.FromMilliseconds(1000 + (new Random()).Next(100, 105)),
                                    "test" + (new Random()).Next()));
        }


        private async Task<string> GetData(TimeSpan wait, string data)
        {
            Interlocked.Increment(ref counter);
            await Task.Delay(wait);
            return data;
        }


        private async Task<string> GetData(IVersionedCacheProvider cacheProvider, TimeSpan wait, string data)
        {
            Interlocked.Increment(ref counter);
            await Task.Delay(wait);
            await GetTask(cacheProvider);
            await Task.Delay(wait);
            return data;
        }

        [TestMethod]
        [TestCategory("Caching")]
        public void Async_CachingGroup()
        {
            var cacheProvider = new VersionedCacheProvider3();
            int countInCache = 10;
            int count = 50;
            string[] data = Enumerable.Range(0, countInCache).Select(v => v.ToString()).ToArray();
            foreach (string item in data) {
                cacheProvider.Set(item, $"{item}value", TimeSpan.FromSeconds(20));
            }
            Assert.AreEqual(0, counter);
            List<Task<List<string>>> tasks = new List<Task<List<string>>>();
            for (int i = 0; i < count; i++)
            {
                tasks.Add(GetGroupData(cacheProvider, data));
                Thread.Sleep(10);
                tasks.Add(GetGroupData(cacheProvider, data));
            }
            Task.WaitAll(tasks.ToArray());

            //Данные по новой не читались
            Assert.AreEqual(0, counter);

            //Ожидаем окончания времени жизни куки
            Thread.Sleep(25000);

            tasks = new List<Task<List<string>>>();
            for (int i = 0; i < count; i++)
            {
                tasks.Add(GetGroupData(cacheProvider, data));
                Thread.Sleep(10);
                tasks.Add(GetGroupData(cacheProvider, data));
            }
            Task.WaitAll(tasks.ToArray());

            //Обновлялась данные только один раз
            Assert.AreEqual(data.Length, counter);

            var duplicates = tasks.SelectMany(x => x.Result).Distinct().Count();

            Assert.AreEqual(countInCache, duplicates);
        }

        [TestMethod]
        [TestCategory("Caching")]
        public void Async_PartialUpdateCachingGroup()
        {
            var cacheProvider = new VersionedCacheProvider3();
            int countInCache = 10;
            int count = 50;
            string[] data = Enumerable.Range(0, countInCache).Select(v => v.ToString()).ToArray();
            for (int i = 0; i < data.Length; i++)
            {
                cacheProvider.Set(data[i], $"{data[i]}value", TimeSpan.FromSeconds(((i % 2) + 1) * 20));
            }
            Assert.AreEqual(0, counter);
            List<Task<List<string>>> tasks = new List<Task<List<string>>>();
            for (int i = 0; i < count; i++)
            {
                tasks.Add(GetGroupData(cacheProvider, data));
                Thread.Sleep(10);
                tasks.Add(GetGroupData(cacheProvider, data));
            }
            Task.WaitAll(tasks.ToArray());

            //Данные по новой не читались;
            Assert.AreEqual(0, counter);

            //Ожидаем окончания времени жизни четных записей
            Thread.Sleep(25000);

            tasks = new List<Task<List<string>>>();
            for (int i = 0; i < count; i++)
            {
                tasks.Add(GetGroupData(cacheProvider, data));
                Thread.Sleep(10);
                tasks.Add(GetGroupData(cacheProvider, data));
            }
            Task.WaitAll(tasks.ToArray());

            //Обновилась только половина данных (остальные не устарели)
            Assert.AreEqual(data.Length / 2, counter);

            var duplicates = tasks.SelectMany(x => x.Result).Distinct().Count();

            Assert.AreEqual(countInCache, duplicates);
        }

        private Task<List<string>> GetGroupData(IVersionedCacheProvider cacheProvider, string[] data)
        {
            return cacheProvider.GetOrAddValuesAsync(
                data,
                "",
                TimeSpan.FromMinutes(2),
                async (excludekeysKeys) =>
                {
                    Dictionary<string, string> result = new Dictionary<string, string>();
                    foreach (string key in excludekeysKeys)
                    {
                        result.Add(key, await GetData(TimeSpan.FromMilliseconds(1000 + (new Random()).Next(100, 105)), $"{key}value"));
                    }
                    return result;
                });
        }

    }
}
