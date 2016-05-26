using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace QA.Core.Web.FluentFilters
{
    public class FilterRecord
    {
        private readonly List<FilterCriteriaResult> _requireCriteria;
        private readonly List<FilterCriteriaResult> _excludeCriteria;

        public object FilterInstance { get; private set; }

        public Type FilterType { get; private set; }

        public int? Order { get; private set; }

        public FilterRecord(IEnumerable<FilterCriteriaResult> criteria, Type filterType, int? order)
        {
            this._requireCriteria = new List<FilterCriteriaResult>(Enumerable.Where<FilterCriteriaResult>(criteria, (Func<FilterCriteriaResult, bool>)(c => c.Type == FilterCriteriaType.And)));
            this._excludeCriteria = new List<FilterCriteriaResult>(Enumerable.Where<FilterCriteriaResult>(criteria, (Func<FilterCriteriaResult, bool>)(c => c.Type == FilterCriteriaType.Not)));
            this.FilterType = filterType;
            this.Order = order;
        }

        public FilterRecord(IEnumerable<FilterCriteriaResult> criteria, object instance, int? order)
            : this(criteria, instance.GetType(), order)
        {
            this.FilterInstance = instance;
        }

        public bool Match(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            int num1 = Enumerable.Aggregate<FilterCriteriaResult, int>((IEnumerable<FilterCriteriaResult>)this._requireCriteria, 0, (Func<int, FilterCriteriaResult, int>)((prev, c) =>
            {
                if (c.Level <= prev)
                    return prev;
                else
                    return c.Level;
            }));
            for (int level = 0; level <= num1; ++level)
            {
                IEnumerable<FilterCriteriaResult> source = Enumerable.Where<FilterCriteriaResult>((IEnumerable<FilterCriteriaResult>)this._requireCriteria, (Func<FilterCriteriaResult, bool>)(c => c.Level.Equals(level)));
                if (!Enumerable.Count<FilterCriteriaResult>(source).Equals(0))
                {
                    if (!Enumerable.Aggregate<FilterCriteriaResult, bool>(source, true, (Func<bool, FilterCriteriaResult, bool>)((prev, f) =>
                    {
                        if (!prev)
                            return prev;
                        else
                            return f.Criteria.Match(controllerContext, actionDescriptor);
                    })))
                    {
                        if (level.Equals(num1))
                            return false;
                    }
                    else
                        break;
                }
            }
            int num2 = Enumerable.Aggregate<FilterCriteriaResult, int>((IEnumerable<FilterCriteriaResult>)this._excludeCriteria, 0, (Func<int, FilterCriteriaResult, int>)((prev, c) =>
            {
                if (c.Level <= prev)
                    return prev;
                else
                    return c.Level;
            }));
            for (int level = 0; level <= num2; ++level)
            {
                IEnumerable<FilterCriteriaResult> source = Enumerable.Where<FilterCriteriaResult>((IEnumerable<FilterCriteriaResult>)this._excludeCriteria, (Func<FilterCriteriaResult, bool>)(c => c.Level.Equals(level)));
                if (!Enumerable.Count<FilterCriteriaResult>(source).Equals(0) && Enumerable.Aggregate<FilterCriteriaResult, bool>(source, true, (Func<bool, FilterCriteriaResult, bool>)((prev, f) =>
                {
                    if (!prev)
                        return prev;
                    else
                        return f.Criteria.Match(controllerContext, actionDescriptor);
                })))
                    return false;
            }
            return true;
        }
    }
}
