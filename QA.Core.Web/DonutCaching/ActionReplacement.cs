using System.Web.Routing;

namespace QA.Core.Web
{
    internal class ActionReplacement : ReplacementBase
    {
        public RouteValueDictionary RouteValues { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}
