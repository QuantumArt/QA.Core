using System;
namespace QA.Core
{
    /// <summary>
    /// Интерфейс для классов, предоставляющих доступ к свойствам объектов.
    /// Экземпляры таких классов надо кешировать.
    /// </summary>
    public interface IPropertyAccessor : IPropertyAccessor<object>
    {
        /// <summary>
        /// Получение значения свойства объекта
        /// </summary>
        /// <param name="obj">Объект, к свойству которого предоставляется доступ</param>
        /// <returns></returns>
        object GetValue(object obj);

        /// <summary>
        /// Устаногвка значения свойства объекта
        /// </summary>
        /// <param name="obj">Объект, к свойству которого предоставляется доступ</param>
        /// <param name="value">Устанавливаемое значение</param>
        void SetValue(object obj, object value);
    }
}
