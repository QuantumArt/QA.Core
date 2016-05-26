// Owners: Karlov Nikolay, Abretov Alexey
using System;
using System.Runtime.Serialization;

namespace QA.Core.Service.Interaction
{
    /// <summary>
    /// Абстрактный базовый класс, описывающий результат выполнения операции.
    /// <remarks>При сериализации будет представлен классом ServiceResult<object></remarks>
    /// </summary>
    [DataContract]
    [KnownType("GetKnownTypes")]
    public abstract class ServiceResult
    {
        /// <summary>
        /// Информация об ошибке. Null в случае успешного выполнения
        /// </summary>
        [DataMember]
        public ServiceError Error { get; set; }

        /// <summary>
        /// True в случае успешного завершения
        /// </summary>
        [DataMember]
        public bool IsSucceeded { get; set; }

        /// <summary>
        /// Создает экземпляр
        /// </summary>
        public ServiceResult()
        {
        }

        /// <summary>
        /// Позволяет корректно сериализовать этот тип
        /// </summary>
        public static Type[] GetKnownTypes()
        {
            return new Type[] { typeof(ServiceResult<object>), typeof(ServiceEnumerationResult<object>) };
        }
    }
}
