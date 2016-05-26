using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QA.Core.Tests
{   
    [TestClass]
    public class ReflectedPropertyAccessorTests : PropertyAccessorsTests<EmitPropertyAccessor>
    {
        protected override IPropertyAccessor GetAccessor(Type type, string propertyName)
        {
            return new ReflectedPropertyAccessor(type, propertyName);
        }
    }
}