using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QA.Core.Tests.Stubs;

namespace QA.Core.Tests
{
    [TestClass()]
    public class ThrowsTest
    {

        #region ArgumentNull
        #region Stub methods
        private void TestMethod1(string arg1)
        {
            Throws.IfArgumentNull(x => arg1);
        }

        private void TestMethod2(StubClass1 arg1)
        {
            Throws.IfArgumentNull(x => arg1);
            Throws.IfArgumentNull(x => arg1.MyProperty1);
            Throws.IfArgumentNull(x => arg1.MyProperty2);
            Throws.IfArgumentNull(x => arg1.Child);
        }

        private void TestMethod3(StubClass1 arg1)
        {
            Throws.IfArgumentNull(arg1, "arg1");
            Throws.IfArgumentNull(arg1.MyProperty1, "arg1.MyProperty1");
            Throws.IfArgumentNull(arg1.MyProperty2, "arg1.MyProperty2");
            Throws.IfArgumentNull(arg1.Child, "arg1.Child");
        }

        private void TestMethod4(StubClass1 arg1)
        {
            Throws.IfArgumentNull(arg1, x => arg1);
            Throws.IfArgumentNull(arg1.MyProperty1, x => arg1.MyProperty1);
            Throws.IfArgumentNull(arg1.MyProperty2, x => arg1.MyProperty2);
            Throws.IfArgumentNull(arg1.Child, x => arg1.Child);
        }

        private void TestMethod5(StubClass1 arg1)
        {
            Throws.IfArgumentNull(x => arg1);
        }

        private void TestMethod6(StubClass1 arg1)
        {
            Throws.IfArgumentNull(arg1, x => arg1);
        }

        private void TestMethod7(StubClass1 arg1)
        {
            Throws.IfArgumentNull(arg1, "arg1");
        }
        #endregion


        /// <summary>
        ///A test for IfArgumentNull
        ///</summary>
        [TestMethod()]
        public void Test_IfArgumentNull_Check_Null_String()
        {
            Exception exception = null;
            try
            {
                TestMethod1(null);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(ArgumentNullException));
            Assert.AreEqual(((ArgumentNullException)exception).ParamName, "arg1");
        }

        /// <summary>
        ///A test for IfArgumentNull
        ///</summary>
        [TestMethod()]
        public void Test_IfArgumentNull_Check_Custom_Type()
        {
            Exception exception = null;
            try
            {
                TestMethod2(null);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(ArgumentNullException));
            Assert.AreEqual(((ArgumentNullException)exception).ParamName, "arg1");
        }

        /// <summary>
        ///A test for IfArgumentNull
        ///</summary>
        [TestMethod()]
        public void Test_IfArgumentNull_Check_String_Property_Of_Custom_Type()
        {
            Exception exception = null;
            try
            {
                TestMethod2(new StubClass1 { });
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(ArgumentNullException));
            Assert.AreEqual(((ArgumentNullException)exception).ParamName, "arg1.MyProperty1");
        }

        /// <summary>
        ///A test for IfArgumentNull
        ///</summary>
        [TestMethod()]
        public void Test_IfArgumentNull_Check_Int_Property_Of_Custom_Type()
        {
            Exception exception = null;
            try
            {
                TestMethod2(new StubClass1 { MyProperty1 = "test" });
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(ArgumentNullException));
            Assert.AreEqual(((ArgumentNullException)exception).ParamName, "arg1.MyProperty2");
        }

        /// <summary>
        ///A test for IfArgumentNull
        ///</summary>
        [TestMethod()]
        public void Test_IfArgumentNull_Check_Reference_Property_Of_Custom_Type()
        {
            Exception exception = null;
            try
            {
                TestMethod2(new StubClass1 { MyProperty1 = "test", MyProperty2 = 12 });
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNotNull(exception);
            Assert.IsInstanceOfType(exception, typeof(ArgumentNullException));
            Assert.AreEqual(((ArgumentNullException)exception).ParamName, "arg1.Child");
        }

        /// <summary>
        ///A test for IfArgumentNull
        ///</summary>
        [TestMethod()]
        public void Test_IfArgumentNull_Benchmark()
        {
            const int n = 1000;
            Stopwatch timer = new Stopwatch();

            Func<int, StubClass1> factory = i => new StubClass1 { MyProperty1 = i, MyProperty2 = 12 + i, Child = new StubClass1 { } };

            timer.Start();

            for (int i = 0; i < n; i++)
            {
                TestMethod4(factory(i));
            }

            double interval1 = timer.ElapsedTicks;
            timer.Reset();
            timer.Start();

            for (int i = 0; i < n; i++)
            {
                TestMethod3(factory(i));
            }

            double interval2 = timer.ElapsedTicks;
            timer.Stop();

            var rate = interval1 / interval2;

            //Assert.IsTrue(rate <= 500);
        }

        #endregion

        #region Conditional stub methods
        private void TestMethod1_Conditions(StubClass1 arg1, string msg)
        {
            Throws.IfNot(arg1.Child != null, msg);
        }


        #endregion

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_IfConditions_Work_Correctly()
        {
            try
            {
                var arg1 = new StubClass1();
                var msg = "msg1";

                Throws.IfNot(() => arg1.Child != null, x => arg1, msg);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message.StartsWith("msg1"));
                throw;
            }
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_IfConditions_Work_Correctly2()
        {
            try
            {
                var arg1 = new StubClass1();
                var msg = "msg1";

                Throws.IfNot(arg1.Child != null, x => arg1, msg);
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message.StartsWith("msg1"));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Test_IfConditions_With_Expressions_Work_Correctly()
        {
            try
            {
                var arg1 = new StubClass1();
                var msg = "msg1";

                Throws.IfNot(() => arg1.Child != null, msg);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.StartsWith("msg1"));
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Test_IfConditions_With_Expressions_Work_Correctly2()
        {
            try
            {
                var arg1 = new StubClass1();
                var msg = "msg1";

                Throws.IfNot(arg1.Child != null, msg);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.StartsWith("msg1"));
                throw;
            }
        }
    }
}
