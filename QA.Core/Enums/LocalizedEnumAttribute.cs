// Owners: Alexey Abretov, Nikolay Karlov

using System;
using System.ComponentModel;
using System.Reflection;
#pragma warning disable 1591

namespace QA.Core
{
    /// <summary>
    /// Реализует атрибут для локализации элементов перечислений
    /// </summary>
    public class LocalizedEnumAttribute : DescriptionAttribute
    {
        private PropertyInfo _nameProperty;
        private Type _resourceType;

        public LocalizedEnumAttribute(string displayNameKey) : base(displayNameKey) { }

        /// <summary>
        /// Тип ресурса
        /// </summary>
        public Type NameResourceType
        {
            get
            {
                return _resourceType;
            }
            set
            {
                _resourceType = value;
                _nameProperty = _resourceType.GetProperty(
                    this.Description, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            }
        }

        /// <summary>
        /// Описание
        /// </summary>
        public override string Description
        {
            get
            {
                //check if nameProperty is null and return original display name value
                if (_nameProperty == null)
                {
                    return base.Description;
                }

                return (string)_nameProperty.GetValue(_nameProperty.DeclaringType, null);
            }
        }
    }
}
