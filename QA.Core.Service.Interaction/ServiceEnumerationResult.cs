// Owners: Alexey Abretov, Nikolay Karlov

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace QA.Core.Service.Interaction
{
    /// <summary>
    /// Класс для возврата перечислений с информацией о постраничном выводе
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    [DataContract]
    public class ServiceEnumerationResult<TResult> : ServiceResult<List<TResult>>
    {
        /// <summary>
        /// Информация о постраничном выводе
        /// </summary>
        [DataMember]
        public PageInfo PageInfo { get; set; }

        /// <summary>
        /// Сигнализирует об использовании пагинации
        /// Если false, то свойство PageInfo может быть null
        /// иначе PageInfo не null
        /// </summary>
        [DataMember]
        public bool IsPagingUsed { get; set; }
    }
}
