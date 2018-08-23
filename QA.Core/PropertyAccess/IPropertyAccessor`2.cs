using System;
namespace QA.Core
{
    /// <summary>
    /// Интерфейс для классов, предоставляющих доступ к свойствам объектов.
    /// Экземпляры таких классов надо кешировать.
    /// <typeparam name="T">тип объекта</typeparam>
    /// <typeparam name="P">тип параметра</typeparam>
    /// </summary>
    public interface IPropertyAccessor<T, P>
    {
        /// <summary>
        /// Получение значения свойства объекта
        /// </summary>
        /// <param name="obj">Объект, к свойству которого предоставляется доступ</param>
        /// <returns></returns>
        P GetValue(T obj);

        /// <summary>
        /// Устаногвка значения свойства объекта
        /// </summary>
        /// <param name="obj">Объект, к свойству которого предоставляется доступ</param>
        /// <param name="value">Устанавливаемое значение</param>
        void SetValue(T obj, P value);
    }
}
