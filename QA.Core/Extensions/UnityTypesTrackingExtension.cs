using System;
using System.Linq;
using System.Collections.Generic;
using Unity.Extension;
using Unity;

namespace QA.Core.Extensions
{
    /// <summary>
    /// Extensions for Microsoft Unity
    /// </summary>
    public static class UnityExtensions
    {
        /// <summary>
        /// Extension that tracks types registered in the container.
        /// </summary>
        public class UnityExtensionWithTypeTracking : UnityContainerExtension
        {
            private readonly Dictionary<Type, Type> types = new Dictionary<Type, Type>();
            private readonly Dictionary<string, Type> namedTypes = new Dictionary<string, Type>();
            private const string NamedTypeFormatString = "{0}, {1}";

            /// <summary>
            /// Returns whether a type is registered.
            /// </summary>
            /// <param name="type">The type.</param>
            public bool IsRegistered(Type type)
            {
                return this.types.ContainsKey(type)
                    || (this.Container.Parent != null && this.Container.Parent.IsRegistered(type));
            }

            /// <summary>
            /// Returns whether a type is registered.
            /// </summary>
            /// <param name="type">The type.</param>
            /// <param name="name">The name the type is registered with.</param>
            public bool IsRegistered(Type type, string name)
            {
                var namedTypeKey = UnityExtensionWithTypeTracking.GetNamedTypeKey(type, name);

                return this.namedTypes.ContainsKey(namedTypeKey)
                    || (this.Container.Parent != null && this.Container.Parent.IsRegistered(type, name));
            }

            protected override void Initialize()
            {
                this.Context.RegisteringInstance += (sender, e) => this.AddRegistrationToDictionaries(e.RegisteredType, e.Name);
                this.Context.Registering += (sender, e) => this.AddRegistrationToDictionaries(e.TypeFrom, e.Name);
            }

            public override void Remove()
            {
                this.Context.RegisteringInstance -= (sender, e) => this.AddRegistrationToDictionaries(e.RegisteredType, e.Name);
                this.Context.Registering -= (sender, e) => this.AddRegistrationToDictionaries(e.TypeFrom, e.Name);

                base.Remove();
            }

            /// <summary>
            /// Returns a key for use in the namedTypes dictionary.
            /// </summary>
            /// <param name="type">The registered type.</param>
            /// <param name="name">The name of the registration.</param>
            private static string GetNamedTypeKey(Type type, string name)
            {
                return string.Format(NamedTypeFormatString, type.AssemblyQualifiedName, name);
            }

            /// <summary>
            /// Adds a registration to the dictionaries.
            /// </summary>
            /// <param name="type">The from type.</param>
            /// <param name="name">The name of the registration.</param>
            private void AddRegistrationToDictionaries(Type type, string name)
            {
                if (!types.ContainsKey(type))
                    types.Add(type, type);

                if (name == null) return;
                var key = GetNamedTypeKey(type, name);
                if (!namedTypes.ContainsKey(key))
                    namedTypes.Add(key, type);
            }
        }

        /// <summary>
        /// Returns whether a type is registered.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="container">The container.</param>
        public static bool IsRegistered<T>(this IUnityContainer container)
        {
            return container.IsRegistered(typeof(T));
        }

        /// <summary>
        /// Returns whether a type is registered.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="type">The type.</param>
        public static bool IsRegistered(this IUnityContainer container, Type type)
        {
            return container.Configure<UnityExtensionWithTypeTracking>().IsRegistered(type);
        }

        /// <summary>
        /// Returns whether a type is registered.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="container">The container.</param>
        /// <param name="name">The name the type is registered with.</param>
        public static bool IsRegistered<T>(this IUnityContainer container, string name)
        {
            return container.IsRegistered(typeof(T), name);
        }

        /// <summary>
        /// Returns whether a type is registered.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="type">The type.</param>
        /// <param name="name">The name the type is registered with.</param>
        public static bool IsRegistered(this IUnityContainer container, Type type, string name)
        {
            return container.Configure<UnityExtensionWithTypeTracking>().IsRegistered(type, name);
        }

