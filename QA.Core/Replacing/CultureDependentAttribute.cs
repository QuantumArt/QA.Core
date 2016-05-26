using System;

namespace QA.Core.Replacing
{
    /// <summary>
    /// Атрибут, указывающий на то, что значение этого поля может зависеть от установленного языка
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class CultureDependentAttribute : Attribute
    {      
    }
}
