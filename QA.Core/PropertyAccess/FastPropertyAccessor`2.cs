using System;
using System.Linq;
using System.Linq.Expressions;

namespace QA.Core
{
    /// <summary>
    /// Быстрый доступ к свойствам объекта.
    /// Созданный экземпляр этого класса следует кешировать.
    /// </summary>
    public class FastPropertyAccessor<T, P> : IPropertyAccessor<T, P>
    {
        private readonly Type _objectType;
        private readonly string _propertyName;
        private Func<T, P> _getter;
        private Action<T, P> _setter;

        /// <summary>
        /// При вызове конструктора происходит генерация выражения для быстрого доступа к свойству
        /// </summary>
        /// <param name="propertyName"></param>
        public FastPropertyAccessor(string propertyName)
        {
            Throws.IfArgumentNullOrEmpty(propertyName, _ => propertyName);

            _objectType = typeof(T);
            _propertyName = propertyName;

            var targetExpression = Expression.Parameter(_objectType, propertyName);
            var valueExpression = Expression.Parameter(typeof(P), "value");

            var mi = _objectType.GetProperty(propertyName).GetSetMethod();
            var parameterType = mi.GetParameters().Select(x => x.ParameterType).FirstOrDefault();

            // Создаем лямбда-выражение:
            // "obj => { (obj).set_PropertyName(value) }"
            var setterExpression = Expression.Lambda<Action<T, P>>(
                  Expression.Call(
                     targetExpression,
                      mi, // сеттер
                      valueExpression // приводим object к типу поля
                  ),
                  targetExpression,
                  valueExpression
            );

            _setter = setterExpression.Compile();

            ParameterExpression obj = Expression.Parameter(_objectType, "obj");

            // Создаем лямбда-выражение:
            // "obj => (obj).get_PropertyName()"
            var getterExpression = Expression.Lambda<Func<T, P>>(
                Expression.Convert(
                    Expression.Call(
                        obj,
                        _objectType.GetProperty(propertyName).GetGetMethod()),
                    typeof(P)),
                obj);

            _getter = getterExpression.Compile();
        }

        /// <summary>
        /// Получение значения свойства объекта
        /// </summary>
        /// <param name="obj">Объект, к свойству которого предоставляется доступ</param>
        /// <returns></returns>
        public P GetValue(T obj)
        {
            // из сображений производительности никаких проверок на корректность не производится
            return _getter(obj);
        }

        /// <summary>
        /// Устаногвка значения свойства объекта
        /// </summary>
        /// <param name="obj">Объект, к свойству которого предоставляется доступ</param>
        /// <param name="value">Устанавливаемое значение</param>
        public void SetValue(T obj, P value)
        {
            // из сображений производительности никаких проверок на корректность не производится
            _setter(obj, value);
        }
    }
}
