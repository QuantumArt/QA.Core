using System;
using System.Linq;
using System.Linq.Expressions;

namespace QA.Core
{
    /// <summary>
    /// Быстрый доступ к свойствам объекта.
    /// Созданный экземпляр этого класса следует кешировать.
    /// </summary>
    public class FastPropertyAccessor<T>: IPropertyAccessor<T>
    {
        private Type _objectType;
        private string _propertyName;
        private Func<T, object> _getter;
        private Action<T, object> _setter;

        /// <summary>
        /// При вызове конструктора происходит генерация выражения для быстрого доступа к свойству
        /// </summary>
        /// <param name="objectType"></param>
        /// <param name="propertyName"></param>
        public FastPropertyAccessor(string propertyName)
        {
            Throws.IfArgumentNullOrEmpty(propertyName, _ => propertyName);

            _objectType = typeof(T);
            _propertyName = propertyName;

            var targetExpression = Expression.Parameter(_objectType, propertyName);
            var valueExpression = Expression.Parameter(typeof(object), "value");

            var mi = _objectType.GetProperty(propertyName).GetSetMethod();
            var parameterType = mi.GetParameters().Select(x => x.ParameterType).FirstOrDefault();

            // Создаем лямбда-выражение:
            // "obj => { ((ObjectType)obj).set_PropertyName((PropertyType)value) }"
            var setterExpression = Expression.Lambda<Action<T, object>>(
                  Expression.Call(
                     targetExpression, 
                      mi, // сеттер
                      Expression.Convert(valueExpression, parameterType) // приводим object к типу поля
                  ),
                  targetExpression,
                  valueExpression
            );

            _setter = setterExpression.Compile();

            ParameterExpression obj = Expression.Parameter(_objectType, "obj");

            // Создаем лямбда-выражение:
            // "obj => (object)((ObjectType)obj).get_PropertyName()"
            var getterExpression = Expression.Lambda<Func<T, object>>(
                Expression.Convert(
                    Expression.Call(
                        obj,
                        _objectType.GetProperty(propertyName).GetGetMethod()),
                    typeof(object)),
                obj);

            _getter = getterExpression.Compile();
        }

        /// <summary>
        /// Получение значения свойства объекта
        /// </summary>
        /// <param name="obj">Объект, к свойству которого предоставляется доступ</param>
        /// <returns></returns>
        public object GetValue(T obj)
        {
            // из сображений производительности никаких проверок на корректность не производится
            return _getter(obj);
        }

        /// <summary>
        /// Устаногвка значения свойства объекта
        /// </summary>
        /// <param name="obj">Объект, к свойству которого предоставляется доступ</param>
        /// <param name="value">Устанавливаемое значение</param>
        public void SetValue(T obj, object value)
        {
            // из сображений производительности никаких проверок на корректность не производится
            _setter(obj, value);
        }
    }
}
