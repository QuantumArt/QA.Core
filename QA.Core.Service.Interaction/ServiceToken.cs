using System;
using System.Linq;
using System.Runtime.Serialization;

namespace QA.Core.Service.Interaction
{
    /// <summary>
    /// Данные передаваемые в сервис вне методов
    /// </summary>
    [DataContract]
    public class ServiceToken
    {
        /// <summary>
        /// Имя контейнера IoC
        /// </summary>
        [DataMember]
        public string DependencyContainerName { get; set; }

        /// <summary>
        /// Идентификатор сайта
        /// </summary>
        [DataMember]
        public int SiteId { get; set; }

        /// <summary>
        /// Название подключения к БД
        /// </summary>
        [DataMember]
        public string ConnectionName { get; set; }

        /// <summary>
        /// Пространство имен для хранения данных
        /// </summary>
        [DataMember]
        public const string ServiceTokenNs = "CustomHeader";

        /// <summary>
        /// Ключ для хранения данных
        /// </summary>
        [DataMember]
        public const string ServiceTokenKey = "ServiceTokenKey";
    }
}
