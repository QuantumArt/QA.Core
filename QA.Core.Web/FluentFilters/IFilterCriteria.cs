using System.Web.Mvc;
#pragma warning disable 1591

namespace QA.Core.Web.FluentFilters
{
    public interface IFilterCriteria
    {
        bool Match(ControllerContext controllerContext, ActionDescriptor actionDescriptor);
    }
}
