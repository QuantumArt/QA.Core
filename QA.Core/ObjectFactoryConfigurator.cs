using System;
using System.Collections.Concurrent;
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
        private const string DefaultContainerName = "Default";
        private const string UnitySectionKey = "unity";

        private readonly static ConcurrentDictionary<string, IUnityContainer> NamedContainers
            = new ConcurrentDictionary<string, IUnityContainer>();
        private static readonly object SyncContainer = new object();
        private static IUnityContainer _defaultContainer;

        /// <summary>
        /// Текущий экземпляр unity-контейнера.
        /// Если не задано обратное, то при первом обращении создается контейнер из конфигурационного файла с конфигурацией "Default"
        /// </summary>
        public static IUnityContainer DefaultContainer
        {
            get
            {
                return _defaultContainer ?? (DefaultContainer = GetNamed(DefaultContainerName, initialize: true));
            }
            set
            {
                lock (SyncContainer)
                {
                    _defaultContainer = value;
                }
            }
        }

        /// <summary>
        /// Инициализация и установка контейнера Default параметрами из конфигурационного файла.
        /// </summary>
        public static IUnityContainer Configure(bool shouldFailOnErrors = false)
        {
            var defaultContainer = InternalConfigure(DefaultContainerName, shouldFailOnErrors);

            DefaultContainer = defaultContainer;

            return defaultContainer;
        }

        /// <summary>
        /// Инициализация пользовательским контейнером.
        /// </summary>
        [Obsolete ("Необходимо использовать setter DefaultContainer")]
        public static IUnityContainer InitializeWith(IUnityContainer container)
        {
            DefaultContainer = container;
            return container;
        }

        /// <summary>
        /// Возвращает контейнер IoC по имени
        /// </summary>
        /// <param name="name"></param>
        /// <param name="initialize"></param>
        /// <returns></returns>
        public static IUnityContainer GetNamed(string name, bool initialize = false)
        {
            if (name.Equals(DefaultContainerName, StringComparison.OrdinalIgnoreCase) && !initialize)
                return DefaultContainer;

            var returnedContainer = NamedContainers.GetOrAdd(
                name, containerName =>
                {
                    var container = InternalConfigure(containerName);
                    return container;
                });

            return returnedContainer;
        }

        private static IUnityContainer InternalConfigure(string name, bool shouldFailOnErrors = false)
        {
            var container = new UnityContainer();

            var section = ConfigurationManager.GetSection(UnitySectionKey) as UnityConfigurationSection;

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
            else if (shouldFailOnErrors)
                throw new InvalidOperationException("Configuration for unity is not exist.");

            return container;
        }
    }
}
