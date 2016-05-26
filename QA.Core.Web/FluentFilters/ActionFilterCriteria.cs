using System;
using System.Web.Mvc;

namespace QA.Core.Web.FluentFilters
{
    public class ActionFilterCriteria : IFilterCriteria
    {
        private readonly string _actionName;

        public ActionFilterCriteria(string actionName)
        {
            this._actionName = actionName;
        }

        public bool Match(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            return string.Equals(this._actionName, controllerContext.RouteData.GetRequiredString("action"), StringComparison.OrdinalIgnoreCase);
        }
    }
}
