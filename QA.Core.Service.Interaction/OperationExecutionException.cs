// Owners: Karlov Nikolay, Abretov Alexey
using System;
using System.Runtime.Serialization;

namespace QA.Core.Service.Interaction
{
    /// <summary>
    /// Исключение с кодом ошибки
    /// </summary>
    [Serializable]
    [DataContract]
    public class OperationExecutionException : Exception
    {
        /// <summary>
        /// Код ошибки
        /// </summary>
        [DataMember]
        public int ErrorCode
        {
            get;
            set;
        }

        public OperationExecutionException(int errorCode, string message)
            : this(errorCode, message, null)
        {
        }

        public OperationExecutionException(int errorCode, string message, Exception innerException)
            : base(message, innerException)
        {

            ErrorCode = errorCode;
        }

        public OperationExecutionException(ServiceError error)
            : this(error, null)
        {
        }

        public OperationExecutionException(ServiceError error, Exception innerException)
            : base(error.Message, innerException)
        {
            ErrorCode = error.ErrorCode;
        }

        public static void Throw(int errorCode, string message) 
        {
            throw new OperationExecutionException(errorCode, message);
        }

        public static void Throw(ServiceError error)
        {
            throw new OperationExecutionException(error);
        }
    }
}
