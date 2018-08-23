#pragma warning disable 1591

namespace QA.Core.Web.FluentFilters
{
    public interface IExcludeResult
    {
        IExcludeResult And(IFilterCriteria criteria);

        IExcludeResult Or(IFilterCriteria criteria);
    }
}
