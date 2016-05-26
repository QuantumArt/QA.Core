using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QA.Core.Tests.Stubs;

namespace QA.Core.Tests
{
    [TestClass]
    public class PropertyAccessors
    {
        #region FastPropertyAccessor
        [TestCategory("ProertyAccessor: tests")]
        [TestMethod]
        public void Test_ProertyAccessor_Fast_Gets()
        {
            var stub = new StubClass2 { StringProperty = "test" };

            IPropertyAccessor accessor = new FastPropertyAccessor(typeof(StubClass2), "StringProperty");

            Assert.AreEqual("test", (string)accessor.GetValue(stub));
        }

        [TestCategory("ProertyAccessor: tests")]
        [TestMethod]
        public void Test_ProertyAccessor_Fast_Sets()
        {
            var stub = new StubClass2 { StringProperty = null };

            IPropertyAccessor accessor = new FastPropertyAccessor(typeof(StubClass2), "StringProperty");

            accessor.SetValue(stub, "test");

            Assert.AreEqual("test", stub.StringProperty);
        }

        [TestCategory("ProertyAccessor: tests")]
        [TestMethod]
        public void Test_ProertyAccessor_Fast_Complex()
        {
            var stub = new StubClass2 { ComplexProperty = new StubClass1() { MyProperty1 = "123" } };

            IPropertyAccessor accessor = new FastPropertyAccessor(typeof(StubClass2), "ComplexProperty");

            Assert.AreSame(stub.ComplexProperty, accessor.GetValue(stub));
        }

        #endregion

        #region ReflectedPropertyAccessor
        [TestCategory("ProertyAccessor: tests")]
        [TestMethod]
        public void Test_ProertyAccessor_Reflection_Gets()
        {
            var stub = new StubClass2 { StringProperty = "test" };

            IPropertyAccessor accessor = new ReflectedPropertyAccessor(typeof(StubClass2), "StringProperty");

            Assert.AreEqual("test", (string)accessor.GetValue(stub));
        }

        [TestCategory("ProertyAccessor: tests")]
        [TestMethod]
        public void Test_ProertyAccessor_Reflection_Sets()
        {
            var stub = new StubClass2 { StringProperty = null };

            IPropertyAccessor accessor = new ReflectedPropertyAccessor(typeof(StubClass2), "StringProperty");

            accessor.SetValue(stub, "test");

            Assert.AreEqual("test", stub.StringProperty);
        }
        #endregion

        #region EmitPropertyAccessor
        [TestCategory("ProertyAccessor: tests")]
        [TestMethod]
        public void Test_ProertyAccessor_Emit_Gets()
        {
            var stub = new StubClass2 { StringProperty = "test" };

            IPropertyAccessor accessor = new EmitPropertyAccessor(typeof(StubClass2), "StringProperty");

            Assert.AreEqual("test", (string)accessor.GetValue(stub));
        }

        [TestCategory("ProertyAccessor: tests")]
        [TestMethod]
        public void Test_ProertyAccessor_Emit_Inheritance()
        {
            var stub = new StubClass2 { MyProperty1 = 1234 };

            IPropertyAccessor accessor = new EmitPropertyAccessor(typeof(StubClass2), "MyProperty1");

            Assert.AreEqual(1234, (int)accessor.GetValue(stub));
        }

        [TestCategory("ProertyAccessor: tests")]
        [TestMethod]
        public void Test_ProertyAccessor_Emit_Virtual_With_Ovewrrides()
        {
            var stub = new StubClass2 { MyProperty3_Backward_Stub = "test" };

            IPropertyAccessor accessor = new EmitPropertyAccessor(typeof(StubClass2), "MyProperty3");

            Assert.AreEqual("test", accessor.GetValue(stub));
        }

        [TestCategory("ProertyAccessor: tests")]
        [TestMethod]
        public void Test_ProertyAccessor_Emit_Virtual()
        {
            var stub = new StubClass2 { MyProperty4 = "test" };

            IPropertyAccessor accessor = new EmitPropertyAccessor(typeof(StubClass2), "MyProperty4");

            Assert.AreEqual("test", accessor.GetValue(stub));
        }

