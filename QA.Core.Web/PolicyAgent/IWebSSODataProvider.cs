// Owners: Karlov Nikolay, Abretov Alexey

using System.Web;

namespace QA.Core.Web
{
    /// <summary>
    /// Поcтавщик данных от полиси-агента
    /// </summary>
    public interface IWebSSODataProvider
    {
        /// <summary>
        /// Получение данных пользователя-абонента МТС
        /// </summary>
        /// <remarks>Передает cookie</remarks>
        /// <param name="context">Контекст запроса</param>
        /// <param name="saveInSession"></param>
        /// <exception cref="System.Exception">ВСе типы исключений при работе с HttpWebRequest</exception>
        /// <returns></returns>
        WebSSOData GetUserData(HttpContextBase context, bool saveInSession);
    }
}
