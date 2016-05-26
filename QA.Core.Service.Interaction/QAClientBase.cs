// Owners: Alexey Abretov, Nikolay Karlov
using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace QA.Core.Service.Interaction
{
    /// <summary>
    /// Базовый класс сервиса
    /// </summary>
    public abstract class QAClientBase : QAServiceBase
    {
        ///// <summary>
        ///// WCF proxys do not clean up properly if they throw an exception. This method ensures that the service proxy is handeled correctly.
        ///// Do not call TService.Close() or TService.Abort() within the action lambda.
        ///// </summary>
        ///// <typeparam name="TService">The type of the service to use</typeparam>
        ///// <param name="action">Lambda of the action to performwith the service</param>

        public void Using<TService>(Action<TService> action, Action<TService> modifier = null)
            where TService : ICommunicationObject, IDisposable, new()
        {
            var service = new TService();

            OnInitializeClient(service);

            bool success = false;
            try
            {
                if (modifier != null)
                {
                    modifier(service);
                }
                action(service);
                if (service.State != CommunicationState.Faulted)
                {
                    service.Close();
                    success = true;
                }
            }
            finally
            {
                if (!success)
                {
                    service.Abort();
                }
            }
        }

        public async Task<T> Using<TService, T>(Func<TService, Task<T>> func, bool continueOnCapturedContext = true, Action<TService> modifier = null)
            where TService : ICommunicationObject, IDisposable, new()
        {
            var service = new TService();

            OnInitializeClient(service);
            T result = default(T);

            bool success = false;
            try
            {
                if (modifier != null)
                {
                    modifier(service);
                }
                
                var c1 = Thread.CurrentThread.CurrentCulture;
                var c2 = Thread.CurrentThread.CurrentUICulture;

                result = await func(service).ConfigureAwait(continueOnCapturedContext);

                Thread.CurrentThread.CurrentCulture = c1;
                Thread.CurrentThread.CurrentUICulture = c2;

                if (service.State != CommunicationState.Faulted)
                {
                    service.Close();
                    success = true;
                }

                return result;
            }
            finally
            {
                if (!success)
                {
                    service.Abort();
                }
            }
        }

        protected abstract void OnInitializeClient(object service);
    }
}
