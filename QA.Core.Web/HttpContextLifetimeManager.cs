// Owners: Alexey Abretov, Nikolay Karlov

using System;
using System.Threading;
using System.Web;
using Unity;
#pragma warning disable 1591

namespace QA.Core.Web
{
    /// <summary>
    /// Реализует жизненный цикл объекта в текущем запросе.
    /// Если контекст запроса пуст, то используется стратегия ThreadLocal&lt;T&gt;.
    /// </summary>
    public class HttpContextLifetimeManager : HttpContextLifetimeManagerBase
    {
        private readonly string _itemName = Guid.NewGuid().ToString();

        protected override string Key
        {
            get { return _itemName; }
        }
    }
}
