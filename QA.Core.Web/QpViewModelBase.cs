// Owners: Alexey Abretov, Nikolay Karlov

using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace QA.Core.Web
{
    /// <summary>
    /// Базовый класс модели представления при использовании Qp
    /// </summary>
    [Serializable]
    public class QpViewModelBase : ViewModelBase
    {
        /// <summary>
        /// Идентификатор хоста
        /// </summary>
        public string HostId { get; set; }

        /// <summary>
        /// Идентификатор бэкенда
        /// </summary>
        public string BackendSid { get; set; }

        /// <summary>
        /// Код заказчика
        /// </summary>
        public string CustomerCode { get; set; }

        /// <summary>
        /// Ключ
        /// </summary>
        public string QpKey
        {
            get
            {
                return (CustomerCode ?? string.Empty) + "_" + (SiteId ?? string.Empty);
            }
        }

        /// <summary>
        /// Идентификатор сайта
        /// </summary>
        public string SiteId { get; set; }

        public T Copy<T>(T instance) where T : QpViewModelBase
        {
            if (instance == null)
            {
                instance = default(T);
            }

            instance.SiteId = this.SiteId;
            instance.HostId = this.HostId;
            instance.CustomerCode = this.CustomerCode;
            instance.BackendSid = this.BackendSid;

            return instance;
        }

        public QpViewModelBase Clone()
        {
            var instance = new QpViewModelBase();

            instance.SiteId = this.SiteId;
            instance.HostId = this.HostId;
            instance.CustomerCode = this.CustomerCode;
            instance.BackendSid = this.BackendSid;

            return instance;
        }
    }
}
