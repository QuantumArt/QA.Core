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

        private Task<string> GetTask(VersionedCacheProvider3 cacheProvider)
        {
            return cacheProvider.GetOrAddAsync("testkey", TimeSpan.FromMinutes(1),
                            () => GetData(
                                    TimeSpan.FromMilliseconds(1000 + (new Random()).Next(100, 105)),
                                    "test" + (new Random()).Next()));
        }


        private async Task<string> GetData(TimeSpan wait, string data)
        {
            Interlocked.Increment(ref counter);
            await Task.Delay(wait);
            return data;
        }
    }
}
