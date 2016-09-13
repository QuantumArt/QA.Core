namespace QA.Core.Web
{
    /// <summary>
    /// Тривиальная ничего не делающая реализация IResourceFormatter. Значения ресурсов остаются неизменными.
    /// </summary>
    public class DefaultResourceFormatter : IResourceFormatter
    {
        public string Modify(string initialResource)
        {
            return initialResource;
        }
    }
}
