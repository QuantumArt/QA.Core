// Owners: Alexey Abretov, Nikolay Karlov

using System.Runtime.Serialization;

namespace QA.Core.Service.Interaction
{
    /// <summary>
    /// Класс, описывающий ответ от сервиса
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    [DataContract]
    public class ServiceResult<TResult> : ServiceResult, IServiceResult<TResult>
    {
        private TResult _result;

        /// <summary>
        /// Возвращаемый объект
        /// </summary>
        [DataMember]
        public TResult Result
        {
            get { return _result; }
            set { _result = value; }
        }
    }
}
