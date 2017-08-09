using Microsoft.Practices.Unity;
using System.Collections.Generic;
using QA.Core.Logger;

namespace QA.Core
{
    /// <summary>
    /// Базовая фабрика объектов
    /// </summary>
    public class ObjectFactoryBase
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public static IUnityContainer DefaultContainer => ObjectFactoryConfigurator.DefaultContainer;

        /// <summary>
        /// Возвращает именованный контейнер
        /// </summary>
        /// <param name="name">Имя контейнера</param>
        /// <returns></returns>
        public static IUnityContainer Get(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return DefaultContainer;
            }

            return ObjectFactoryConfigurator.GetNamed(name);
        }

        /// <summary>
        /// Настройка контейнера
        /// </summary>
        static ObjectFactoryBase()
        {
        }

        /// <summary>
        /// Cтандартный логгера.
        /// </summary>
        public static ILogger Logger => DefaultContainer.Resolve<ILogger>();

        /// <summary>
        /// Экземпляр класса для клиентского журналирования
        /// </summary>
        public static ILogger ClientLogger => DefaultContainer.Resolve<ILogger>("Client");

        /// <summary>
        /// Экземпляр класса для серверного журналирования
        /// </summary>
        public static ILogger ServerLogger => DefaultContainer.Resolve<ILogger>("Server");

        /// <summary>
        /// Экземпляр класса для журналирования почтовых сообщений 
        /// </summary>
        public static ILogger MailLogger => DefaultContainer.Resolve<ILogger>("Mail");

        /// <summary>
        /// Экземпляр класса для журналирования с именнованой конфигурацией 
        /// </summary>
        public static ILogger GetNamedLogger(string name)
        {
            Throws.IfArgumentNullOrEmpty(name, _ => name);
            return DefaultContainer.Resolve<ILogger>(name);
        }

        /// <summary>
        /// Получение стандартного провайдера кэша.
        /// </summary>
        public static ICacheProvider CacheProvider => DefaultContainer.Resolve<ICacheProvider>();

        /// <summary>
        /// Получение стандартного провайдера кэша.
        /// </summary>
        public static IVersionedCacheProvider VersionedCacheProvider => DefaultContainer.Resolve<IVersionedCacheProvider>();

        #region IoC
        /// <summary>
        /// Получение из контейнера или построение объекта.
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
        {
            return DefaultContainer.Resolve<T>();
        }

        /// <summary>
        /// Получение из контейнера или построение объекта.
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="name">имя</param>
        /// <returns></returns>
        public static T Resolve<T>(string name)
        {
            return DefaultContainer.Resolve<T>(name);
        }

        /// <summary>
        /// Получение из контейнера или построение объекта.
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="containerName">Имя контейнера</param>
        /// <returns></returns>
        public static T ResolveByName<T>(string containerName)
        {
            return Get(containerName).Resolve<T>();
        }

        /// <summary>
        /// Получение из контейнера или построение объекта.
        /// </summary>
        /// <typeparam name="T">Тип</typeparam>
        /// <param name="name">Имя объекта в контейнере</param>
        /// <param name="containerName">Имя контейнера</param>
        /// <returns></returns>
        public static T ResolveByName<T>(string containerName, string name)
        {
            return Get(containerName).Resolve<T>(name);
        }


        /// <summary>
        /// возвращает все ИМЕНОВАННЫЕ реализации интерфейса T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> ResolveAll<T>()
        {
            return DefaultContainer.ResolveAll<T>();
        }

        #endregion
    }
}
