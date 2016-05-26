using System;
using System.Linq;
using System.Linq.Expressions;

namespace QA.Core
{
    /// <summary>
    /// Доступ к свойствам объекта. Используется Reflection, скорость выполнения достаточно низкая
    /// Созданный экземпляр этого класса следует кешировать.
    /// </summary>
    public class ReflectedPropertyAccessor : IPropertyAccessor
    {
        private readonly System.Reflection.PropertyInfo _propertyInfo;

        /// <summary>
        /// При вызове конструктора происходит генерация выражения для доступа к свойству
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="propertyName"></param>
        public ReflectedPropertyAccessor(Type objectType, string propertyName)
        {
            Throws.IfArgumentNull(objectType, _ => objectType);
            Throws.IfArgumentNullOrEmpty(propertyName, _ => propertyName);

            _propertyInfo = objectType.GetProperty(propertyName);

            Throws.IfArgumentNull(_propertyInfo, _ => _propertyInfo);
        }

        /// <summary>
        /// Получение значения свойства объекта
        /// </summary>
        /// <param name="obj">Объект, к свойству которого предоставляется доступ</param>
        /// <returns></returns>
        public object GetValue(object obj)
        {
            // из сображений производительности никаких проверок на корректность не производится
            return _propertyInfo.GetValue(obj, new object[] { });
        }

        /// <summary>
        /// Устаногвка значения свойства объекта
        /// </summary>
        /// <param name="obj">Объект, к свойству которого предоставляется доступ</param>
        /// <param name="value">Устанавливаемое значение</param>
        public void SetValue(object obj, object value)
        {
            // из сображений производительности никаких проверок на корректность не производится
            _propertyInfo.SetValue(obj, value);
        }
    }
}
