using System;
using System.Collections.Generic;
using System.Linq;

namespace QA.Core.Web.FluentFilters
{
    public class FilterCriteriaBuilder : IFilterCriteriaBuilder
    {
        private readonly List<FilterCriteriaResult> _results;

        public FilterCriteriaBuilder()
        {
            this._results = new List<FilterCriteriaResult>();
        }

        public IRequireResult Require(IFilterCriteria criteria)
        {
            if (Enumerable.Count<FilterCriteriaResult>(Enumerable.Where<FilterCriteriaResult>((IEnumerable<FilterCriteriaResult>)this._results, (Func<FilterCriteriaResult, bool>)(c => c.Type.Equals((object)FilterCriteriaType.And)))) > 0)
                throw new InvalidOperationException("Required criteria were already registered. Use methods And(...) or Or(..) at required criteria chain for register new one.");
            else
                return (IRequireResult)new FilterCriteriaBuilder.RequireResult(this, criteria);
        }

        public IExcludeResult Exclude(IFilterCriteria criteria)
        {
            if (Enumerable.Count<FilterCriteriaResult>(Enumerable.Where<FilterCriteriaResult>((IEnumerable<FilterCriteriaResult>)this._results, (Func<FilterCriteriaResult, bool>)(c => c.Type.Equals((object)FilterCriteriaType.Not)))) > 0)
                throw new InvalidOperationException("Excluded criteria were already registered. Use method And(...) at excluded criteria chain for register new one.");
            else
                return (IExcludeResult)new FilterCriteriaBuilder.ExcludeResult(this, criteria);
        }

        public List<FilterCriteriaResult> GetResults()
        {
            return this._results;
        }

        public class RequireResult : IRequireResult
        {
            private readonly FilterCriteriaBuilder _builder;

            public RequireResult(FilterCriteriaBuilder builder, IFilterCriteria criteria)
            {
                this._builder = builder;
                this.And(criteria);
            }

            public IRequireResult And(IFilterCriteria criteria)
            {
                this._builder._results.Add(new FilterCriteriaResult(criteria, FilterCriteriaType.And, this.GetLevel()));
                return (IRequireResult)this;
            }

            public IRequireResult Or(IFilterCriteria criteria)
            {
                this._builder._results.Add(new FilterCriteriaResult(criteria, FilterCriteriaType.And, this.GetLevel() + 1));
                return (IRequireResult)this;
            }

            private int GetLevel()
            {
                int lastIndex = this._builder._results.FindLastIndex((Predicate<FilterCriteriaResult>)(p => p.Level >= 0));
                if (lastIndex == -1)
                    return 0;
                else
                    return this._builder._results[lastIndex].Level;
            }
        }

        public class ExcludeResult : IExcludeResult
        {
            private readonly FilterCriteriaBuilder _builder;

            public ExcludeResult(FilterCriteriaBuilder builder, IFilterCriteria criteria)
            {
                this._builder = builder;
                this.And(criteria);
            }

            public IExcludeResult And(IFilterCriteria criteria)
            {
                this._builder._results.Add(new FilterCriteriaResult(criteria, FilterCriteriaType.Not, this.GetLevel()));
                return (IExcludeResult)this;
            }

            public IExcludeResult Or(IFilterCriteria criteria)
            {
                this._builder._results.Add(new FilterCriteriaResult(criteria, FilterCriteriaType.Not, this.GetLevel() + 1));
                return (IExcludeResult)this;
            }

            private int GetLevel()
            {
                int lastIndex = this._builder._results.FindLastIndex((Predicate<FilterCriteriaResult>)(p => p.Level >= 0));
                if (lastIndex == -1)
                    return 0;
                else
                    return this._builder._results[lastIndex].Level;
            }
        }
    }
}
