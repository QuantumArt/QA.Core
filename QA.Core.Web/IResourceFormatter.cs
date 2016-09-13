namespace QA.Core.Web
{
    /// <summary>
    /// Предоставляет метод для преобразования значения из файла ресурсов
    /// </summary>
    public interface IResourceFormatter
    {
        /// <summary>
        /// Преобразовать значение из файла ресурсов
        /// </summary>
        /// <param name="initialResource"></param>
        /// <returns></returns>
        string Modify(string initialResource);
    }
}
