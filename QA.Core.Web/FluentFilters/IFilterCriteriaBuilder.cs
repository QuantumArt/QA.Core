
namespace QA.Core.Web.FluentFilters
{
    public interface IFilterCriteriaBuilder
    {
        IRequireResult Require(IFilterCriteria criteria);

        IExcludeResult Exclude(IFilterCriteria criteria);
    }
}
