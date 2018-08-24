using System;
using System.Web;
#pragma warning disable 1591

namespace QA.Core.Web
{
    /// <summary>
    /// Очистка ресурсов запроса. Применяется в элементам коллекции HttpContext.Items, которые реализуют IDisposable
    /// </summary>
    public class RequestFinalizerModule : IHttpModule
    {
        #region IHttpModule Members

        void IHttpModule.Dispose()
        {
        }

        void IHttpModule.Init(HttpApplication context)
        {
            context.EndRequest += new EventHandler(OnEndRequest);
        }

        #endregion

        protected virtual void OnEndRequest(Object source, EventArgs e)
        {
            var context = ((HttpApplication)source).Context;

            foreach (var key in context.Items.Keys)
            {
                var disposable = context.Items[key] as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}
