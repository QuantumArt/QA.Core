
namespace QA.Core.Web.FluentFilters
{
    public interface IRequireResult
    {
        IRequireResult And(IFilterCriteria criteria);

        IRequireResult Or(IFilterCriteria criteria);
    }
}