        [TestCategory("ProertyAccessor: tests")]
        [TestMethod]
        public void Test_ProertyAccessor_Emit_Boxing()
        {
            var stub = new StubClass2 { IntProperty = 1234 };

            IPropertyAccessor accessor = new EmitPropertyAccessor(typeof(StubClass2), "IntProperty");

            Assert.AreEqual(1234, (int)accessor.GetValue(stub));
        }

        [TestCategory("ProertyAccessor: tests")]
        [TestMethod]
        public void Test_ProertyAccessor_Emit_NullableInt()
        {
            var stub = new StubClass2 { NullableInt = 1234 };

            IPropertyAccessor accessor = new EmitPropertyAccessor(typeof(StubClass2), "NullableInt");

            Assert.AreEqual(1234, (int)accessor.GetValue(stub));
        }

        [TestCategory("ProertyAccessor: tests")]
        [TestMethod]
        public void Test_ProertyAccessor_Emit_Complex()
        {
            var stub = new StubClass2 { ComplexProperty = new StubClass1() { MyProperty1 = "123" } };

            IPropertyAccessor accessor = new EmitPropertyAccessor(typeof(StubClass2), "ComplexProperty");

            Assert.AreSame(stub.ComplexProperty, accessor.GetValue(stub));
        }

        [TestCategory("ProertyAccessor: tests")]
        [TestMethod]
        public void Test_ProertyAccessor_Emit_Sets()
        {
            var stub = new StubClass2 { StringProperty = null };

            IPropertyAccessor accessor = new EmitPropertyAccessor(typeof(StubClass2), "StringProperty");

            accessor.SetValue(stub, "test");

            Assert.AreEqual("test", stub.StringProperty);
        }
        #endregion

        #region Benchmarks
        const int LoopCount = 1000000;
        [TestCategory("ProertyAccessor: benchmarks")]
        [TestMethod]
        public void Test_ProertyAccessor_Fast_Gets_Benchmark()
        {
            var stub = new StubClass2 { StringProperty = "test" };

            IPropertyAccessor accessor = new FastPropertyAccessor(typeof(StubClass2), "StringProperty");

            DoGetStuff(stub, accessor);
        }

        [TestCategory("ProertyAccessor: benchmarks")]
        [TestMethod]
        public void Test_ProertyAccessor_Fast_Sets_Benchmark()
        {
            var stub = new StubClass2 { };

            IPropertyAccessor accessor = new FastPropertyAccessor(typeof(StubClass2), "StringProperty");

            DoSetStuff(stub, "test value to set", accessor);
        }

        [TestCategory("ProertyAccessor: benchmarks")]
        [TestMethod]
        public void Test_ProertyAccessor_Emit_Gets_Benchmark()
        {
            var stub = new StubClass2 { StringProperty = "test" };

            IPropertyAccessor accessor = new EmitPropertyAccessor(typeof(StubClass2), "StringProperty");

            DoGetStuff(stub, accessor);
        }

        [TestCategory("ProertyAccessor: benchmarks")]
        [TestMethod]
        public void Test_ProertyAccessor_Emit_Sets_Benchmark()
        {
            var stub = new StubClass2 { };

            IPropertyAccessor accessor = new EmitPropertyAccessor(typeof(StubClass2), "StringProperty");

            DoSetStuff(stub, "test value to set", accessor);
        }

        [TestCategory("ProertyAccessor: benchmarks")]
        [TestMethod]
        public void Test_ProertyAccessor_Direct_Gets_Benchmark()
        {
            var stub = new StubClass2 { StringProperty = "test" };

            DirectGetStuff(stub);
        }

        [TestCategory("ProertyAccessor: benchmarks")]
        [TestMethod]
        public void Test_ProertyAccessor_Direct_Sets_Benchmark()
        {
            var stub = new StubClass2 { StringProperty = "test" };

            DirectSetStuff(stub);
        }

