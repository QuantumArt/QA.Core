using System;
using System.Web.Mvc;
#pragma warning disable 1591

namespace QA.Core.Web.FluentFilters
{
    public class ControllerFilterCriteria : IFilterCriteria
    {
        private readonly string _controllerName;

        public ControllerFilterCriteria(string controllerName)
        {
            this._controllerName = controllerName;
        }

        public bool Match(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            return string.Equals(this._controllerName, controllerContext.RouteData.GetRequiredString("controller"), StringComparison.OrdinalIgnoreCase);
        }
    }
}
