// Owners: Karlov Nikolay, Abretov Alexey

using System.Collections.Generic;
using System.Web.Mvc;

namespace QA.Core.Web
{
    internal interface IReplacementStorage
    {
        List<ReplacementBase> GetReplacements();
        void SetCache(DonutCacheItem itemToSet);
        void RenderAction(ActionReplacement actionReplacement, System.IO.StringWriter writer);
    }
}
