using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
}