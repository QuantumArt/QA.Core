using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QA.Core.PropertyAccess;

namespace QA.Core.Tests
{
    [TestClass]
    public class FastPropertyAccessorTests : PropertyAccessorsTests<EmitPropertyAccessor>
    {
        protected override IPropertyAccessor GetAccessor(Type type, string propertyName)
        {
            return new FastPropertyAccessor(type, propertyName);
        }
    }

    [TestClass]
    public class TDPropertyAccessorTests : PropertyAccessorsTests<TypeDescriptorPropertyAccessor>
    {
        protected override IPropertyAccessor GetAccessor(Type type, string propertyName)
        {
            return new TypeDescriptorPropertyAccessor(type, propertyName);
        }
    }
}