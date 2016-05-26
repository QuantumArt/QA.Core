// Owners: Alexey Abretov, Nikolay Karlov

using System.Web;

namespace QA.Core.Web
{
    /// <summary>
    /// Расширяет веб-запрос
    /// </summary>
    public static class RequestExtender
    {
        /// <summary>
        /// Проверяет наличие файла в запросе
        /// </summary>
        /// <param name="file">Файл</param>
        /// <returns>True - присутствует, False - отсутствует</returns>
        public static bool HasFile(this HttpPostedFileBase file)
        {
            return (file != null && file.ContentLength > 0) ? true : false;
        }
    }
}
