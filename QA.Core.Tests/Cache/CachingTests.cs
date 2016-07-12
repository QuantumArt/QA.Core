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

        static readonly object _locker = new object();

        [TestMethod]
        [TestCategory("Caching")]
        public void VersionedCacheProvider2_test()
        {
            var cache = new VersionedCacheProvider2();

            cache.Add("!test1", "!key1", new string[] { "!tag1", "!tag2" }, TimeSpan.FromMinutes(1));


            Assert.AreEqual("!test1", cache.Get("!key1"));
            cache.Add("!test2", "!key2", new string[] { "!tag1", "!tag2" }, TimeSpan.FromMinutes(1));
            cache.Add("!test3", "!key3", new string[] { "!tag2" }, TimeSpan.FromMinutes(1));

            Assert.AreEqual("!test1", cache.Get("!key1"));
            Assert.AreEqual("!test2", cache.Get("!key2"));
            Assert.AreEqual("!test3", cache.Get("!key3"));

            cache.InvalidateByTag(InvalidationMode.All, "!tag1");

            Assert.IsNull(cache.Get("!key1"));
            Assert.IsNull(cache.Get("!key2"));
            Assert.IsNotNull(cache.Get("!key3"));

            Assert.IsNotNull(cache.Get("!tag1"));
            Assert.IsNotNull(cache.Get("!tag2"));
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
            var cache = new VersionedCacheProvider3();

            cache.Add("!test1", "key1", new string[] { "!tag1", "!tag2" }, TimeSpan.FromMinutes(1));

            var tag1 = cache.Get("!tag1");

            cache.Add("!test2", "key2", new string[] { "!tag1", "!tag2" }, TimeSpan.FromMinutes(1));

            var tag2 = cache.Get("!tag1");

            Assert.AreEqual(tag1, tag2);
        }
    }
}
