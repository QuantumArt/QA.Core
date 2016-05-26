// Owners: Karlov Nikolay, Abretov Alexey
using System.Runtime.Serialization;

namespace QA.Core.Service.Interaction
{
    /// <summary>
    /// Контекст пользователя
    /// <remarks>передается при каждом обращении к wcf-сервису</remarks>
    /// </summary>
    [DataContract]
    public class UserContext
    {
        /// <summary>
        /// Логин пользователя (номер телефона)
        /// Обязательно передавать с клиента при каждом запросе
        /// </summary>
        [DataMember]
        public string UserLogin { get; set; }

        /// <summary>
        /// MessageBoxId (Optional)
        /// Заполняется на стороне сервиса при авторизации. С клиента передавать не обязательно
        /// </summary>
        [DataMember]
        public int UserId { get; set; }
    }
}
