// Owners: Alexey Abretov, Nikolay Karlov

using System;
using System.Threading;
using System.Web;
using Microsoft.Practices.Unity;

namespace QA.Core.Web
{
    /// <summary>
    /// Реализует жизненный цикл объекта в текущем запросе.
    /// Если контекст запроса пуст, то используется стратегия ThreadLocal<T>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HttpContextLifetimeManager<T> : HttpContextLifetimeManagerBase
    {
        /// <summary>
        /// TODO: разобраться, какое именование лучше: _itemName = Guid.NewGuid().ToString() или 
        /// _itemName = typeof(T).AssemblyQualifiedName;
        /// Второе именование не позволяет создавать именнованые экземпляры одного типа
        /// </summary>
        private readonly string _itemName = typeof(T).AssemblyQualifiedName;


        protected override string Key
        {
            get { return _itemName; }
        }
    }
}
