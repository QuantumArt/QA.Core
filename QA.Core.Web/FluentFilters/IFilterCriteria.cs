using System.Web.Mvc;

namespace QA.Core.Web.FluentFilters
{
    public interface IFilterCriteria
    {
        bool Match(ControllerContext controllerContext, ActionDescriptor actionDescriptor);
    }
}
