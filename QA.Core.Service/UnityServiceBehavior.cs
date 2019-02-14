#if !NETSTANDARD
// Owners: Alexey Abretov, Nikolay Karlov

using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Unity;
#pragma warning disable 1591

namespace QA.Core.Service
{
    /// <summary>
    /// Класс, отвечающий за инстанцирование Wcf-сервиса
    /// </summary>
    public class UnityServiceBehavior : IServiceBehavior
    {
        private readonly IUnityContainer container;

        public UnityServiceBehavior(IUnityContainer container)
        {
            this.container = container;
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase)
        {
            Throws.IfArgumentNull(serviceDescription, _ => serviceDescription);
            Throws.IfArgumentNull(serviceDescription.Endpoints, _ => serviceDescription.Endpoints);
            Throws.IfArgumentNull(serviceDescription.ServiceType, _ => serviceDescription.ServiceType);
            Throws.IfArgumentNull(serviceHostBase, _ => serviceHostBase);

            foreach (ChannelDispatcher channelDispatcher in serviceHostBase.ChannelDispatchers)
            {
                foreach (EndpointDispatcher endpointDispatcher in channelDispatcher.Endpoints)
                {
                    if (endpointDispatcher.ContractName != "IMetadataExchange")
                    {
                        string contractName = endpointDispatcher.ContractName;
                        ServiceEndpoint serviceEndpoint = serviceDescription.Endpoints.FirstOrDefault(
                            e => e.Contract.Name == contractName);

                        endpointDispatcher.DispatchRuntime.InstanceProvider = new UnityInstanceProvider(
                            this.container, serviceEndpoint.Contract.ContractType);
                    }
                }
            }
        }
    }
}
#endif
