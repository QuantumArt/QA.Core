using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QA.Core.Tests
{
    [TestClass]
    public class ObjectDumperTests
    {
        [TestMethod]
        public void TestIgnoreAttr()
        {
            ObjectDumper.DumpObject(new ServiceResultTest<string>());
        }
    }

    public class ServiceResultTest<TResult>
    {
        /// <summary>
        /// Возвращаемый объект
        /// </summary>

        public TResult Result { get; set; }

        /// <summary>
        /// Возвращает Result при условии что IsSucceeded==true и он не null
        /// в противном случае бросает эксепшн
        /// </summary>
        [IgnoreWhileDumping]
        public TResult ResultEnsured
        {
            get
            {
                throw new Exception("Method is not expected to call");
            }
        }
    }

}
