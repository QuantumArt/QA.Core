using System;
using System.Linq;

namespace QA.Core.Service.Interaction
{
    /// <summary>
    /// Контракт результатов рабоыт сервиса
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IServiceResult<out T>
    {
        /// <summary>
        /// Ошибка
        /// </summary>
        ServiceError Error
        {
            get;
        }
        
        /// <summary>
        /// Признак успешного завершения
        /// </summary>
        bool IsSucceeded
        {
            get;
        }

        /// <summary>
        /// Возвращаемые данные
        /// </summary>
        T Result
        {
            get;
        }
    }
}
