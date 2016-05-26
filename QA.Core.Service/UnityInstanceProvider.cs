// Owners: Alexey Abretov, Nikolay Karlov

using System;
using Microsoft.Practices.Unity;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace QA.Core.Service
{
    /// <summary>
    /// Класс, отвечающий за инстанцирование Wcf-сервиса
    /// </summary>
    internal class UnityInstanceProvider : IInstanceProvider
    {
        private readonly IUnityContainer container;
        private readonly Type contractType;

        public UnityInstanceProvider(IUnityContainer container, Type contractType)
        {
            this.container = container;
            this.contractType = contractType;
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return container.Resolve(contractType);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            container.Teardown(instance);
        }
    }
}
