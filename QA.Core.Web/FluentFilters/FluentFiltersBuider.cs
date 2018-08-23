
#pragma warning disable 1591

namespace QA.Core.Web.FluentFilters
{
    public static class FluentFiltersBuider
    {
        public static FluentFilterCollection Filters { get; private set; }

        static FluentFiltersBuider()
        {
            FluentFiltersBuider.Filters = new FluentFilterCollection();
        }
    }
}