        /// <summary>
        /// Attempts to resolve a type, returning null when the type isn't registered.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="type">The type to resolve.</param>
        public static object TryResolve(this IUnityContainer container, Type type)
        {
            if (!type.IsInterface)
                throw new ArgumentException("The type must be an interface.", "type");

            if (container.IsRegistered(type))
                return container.Resolve(type);
            return null;
        }

        /// <summary>
        /// Attempts to resolve a type, returning null when the type isn't registered.
        /// </summary>
        /// <typeparam name="T">The type to resolve.</typeparam>
        /// <param name="container">The container.</param>
        public static T TryResolve<T>(this IUnityContainer container)
            where T : class
        {
            return (T)container.TryResolve(typeof(T));
        }

        /// <summary>
        /// Attempts to resolve a type, returning null when the type isn't registered.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="type">The type to resolve.</param>
        /// <param name="name">The name the type is registered with.</param>
        public static object TryResolve(this IUnityContainer container, Type type, string name)
        {
            if (!type.IsInterface)
                throw new ArgumentException("The type must be an interface.", "type");

            if (container.IsRegistered(type, name))
                return container.Resolve(type, name);
            return null;
        }

        /// <summary>
        /// Attempts to resolve a type, returning null when the type isn't registered.
        /// </summary>
        /// <typeparam name="T">The type to resolve.</typeparam>
        /// <param name="container">The container.</param>
        /// <param name="name">The name the type is registered with.</param>
        public static T TryResolve<T>(this IUnityContainer container, string name)
            where T : class
        {
            return (T)container.TryResolve(typeof(T), name);
        }

        /// <summary>
        /// Attempts to resolve a type, resolving the substitute type if the type isn't registered.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="type">The type to resolve.</param>
        /// <param name="substitute">The type to resolve if the first type isn't registered.</param>
        public static object TryResolve(this IUnityContainer container, Type type, Type substitute)
        {
            return container.TryResolve(type) ?? container.Resolve(substitute);
        }

        /// <summary>
        /// Attempts to resolve a type, resolving the substitute type if the type isn't registered.
        /// </summary>
        /// <typeparam name="T">The type to resolve.</typeparam>
        /// <typeparam name="TSubstitute">The type to resolve if the first type isn't registered.</typeparam>
        /// <param name="container">The container.</param>
        public static T TryResolve<T, TSubstitute>(this IUnityContainer container)
            where T : class
            where TSubstitute : T
        {
            return (T)container.TryResolve(typeof(T), typeof(TSubstitute));
        }

        /// <summary>
        /// Retrieves the child container registered with the specified name.  If one doesn't exist
        /// then a new child container is created, registered and returned.
        /// </summary>        
        /// The name the child container instance was registered with
        /// </param>
        public static IUnityContainer GetChildContainer(this IUnityContainer container, string name)
        {
            if (container.IsRegistered<IUnityContainer>(name))
                return container.Resolve<IUnityContainer>(name);

            var childContainer = container.CreateChildContainer();
            childContainer.AddNewExtension<UnityExtensionWithTypeTracking>();
            container.RegisterInstance<IUnityContainer>(name, childContainer);

            return childContainer;
        }

        /// <summary>
        /// Resolves a list of types.
        /// </summary>
        /// <typeparam name="T">The base type or common interface of each item in the list.</typeparam>
        /// <param name="types">A list of types to resolve.</param>
        public static IEnumerable<T> Resolve<T>(this IUnityContainer container, IEnumerable<Type> types)
        {
            var items = new List<T>(types.Count());
            foreach (var type in types)
            {
                if (typeof(T).IsAssignableFrom(type))
                    items.Add((T)container.Resolve(type));
                else
                    throw new ArgumentException(string.Format("Each element must be a {0}.", typeof(T).FullName), "types");
            }
            return items;
        }
    }
}
