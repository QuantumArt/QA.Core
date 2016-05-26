using System;
namespace QA.Core
{
    /// <summary>
    /// Интерфейс для классов, предоставляющих доступ к свойствам объектов.
    /// Экземпляры таких классов надо кешировать.
    /// </summary>
    public interface IPropertyAccessor<T> : IPropertyAccessor<T, object>
    {

    }
}
