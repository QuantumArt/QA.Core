using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QA.Core.Service.Interaction
{
    /// <summary>
    /// Утилита для вызова wcf-сервисов
    /// </summary>
    public static class ServiceExecutor
    {
        ///// <summary>
        ///// WCF proxys do not clean up properly if they throw an exception. This method ensures that the service proxy is handeled correctly.
        ///// Do not call TService.Close() or TService.Abort() within the action lambda.
        ///// </summary>
        ///// <typeparam name="TService">The type of the service to use</typeparam>
        ///// <param name="action">Lambda of the action to performwith the service</param>

        //public void Using<TService>(Action<TService> action, Action<TService> modifier = null)
        //    where TService : ICommunicationObject, IDisposable, new()
        //{
        //    var service = new TService();

        //    //OnInitializeClient(service);

        //    bool success = false;
        //    try
        //    {
        //        if(modifier != null)
        //        {
        //            modifier(service);
        //        }
        //        action(service);
        //        if (service.State != CommunicationState.Faulted)
        //        {
        //            service.Close();
        //            success = true;
        //        }
        //    }
        //    finally
        //    {
        //        if (!success)
        //        {
        //            service.Abort();
        //        }
        //    }
        //}
    }
}
