// Owners: Alexey Abretov, Nikolay Karlov

using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Unity;
using QA.Core.Logger;
using Unity.Attributes;

namespace QA.Core.Web
{
    /// <summary>
    /// Фабрика контроллеров для MVC
    /// </summary>
    public class UnityControllerFactory : DefaultControllerFactory
    {
        private readonly IUnityContainer _container;

        /// <summary>
        /// Экземпляр класса для журналирования
        /// </summary>
        [Dependency]
        public static ILogger Logger
        {
            get;
            set;
        }

        /// <summary>
        /// Конструирует класс фабрики с unity-контейнером
        /// </summary>
        /// <param name="_container">Unity-контейнер</param>
        public UnityControllerFactory(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Создает экземпляр контроллера
        /// </summary>
        /// <param name="requestContext">Контекст запроса</param>
        /// <param name="controllerType">Тип контроллера</param>
        /// <returns>Контроллер</returns>
        protected override IController GetControllerInstance(
            RequestContext requestContext, Type controllerType)
        {
            try
            {
                if (controllerType == null || !typeof(IController).IsAssignableFrom(controllerType))
                {
                    // создаем ошибку 404
                    throw new HttpException(404, String.Format(
                     "The controller for path '{0}' could not be found" +
                            "or it does not implement IController.",
                                 requestContext.HttpContext.Request.Path));
                }

                return _container.Resolve(controllerType) as IController;
            }
            catch (Exception ex)
            {
                if (Logger != null)
                {
                    Logger.ErrorException(ex.Message, ex); 
                }

                throw new HttpException(404, ex.Message);
            }
        }
    }
}
