// Owners: Alexey Abretov, Nikolay Karlov

using System;
using System.ServiceModel;
using Unity;

namespace QA.Core.Service
{
    /// <summary>
    /// Хост сервисов, использующий IoC и DI
    /// </summary>
    public class UnityServiceHost : ServiceHost
    {
        private IUnityContainer unityContainer;

        public UnityServiceHost(IUnityContainer unityContainer, Type serviceType)
            : base(serviceType)
        {
            this.unityContainer = unityContainer;
        }

        protected override void OnOpening()
        {
            base.OnOpening();

            if (this.Description.Behaviors.Find<UnityServiceBehavior>() == null)
            {
                this.Description.Behaviors.Add(new UnityServiceBehavior(this.unityContainer));
            }
        }
    }
}
