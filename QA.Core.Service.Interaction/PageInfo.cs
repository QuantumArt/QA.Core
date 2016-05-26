// Owners: Alexey Abretov, Nikolay Karlov

using System.Runtime.Serialization;

namespace QA.Core.Service.Interaction
{
    /// <summary>
    /// Информация о текущей странице. 
    /// <remarks>используется при постраничномы выводе коллекций</remarks>
    /// </summary>
    [DataContract]
    public class PageInfo
    {
        /// <summary>
        /// Общее количество
        /// </summary>
        [DataMember]
        public int TotalCount { get; set; }

        /// <summary>
        /// Номер страницы
        /// </summary>
        [DataMember]
        public int PageNumber { get; set; }

        /// <summary>
        /// Размер страницы
        /// </summary>
        [DataMember]
        public int PageSize { get; set; }
    }
}
