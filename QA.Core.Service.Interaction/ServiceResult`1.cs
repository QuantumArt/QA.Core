// Owners: Alexey Abretov, Nikolay Karlov

using System;
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
	    /// <summary>
        /// Возвращаемый объект
        /// </summary>
        [DataMember]
        public TResult Result { get; set; }

		/// <summary>
		/// Возвращает Result при условии что IsSucceeded==true и он не null
		/// в противном случае бросает эксепшн
		/// </summary>
	    [IgnoreWhileDumping]
		public TResult ResultEnsured
	    {
		    get
		    {
			    if (!IsSucceeded)
				    throw new OperationExecutionException(Error);

			    if (Result == null)
				    throw new Exception("Result is null");

			    return Result;
		    }
	    }
    }
}
