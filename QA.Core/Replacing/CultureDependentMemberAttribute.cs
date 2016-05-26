using System;

namespace QA.Core.Replacing
{
    /// <summary>
    /// Атрибут, указывающий на то, что свойство объекта следует обрабатывать процессором
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class CultureDependentMemberAttribute : Attribute
    {      
    }
}
