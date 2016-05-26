using System;
using System.Web.Mvc;

namespace QA.Core.Web.FluentFilters
{
    public class AreaFilterCriteria : IFilterCriteria
    {
        private readonly string _areaName;

        public AreaFilterCriteria()
        {
            this._areaName = string.Empty;
        }

        public AreaFilterCriteria(string areaName)
        {
            this._areaName = areaName;
        }

        public bool Match(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            object obj = controllerContext.RequestContext.RouteData.DataTokens["area"];
            if (obj == null && string.IsNullOrEmpty(this._areaName))
                return true;
            else if (obj != null)
                return string.Equals(obj.ToString(), this._areaName, StringComparison.OrdinalIgnoreCase);
            else
                return false;
        }
    }
}
