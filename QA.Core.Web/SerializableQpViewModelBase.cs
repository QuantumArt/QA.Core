// Owners: Alexey Abretov, Nikolay Karlov

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace QA.Core.Web
{
    /// <summary>
    /// Базовый класс модели представления при использовании Qp
    /// </summary>
    [DataContract]
    public class SerializableQpViewModelBase
    {
        /// <summary>
        /// Идентификатор хоста
        /// </summary>
        [DataMember]
        public string HostId { get; set; }

        /// <summary>
        /// Идентификатор бэкенда
        /// </summary>
        [DataMember]
        public string BackendSid { get; set; }

        /// <summary>
        /// Код заказчика
        /// </summary>
        [DataMember]
        public string CustomerCode { get; set; }

        /// <summary>
        /// Ключ
        /// </summary>
        [IgnoreDataMember]
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
        [DataMember]
        public string SiteId { get; set; }

        /// <summary>
        /// Сериализация JSON в объект
        /// </summary>
        /// <param name="json">Строка для десериализации</param>
        public static SerializableQpViewModelBase FromJsonString(string json)
        {
            var serializer = new DataContractJsonSerializer(typeof(SerializableQpViewModelBase));

            using (var stream = new MemoryStream())
            {
                using (var sw = new StreamWriter(stream))
                {
                    sw.Write(json);
                    sw.Flush();
                    return (SerializableQpViewModelBase)serializer.ReadObject(stream);
                }
            }
        }

        /// <summary>
        /// Сериализация потока в объект
        /// </summary>
        /// <param name="stream">Поток для десериализации</param>
        public static SerializableQpViewModelBase FromJsonString(Stream stream)
        {
            var sr = new StreamReader(stream);
            stream.Position = 0;
            string data = sr.ReadToEnd();

            stream.Position = 0;

            using (var strm = new MemoryStream())
            {
                using (var sw = new StreamWriter(strm))
                {
                    var serializer = new DataContractJsonSerializer(typeof(SerializableQpViewModelBase));
                    sw.Write(data);
                    sw.Flush();
                    strm.Position = 0;
                    return (SerializableQpViewModelBase)serializer.ReadObject(strm);
                }
            }
        }
    }
}
