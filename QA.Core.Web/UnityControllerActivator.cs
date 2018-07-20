using System;
using System.Web.Mvc;
using System.Web.Routing;
using Unity;

namespace QA.Core.Web
{
    /// <summary>
    /// Активатор контроллеров, использующий UnityContainer
    /// </summary>
    public class UnityControllerActivator : IControllerActivator
    {
        private IUnityContainer _container;

        /// <summary>
        /// Создание экземпляра класса
        /// </summary>
        /// <param name="container">Текущий контейнер</param>
        public UnityControllerActivator(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Создание экземпляра контроллера
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        public IController Create(RequestContext requestContext, Type controllerType)
        {
            if (!controllerType.IsAbstract && controllerType.IsClass && controllerType.IsPublic)
            {
                return (IController)_container.Resolve(controllerType);
            }

            return null;
        }
    }
}
