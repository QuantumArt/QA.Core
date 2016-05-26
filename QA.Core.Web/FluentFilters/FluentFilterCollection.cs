using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace QA.Core.Web.FluentFilters
{
    public class FluentFilterCollection : IFilterProvider
    {
        private List<FilterRecord> _records = new List<FilterRecord>();

        public int Count
        {
            get
            {
                return this._records.Count;
            }
        }

        public void Add<T>()
        {
            this.AddInternal<T>(new int?());
        }

        public void Add<T>(int order)
        {
            this.AddInternal<T>(new int?(order));
        }

        public void Add<T>(Action<IFilterCriteriaBuilder> criteria)
        {
            FluentFilterCollection filterCollection = this;
            int? nullable = new int?();
            Action<IFilterCriteriaBuilder> criteria1 = criteria;
            int? order = nullable;
            filterCollection.AddInternal<T>(criteria1, order);
        }

        public void Add<T>(Action<IFilterCriteriaBuilder> criteria, int order)
        {
            this.AddInternal<T>(criteria, new int?(order));
        }

        public void Add(object filter, Action<IFilterCriteriaBuilder> criteria)
        {
            this.AddInternal(filter, criteria, new int?());
        }

        public void Add(object filter, Action<IFilterCriteriaBuilder> criteria, int order)
        {
            this.AddInternal(filter, criteria, new int?(order));
        }

        public void Clear()
        {
            this._records.Clear();
        }

        public void Remove<T>()
        {
            this._records.RemoveAll((Predicate<FilterRecord>)(f => f.FilterType.Equals(typeof(T))));
        }

        private void AddInternal(object filter, Action<IFilterCriteriaBuilder> criteria, int? order)
        {
            FilterCriteriaBuilder filterCriteriaBuilder = new FilterCriteriaBuilder();
            criteria((IFilterCriteriaBuilder)filterCriteriaBuilder);
            this._records.Add(new FilterRecord((IEnumerable<FilterCriteriaResult>)filterCriteriaBuilder.GetResults(), filter, order));
        }

        private void AddInternal<T>(Action<IFilterCriteriaBuilder> criteria, int? order)
        {
            FilterCriteriaBuilder filterCriteriaBuilder = new FilterCriteriaBuilder();
            criteria((IFilterCriteriaBuilder)filterCriteriaBuilder);
            this._records.Add(new FilterRecord((IEnumerable<FilterCriteriaResult>)filterCriteriaBuilder.GetResults(), typeof(T), order));
        }

        private void AddInternal<T>(int? order)
        {
            this._records.Add(new FilterRecord(Enumerable.Empty<FilterCriteriaResult>(), typeof(T), order));
        }

        protected virtual IMvcFilter GetFilterInstance(Type filterType)
        {
            return (IMvcFilter)Activator.CreateInstance(filterType);
        }

        IEnumerable<Filter> IFilterProvider.GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            List<Filter> list = new List<Filter>();
            foreach (FilterRecord filterRecord in this._records)
            {
                if (filterRecord.Match(controllerContext, actionDescriptor))
                {
                    if (filterRecord.FilterInstance != null)
                        list.Add(new Filter(filterRecord.FilterInstance, FilterScope.Global, filterRecord.Order));
                    else
                        list.Add(new Filter((object)this.GetFilterInstance(filterRecord.FilterType), FilterScope.Global, filterRecord.Order));
                }
            }
            return (IEnumerable<Filter>)list;
        }
    }
}
