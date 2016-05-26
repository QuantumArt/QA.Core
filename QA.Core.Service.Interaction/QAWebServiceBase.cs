// Owners: Alexey Abretov, Nikolay Karlov
using System;

namespace QA.Core.Service.Interaction
{
    /// <summary>
    /// Базовый класс сервиса
    /// </summary>
    public abstract class QAWebServiceBase : QAServiceBase
    {
        /// <summary>
        /// Данные передаваемые в сервис
        /// </summary>
        public WebServiceToken ServiceToken { get; set; }

        /// <summary>
        /// Текущие данные с клиента сервису
        /// </summary>
        protected override ServiceToken CurrentServiceToken
        {
            get
            {
                ServiceToken token = base.CurrentServiceToken;

                if (token == null & ServiceToken != null)
                {
                    return new ServiceToken
                    {
                        ConnectionName = ServiceToken.ConnectionName,
                        DependencyContainerName = ServiceToken.DependencyContainerName,
                        SiteId = ServiceToken.SiteId
                    };
                }

                return token;
            }
        }
    }
}
