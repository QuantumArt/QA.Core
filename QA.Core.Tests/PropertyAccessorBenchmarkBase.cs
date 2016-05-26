using Microsoft.VisualStudio.TestTools.UnitTesting;
using QA.Core.PropertyAccess;
using QA.Core.Tests.Stubs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA.Core.Tests
{
    [TestClass]
    public abstract class PropertyAccessorBenchmarkBase<TAccessor>
        where TAccessor : IPropertyAccessor
    {
        static int LoopCount = 10000;
        protected abstract IPropertyAccessor GetAccessor(Type type, string propertyName);


        [TestMethod]
        [TestCategory("ProertyAccessor: bench")]
        public void Test_StringPropertyGets()
        {
            var a = GetAccessor(typeof(StubClass1), "MyProperty3");

            Log("start " + LoopCount);
            var sw = new Stopwatch();
            var obj = new StubClass1() { MyProperty3 = "1234" };

            sw.Start();
            for (int i = 0; i < LoopCount; i++)
            {
                a.GetValue(obj);
            }
            sw.Stop();

            Log("Elapsed " + sw.ElapsedMilliseconds);
            
        }

        protected void Log(string message)
        {
            Console.WriteLine(string.Format("{0} {1}"), this.GetType(), message);

        }
    }


   
    public class FastPropertyAccessorBTests : PropertyAccessorBenchmarkBase<EmitPropertyAccessor>
    {
        protected override IPropertyAccessor GetAccessor(Type type, string propertyName)
        {
            return new FastPropertyAccessor(type, propertyName);
        }
    }

   
    public class TDPropertyAccessorBTests : PropertyAccessorBenchmarkBase<TypeDescriptorPropertyAccessor>
    {
        protected override IPropertyAccessor GetAccessor(Type type, string propertyName)
        {
            return new TypeDescriptorPropertyAccessor(type, propertyName);
        }
    }
}