        [TestCategory("ProertyAccessor: benchmarks")]
        [TestMethod]
        public void Test_ProertyAccessor_Reflection_Gets_Benchmark()
        {
            var stub = new StubClass2 { StringProperty = "test" };

            IPropertyAccessor accessor = new ReflectedPropertyAccessor(typeof(StubClass2), "StringProperty");

            DoGetStuff(stub, accessor);
        }

        [TestCategory("ProertyAccessor: benchmarks")]
        [TestMethod]
        public void Test_ProertyAccessor_Reflection_Sets_Benchmark()
        {
            var stub = new StubClass2 { };

            IPropertyAccessor accessor = new ReflectedPropertyAccessor(typeof(StubClass2), "StringProperty");

            DoSetStuff(stub, "test value to set", accessor);
        }

        [TestCategory("ProertyAccessor: benchmarks")]
        [TestMethod]
        public void Test_ProertyAccessor_All_Gets_Benchmark()
        {
            Console.WriteLine("Gets with {0} loops", LoopCount);
            BaseLine(null, null);
            DirectGetStuff(new StubClass2 { StringProperty = "test" });
            DoGetStuff(new StubClass2 { StringProperty = "test" }, new EmitPropertyAccessor(typeof(StubClass2), "StringProperty"));
            DoGetStuff(new StubClass2 { StringProperty = "test" }, new FastPropertyAccessor(typeof(StubClass2), "StringProperty"));
            DoGetStuff(new StubClass2 { StringProperty = "test" }, new ReflectedPropertyAccessor(typeof(StubClass2), "StringProperty"));
        }

        [TestCategory("ProertyAccessor: benchmarks")]
        [TestMethod]
        public void Test_ProertyAccessor_All_Sets_Benchmark()
        {
            Console.WriteLine("Sets with {0} loops", LoopCount);
            BaseLine(null, null);
            DirectSetStuff(new StubClass2 { });
            DoSetStuff(new StubClass2 { }, "test value to set", new EmitPropertyAccessor(typeof(StubClass2), "StringProperty"));
            DoSetStuff(new StubClass2 { }, "test value to set", new FastPropertyAccessor(typeof(StubClass2), "StringProperty"));
            DoSetStuff(new StubClass2 { }, "test value to set", new ReflectedPropertyAccessor(typeof(StubClass2), "StringProperty"));
        }
        #endregion

        #region Helpers
        private static void DoGetStuff(StubClass2 stub, IPropertyAccessor accessor)
        {
            var st = new Stopwatch();
            st.Start();
            for (int i = 0; i < LoopCount; i++)
            {
                var val = accessor.GetValue(stub);
            }
            st.Stop();
            Console.WriteLine(string.Format("Get {0}: {1}", accessor, st.ElapsedMilliseconds));
        }

        private static void DoSetStuff(StubClass2 stub, string value, IPropertyAccessor accessor)
        {
            var st = new Stopwatch();
            st.Start();
            for (int i = 0; i < LoopCount; i++)
            {
                accessor.SetValue(stub, value);
            }
            st.Stop();
            Console.WriteLine(string.Format("Set {0}: {1}", accessor, st.ElapsedMilliseconds));
        }

        private static void DirectGetStuff(StubClass2 stub)
        {
            var st = new Stopwatch();
            st.Start();
            for (int i = 0; i < LoopCount; i++)
            {
                var test = stub.StringProperty;
            }
            st.Stop();
            Console.WriteLine(string.Format("Get {0}: {1}", "Direct", st.ElapsedMilliseconds));
        }

        private static void DirectSetStuff(StubClass2 stub)
        {
            var st = new Stopwatch();
            st.Start();
            for (int i = 0; i < LoopCount; i++)
            {
                stub.StringProperty = "test";
            }
            st.Stop();
            Console.WriteLine(string.Format("Set {0}: {1}", "Direct", st.ElapsedMilliseconds));
        }

        private static void BaseLine(StubClass2 stub, IPropertyAccessor accessor)
        {
            var st = new Stopwatch();
            st.Start();
            for (int i = 0; i < LoopCount; i++)
            {
            }
            st.Stop();
            Console.WriteLine(string.Format("{0}: {1}", "Baseline (empty loop)", st.ElapsedMilliseconds));
        } 
        #endregion
    }
}
