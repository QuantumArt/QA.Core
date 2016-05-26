using System;
using System.ComponentModel;
using System.Linq;

namespace QA.Core.PropertyAccess
{
    /// <summary>
    /// Медленный. Не использовать
    /// </summary>
    public class TypeDescriptorPropertyAccessor : IPropertyAccessor
    {
        private readonly string _propertyName;
        private readonly Type _objectType;
        private PropertyDescriptor _propertyDescriptor;

        public TypeDescriptorPropertyAccessor(Type objectType, string propertyName)
        {
            Throws.IfArgumentNull(objectType, _ => objectType);
            Throws.IfArgumentNullOrEmpty(propertyName, _ => propertyName);

            _objectType = objectType;
            _propertyName = propertyName;

            _propertyDescriptor = TypeDescriptor
                .GetProperties(_objectType)
                .OfType<PropertyDescriptor>()
                .FirstOrDefault(x => x.Name == _propertyName);
        }

        public object GetValue(object obj)
        {
            return _propertyDescriptor.GetValue(obj);
        }

        public void SetValue(object obj, object value)
        {
            _propertyDescriptor.SetValue(obj, value);
        }
    }
}
