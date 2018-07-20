using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Mvc;
using Unity;
using QA.Core.Logger;

//using QA.Core;

namespace QA.Core.Web
{
    /// <summary>
    /// IoC-контейнер, используемый в MVC3
    /// </summary>
    public class UnityDependencyResolver : IDependencyResolver
    {
        private IUnityContainer _container;
        private ILogger _logger;

        /// <summary>
        /// Создание экземпляра класса
        /// </summary>
        /// <param name="container">Текущий контейнер</param>
        public UnityDependencyResolver(IUnityContainer container, ILogger logger)
        {
            _container = container;
            _logger = logger;
        }

        /// <summary>
        /// Получить сервис из контейнера.
        /// </summary>
        /// <param name="serviceType">тип сервиса</param>
        /// <returns>Экземпляр или null, есть данного типа нет в контейнере</returns>
        [DebuggerStepThrough]
        public object GetService(Type serviceType)
        {
            try
            {
                return _container.Resolve(serviceType);
            }
            catch
            {
                _logger.Debug(string.Format("An error was occured when resolving type {0} in UnityDependencyResolver", serviceType));
                return null;
            }            
        }

        /// <summary>
        /// Получить все сервисы данного типа из контейнера.
        /// </summary>
        /// <param name="serviceType">тип сервиса</param>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.ResolveAll(serviceType);
        }
    }
}
