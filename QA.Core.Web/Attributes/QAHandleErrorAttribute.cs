// Owners: Alexey Abretov, Nikolay Karlov

using System;
using System.Web.Mvc;

namespace QA.Core.Web
{
    /// <summary>
    /// Атрибут обработки исключений
    /// <remarks>
    /// Запрещаем множественное применение.
    /// </remarks>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, 
        AllowMultiple = false)]
    public class QAHandleErrorAttribute : HandleErrorAttribute
    {
        /// <summary>
        /// Экземпляр класса для журналирования
        /// </summary>
        public ILogger Logger
        {
            get { return ObjectFactoryBase.Logger; }
        }
         
        /// <summary>
        /// Вызывается при обнаружении ошибки.
        /// Журналирует ошибку и перенаправляет на страницу по умолчанию.
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            var ex = filterContext.Exception;

            while (ex != null)
            {
                Logger.ErrorException(ex.Message, ex);

                ex = ex.InnerException;
            }

            base.OnException(filterContext);
        }
    }
}
