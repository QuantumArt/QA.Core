// Owners: Karlov Nikolay, Abretov Alexey

namespace QA.Core.Web
{
    /// <summary>
    /// AJAX-ответ от сервера
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonServerResult<T> : JsonServerResult
        where T : new()
    {
        /// <summary>
        /// Произвольные сериализуемые данные
        /// </summary>
        public T Result { get; set; }

        public JsonServerResult()
        {
            Result = new T();
            Error = new JsonError();
        }
    }


    /// <summary>
    /// AJAX-ответ от сервера
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonServerResult
    {
        /// <summary>
        /// Успешно ли выполнение запроса
        /// </summary>
        public bool IsSucceeded { get; set; }

        /// <summary>
        /// Информация об ошибке (если есть)
        /// </summary>
        public JsonError Error { get; set; }

        /// <summary>
        /// Сообщение для пользователя
        /// </summary>
        public string Message { get; set; }

        public JsonServerResult()
        {
        }
    }
}
