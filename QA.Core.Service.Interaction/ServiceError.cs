// Owners: Alexey Abretov, Nikolay Karlov

using System.Runtime.Serialization;

namespace QA.Core.Service.Interaction
{
    /// <summary>
    /// Класс, описывающий ошибка выполнения операции.
    /// </summary>
    [DataContract]
    public class ServiceError
    {
        /// <summary>
        /// Код ошибки
        /// </summary>
        [DataMember]
        public int ErrorCode { get; set; }

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Тип ошибки
        /// </summary>
        [DataMember]
        public ServiceErrorType Type { get; set; }

        /// <summary>
        /// Конструирует пустое исключение
        /// </summary>
        public ServiceError()
        {
            Type = ServiceErrorType.Exception;
        }

        /// <summary>
        /// Конструирует исключение
        /// <param name="code">Код исключения</param>
        /// <param name="message">Текст исключения</param>
        /// </summary>
        public ServiceError(int code, string message)
            : this()
        {
            ErrorCode = code;
            Message = message;

            Type = ServiceErrorType.Exception;
        }

        /// <summary>
        /// Конструирует ошибку
        /// <param name="code">Код ошибки</param>
        /// <param name="message">Текст ошибки</param>
        /// <param name="type">Тип ошибки</param>
        /// </summary>
        public ServiceError(int code, string message, ServiceErrorType type)
            : this(code, message)
        {
            Type = type;
        }
    }
}
