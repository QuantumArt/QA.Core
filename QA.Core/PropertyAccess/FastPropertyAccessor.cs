using System;
using System.Linq;
using System.Linq.Expressions;
#pragma warning disable 1591

namespace QA.Core
{
    /// <summary>
    /// Быстрый доступ к свойствам объекта.
    /// Созданный экземпляр этого класса следует кешировать.
    /// </summary>
    public class FastPropertyAccessor : IPropertyAccessor
    {
        private Type _objectType;
        private string _propertyName;
        private Func<object, object> _getter;
        private Action<object, object> _setter;

        public FastPropertyAccessor(Type objectType, string propertyName, bool isReadonly)
        {
            Throws.IfArgumentNull(objectType, _ => objectType);
            Throws.IfArgumentNullOrEmpty(propertyName, _ => propertyName);

            _objectType = objectType;
            _propertyName = propertyName;

            var targetExpression = Expression.Parameter(typeof(object), propertyName);
            var valueExpression = Expression.Parameter(typeof(object), "value");

            if (!isReadonly)
            {
                _setter = CompileSetter(propertyName, targetExpression, valueExpression);
            }

            ParameterExpression obj = Expression.Parameter(typeof(object), "obj");

            // Создаем лямбда-выражение:
            // "obj => (object)((ObjectType)obj).get_PropertyName()"
            var getterExpression = Expression.Lambda<Func<object, object>>(
                Expression.Convert(
                    Expression.Call(
                        Expression.Convert(obj, _objectType),
                        _objectType.GetProperty(propertyName).GetGetMethod()),
                    typeof(object)),
                obj);

            _getter = getterExpression.Compile();
        }

        private Action<object, object> CompileSetter(string propertyName, ParameterExpression targetExpression, ParameterExpression valueExpression)
        {
                var mi = _objectType.GetProperty(propertyName).GetSetMethod();
                var parameterType = mi.GetParameters().Select(x => x.ParameterType).FirstOrDefault();

                // Создаем лямбда-выражение:
                // "obj => { ((ObjectType)obj).set_PropertyName((PropertyType)value) }"
                var setterExpression = Expression.Lambda<Action<object, object>>(
                      Expression.Call(
                          Expression.Convert(targetExpression, _objectType), // приводим object к типу объекта
                          mi, // сеттер
                          Expression.Convert(valueExpression, parameterType) // приводим object к типу поля
                      ),
                      targetExpression,
                      valueExpression
                );

            return setterExpression.Compile();
        }

        /// <summary>
        /// При вызове конструктора происходит генерация выражения для быстрого доступа к свойству
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="propertyName"></param>
        public FastPropertyAccessor(Type objectType, string propertyName)
            : this(objectType, propertyName, false)
        {

        }

        /// <summary>
        /// Получение значения свойства объекта
        /// </summary>
        /// <param name="obj">Объект, к свойству которого предоставляется доступ</param>
        /// <returns></returns>
        public object GetValue(object obj)
        {
            // из сображений производительности никаких проверок на корректность не производится
            return _getter(obj);
        }

        /// <summary>
        /// Устаногвка значения свойства объекта
        /// </summary>
        /// <param name="obj">Объект, к свойству которого предоставляется доступ</param>
        /// <param name="value">Устанавливаемое значение</param>
        public void SetValue(object obj, object value)
        {
            // из сображений производительности никаких проверок на корректность не производится
            _setter(obj, value);
        }
    }
}
