// Owners: Karlov Nikolay, Abretov Alexey

using System.Web.Mvc;
using System.Web.Routing;

namespace QA.Core.Web
{
    /// <summary>
    /// Интерфейс для фильтра, который срабатывает при вызове Initialize контроллера
    /// </summary>
    public interface IInitilizeFilter
    {
        int Order { get; }
        void OnInitialized(ControllerContext context);
        void OnInitializing(RequestContext context);
    }
}
