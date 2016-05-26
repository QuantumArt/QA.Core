using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QA.Core.Cache;

namespace QA.Core.Tests
{
    [TestClass]
    public class CacheTests
    {
        [TestMethod]
        public void GetOrAddTest01()
        {
            //ThreadStart threadDelegate = new ThreadStart(() => { CacheProvider.GetOrAdd<string>("key1", new TimeSpan(0, 1, 15), () => { Thread.Sleep(50000); return "test1"; }); });
            //Thread newThread = new Thread(threadDelegate);
            //newThread.Start();

            //Thread.Sleep(2000);

            //ThreadStart threadDelegate2 = new ThreadStart(() => { CacheProvider.GetOrAdd<string>("key1", new TimeSpan(0, 1, 15), () => { return "test2"; }); });
            //Thread newThread2 = new Thread(threadDelegate2);
            //newThread2.Start();

            //Debug.Write("Start");

            //Thread.Sleep(100000);
        }

        public static ICacheProvider CacheProvider
        {
            get { return new CacheProvider(); }
        }
    }
}
