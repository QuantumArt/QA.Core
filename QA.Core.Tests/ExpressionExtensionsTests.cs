using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QA.Core.Linq;
using QA.Core.Tests.Stubs;

namespace QA.Core.Tests
{
    [TestClass]
    public class ExpressionExtensionsTests
    {
        [TestMethod]
        public void Test_ExpressionExtensions_single()
        {
            StubClass2 s = new StubClass2();

            var name = ExpressionExtensions.GetPropertyName<StubClass2, string>(m => s.StringProperty);

            Assert.AreEqual("s.StringProperty", name);
        }

        [TestMethod]
        public void Test_ExpressionExtensions_methodname()
        {
            StubClass2 s = new StubClass2();

            var name = ExpressionExtensions.GetPropertyName<object, string>(m => s.Method());

            Assert.AreEqual("Method", name);
        }
    }
}
