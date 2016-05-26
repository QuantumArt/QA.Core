// Owners: Karlov Nikolay, Abretov Alexey

using System;
using QA.Core.Service.Interaction;

namespace QA.Core.Service
{
    public interface IServiceManager
    {
        ServiceResult<TResult> Run<TResult>(UserContext userContext, Func<TResult> func);
        ServiceEnumerationResult<TResult> Run<TResult>(UserContext userContext, Func<TResult> func, PageInfo info);
        ServiceResult RunAction(UserContext userContext, Action func);
    }
}
