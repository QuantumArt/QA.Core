using System;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QA.Core.Tests.Stubs;

namespace QA.Core.Tests
{
    /// <summary>
    /// Общие тесты для IPropertyAccessor
    /// </summary>
    /// <typeparam name="TAccessor"></typeparam>
    [TestClass]
    public abstract class PropertyAccessorsTests<TAccessor>
        where TAccessor : IPropertyAccessor
    {
        protected abstract IPropertyAccessor GetAccessor(Type type, string propertyName);

        #region Generic tests
        [TestCategory("ProertyAccessor: tests")]
        [TestMethod]
        public void Test_ProertyAccessor_Gets()
        {
            var stub = new StubClass2 { StringProperty = "test" };

            IPropertyAccessor accessor = GetAccessor(typeof(StubClass2), "StringProperty");

            Assert.AreEqual("test", (string)accessor.GetValue(stub), GetMessage());
        }

        [TestCategory("ProertyAccessor: tests")]
        [TestMethod]
        public void Test_ProertyAccessor_Sets()
        {
            var stub = new StubClass2 { StringProperty = null };

            IPropertyAccessor accessor = GetAccessor(typeof(StubClass2), "StringProperty");

            accessor.SetValue(stub, "test");

            Assert.AreEqual("test", stub.StringProperty, GetMessage());
        }

        [TestCategory("ProertyAccessor: tests")]
        [TestMethod]
        public void Test_ProertyAccessor_Complex()
        {
            var stub = new StubClass2 { ComplexProperty = new StubClass1() { MyProperty1 = "123" } };

            IPropertyAccessor accessor = GetAccessor(typeof(StubClass2), "ComplexProperty");

            Assert.AreSame(stub.ComplexProperty, accessor.GetValue(stub), GetMessage());
        }

        [TestCategory("ProertyAccessor: tests")]
        [TestMethod]
        public void Test_ProertyAccessor_Inheritance()
        {
            var stub = new StubClass2 { MyProperty1 = 1234 };

            IPropertyAccessor accessor = GetAccessor(typeof(StubClass2), "MyProperty1");

            Assert.AreEqual(1234, (int)accessor.GetValue(stub), GetMessage());
        }

        [TestCategory("ProertyAccessor: tests")]
        [TestMethod]
        public void Test_ProertyAccessor_Virtual_With_Ovewrrides()
        {
            var stub = new StubClass2 { MyProperty3_Backward_Stub = "test" };

            IPropertyAccessor accessor = GetAccessor(typeof(StubClass2), "MyProperty3");

            Assert.AreEqual("test", accessor.GetValue(stub), GetMessage());
        }

        [TestCategory("ProertyAccessor: tests")]
        [TestMethod]
        public void Test_ProertyAccessor_Virtual()
        {
            var stub = new StubClass2 { MyProperty4 = "test" };

            IPropertyAccessor accessor = GetAccessor(typeof(StubClass2), "MyProperty4");

            Assert.AreEqual("test", accessor.GetValue(stub), GetMessage());
        }

        [TestCategory("ProertyAccessor: tests")]
        [TestMethod]
        public void Test_ProertyAccessor_Boxing()
        {
            var stub = new StubClass2 { IntProperty = 1234 };

            IPropertyAccessor accessor = GetAccessor(typeof(StubClass2), "IntProperty");

            Assert.AreEqual(1234, (int)accessor.GetValue(stub), GetMessage());
        }

        [TestCategory("ProertyAccessor: tests")]
        [TestMethod]
        public void Test_ProertyAccessor_NullableInt()
        {
            var stub = new StubClass2 { NullableInt = 1234 };

            IPropertyAccessor accessor = GetAccessor(typeof(StubClass2), "NullableInt");

            Assert.AreEqual(1234, (int)accessor.GetValue(stub), GetMessage());
        }

        #endregion

        private string GetMessage([CallerMemberName]string caller = "", [CallerFilePath] string path = "", [CallerLineNumber] int line = 0)
        {
            return string.Format("\nFailed test '{0}' is inherited by '{1}'. \nTest is defined in {2} [{3}]", caller, this, path, line);
        }
    }
}
