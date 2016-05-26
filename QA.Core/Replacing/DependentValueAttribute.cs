using System;

namespace QA.Core.Replacing
{
    /// <summary>
    /// Атрибут, указывающий на то, что значение этого поля может быть языковой версией жругого поля
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class DependentValueAttribute : Attribute
    {
        /// <summary>
        /// Имя поля, версией которого является данное поле
        /// </summary>
        public string TargetPropertyName { get; private set; }

        /// <summary>
        /// Язык версии
        /// </summary>
        public string CultureName { get; private set; }
        
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="targetPropertyName">Имя поля, версией которого является данное поле</param>
        /// <param name="cultureName">Язык версии</param>
        public DependentValueAttribute(string targetPropertyName, string cultureName)
        {
            TargetPropertyName = targetPropertyName;
            CultureName = cultureName;
        }
    }
}
