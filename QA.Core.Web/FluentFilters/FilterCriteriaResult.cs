#pragma warning disable 1591

namespace QA.Core.Web.FluentFilters
{
    public class FilterCriteriaResult
    {
        public IFilterCriteria Criteria { get; private set; }

        public FilterCriteriaType Type { get; private set; }

        public int Level { get; private set; }

        public FilterCriteriaResult(IFilterCriteria criteria, FilterCriteriaType type, int level)
        {
            this.Criteria = criteria;
            this.Type = type;
            this.Level = level;
        }
    }
}
