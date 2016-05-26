using System;
using System.Linq;
using System.Web.Services.Protocols;

namespace QA.Core.Service.Interaction
{
    /// <summary>
    /// Данные передаваемые в сервис вне методов
    /// </summary>
    public class WebServiceToken : SoapHeader
    {
        /// <summary>
        /// Имя контейнера IoC
        /// </summary>
        public string DependencyContainerName { get; set; }

        /// <summary>
        /// Идентификатор сайта
        /// </summary>
        public int SiteId { get; set; }

        /// <summary>
        /// Название подключения к БД
        /// </summary>
        public string ConnectionName { get; set; }

        /// <summary>
        /// Пространство имен для хранения данных
        /// </summary>
        public const string ServiceTokenNs = "CustomHeader";

        /// <summary>
        /// Ключ для хранения данных
        /// </summary>
        public const string ServiceTokenKey = "ServiceTokenKey";
    }
}
