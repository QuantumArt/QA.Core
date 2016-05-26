using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QA.Core.Tests
{
    [TestClass]
    public class EmitPropertyAccessorTests : PropertyAccessorsTests<EmitPropertyAccessor>
    {
        protected override IPropertyAccessor GetAccessor(Type type, string propertyName)
        {
            return new EmitPropertyAccessor(type, propertyName);
        }
    }
}