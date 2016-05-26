using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace QA.Core
{
    /// <summary>
    /// Конфигуратор фабрики объектов
    /// </summary>
    public static class ObjectFactoryConfigurator
    {
        private static readonly object _syncRoot = new object();
        private static IUnityContainer _defaultContainer;

        /// <summary>
        /// Текущий экземпляр unity-контейнера.
        /// Если не задано обратное, то при первом обращении создается контейнер из конфигурационного файла с конфигурацией "Default"
        /// </summary>
        public static IUnityContainer DefaultContainer
        {
            get
            {
                if (_defaultContainer == null)
                {
                    _defaultContainer = GetNamed("Default", true);
                }

                return _defaultContainer;
            }

            private set { _defaultContainer = value; }
        }

        /// <summary>
        /// Инициализация и установка контейнера Default параметрами из конфигурационного файла.
        /// </summary>
        public static IUnityContainer Configure(bool shouldFailOnErrors = false)
        {
            var defaultContainer = new UnityContainer();

            var section = ConfigurationManager.GetSection("unity") as UnityConfigurationSection;

            if (section == null)
            {
                if (shouldFailOnErrors)
                    throw new InvalidOperationException("Configuration for unity is not exist.");
            }

            try
            {
                section.Configure(defaultContainer, "Default");
            }
            catch (Exception ex)
            {
                //if (shouldFailOnErrors)
                    throw new InvalidOperationException("An error occured while initializing unity container.", ex);
            }

            //var serviceLocator = new UnityServiceLocator(defaultContainer);

            // устанавливаем ServiceLocator
            //ServiceLocator.SetLocatorProvider(() => serviceLocator);

            //NOTE: was _defaultContainer. Is it right?
            DefaultContainer = defaultContainer;

            return defaultContainer;
        }

        /// <summary>
        /// Инициализация пользовательским контейнером.
        /// </summary>
        public static void InitializeWith(IUnityContainer container)
        {
            DefaultContainer = container;
            //var serviceLocator = new UnityServiceLocator(DefaultContainer);

            // устанавливаем ServiceLocator
            //ServiceLocator.SetLocatorProvider(() => serviceLocator);
        }

        private readonly static Dictionary<string, IUnityContainer> _namedContainers
            = new Dictionary<string,IUnityContainer>();
        private static object _syncContainer = new object();

        /// <summary>
        /// Возвращает контейнер IoC по имени
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IUnityContainer GetNamed(
            string name, bool initialize = false)
        {
            lock (_syncContainer)
            {
                if (name.Equals("Default", StringComparison.OrdinalIgnoreCase) && !initialize)
                    return DefaultContainer;

                if (_namedContainers.ContainsKey(name))
                {
                    IUnityContainer val = _namedContainers[name];

                    if (val != null)
                    {
                        return val;
                    }
                }

                var container = new UnityContainer();

                var section = ConfigurationManager.GetSection("unity") as UnityConfigurationSection;

                if (section != null)
                { 
                    try
                    {
                        section.Configure(container, name);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException("An error occured while initializing unity container.", ex);
                    }
                }

                _namedContainers.Add(name, container);

                return container;
            }
        }
    }
}
