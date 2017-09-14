// Owners: Alexey Abretov, Nikolay Karlov
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Web;
using QA.Core.Data;
using QA.Core.Data.Repository;
using QA.Core.Logger;

namespace QA.Core.Service.Interaction
{
    /// <summary>
    /// Базовый класс сервиса
    /// </summary>
    public abstract class QAServiceBase : IDisposable
    {
        /// <summary>
        /// Текст ошибки
        /// </summary>
        protected const string GeneralServiceExceptionMessage = "Произошла ошибка при работе сервиса.";

        /// <summary>
        /// Текст ошибки
        /// </summary>
        protected const string UserContentNullExceptionMessage = "Неверно указан контекст пользователя.";

        /// <summary>
        /// Собственный логгер
        /// </summary>
        protected ILogger Logger { get; set; }


        private ILogger GetLogger()
        {
            return Logger ?? ObjectFactoryBase.Logger;
        }

        /// <summary>
        /// Создает ненулевой объект для репорта об ошибке
        /// </summary>
        protected static ServiceResult<T> GenerateError<T>(string msg, int errorCode = 0)
        {
            return new ServiceResult<T>
            {
                Error = new ServiceError { Message = msg, ErrorCode = errorCode, Type = ServiceErrorType.Exception }
            };
        }

        /// <summary>
        /// Создает ненулевой объект для репорта об ошибке
        /// </summary>
        protected static ServiceResult<T> GenerateError<T>(
            string msg, int errorCode, ServiceErrorType type)
        {
            return new ServiceResult<T>
            {
                Error = new ServiceError { Message = msg, ErrorCode = errorCode, Type = type },
                IsSucceeded = false
            };
        }

        /// <summary>
        /// Создает ненулевой объект для репорта об ошибке
        /// </summary>
        protected static ServiceEnumerationResult<T> GenerateEnumerationError<T>(
            string msg, int errorCode = 0, ServiceErrorType type = ServiceErrorType.Exception)
        {
            return new ServiceEnumerationResult<T>
            {
                Error = new ServiceError { Message = msg, ErrorCode = errorCode, Type = type },
                IsSucceeded = false
            };
        }

        /// <summary>
        /// Журналирует исключение
        /// </summary>
        /// <param name="exception">Объект исключения</param>
        protected virtual void LogException(Exception exception)
        {
            GetLogger()?.ErrorException(exception.Message, exception);
        }


        #region Run

        /// <summary>
        /// Выполнение сервиса
        /// </summary>
        /// <param name="func">Функция, выполняющаяся при успешных проверках</param>
        /// <returns></returns>
        protected virtual ServiceResult<TResult> Run<TResult>(
            UserContext userContext,
            ISecurityContext securityContext,
            Func<TResult> func/*, [CallerMemberName] string caller = ""*/)
        {
            var result = new ServiceResult<TResult>();

            if (userContext == null)
            {
                Throws.IfArgumentNull(
                    UserContentNullExceptionMessage, userContext, "userContext");
            }

            try
            {
                result.Result = func();
                result.IsSucceeded = true;
            }
            catch (OperationExecutionException ex)
            {
                result.IsSucceeded = false;
                result.Error = new ServiceError { ErrorCode = -1, Message = ex.Message, Type = ServiceErrorType.BusinessLogicMessage };
            }
            catch (DataException ex)
            {
                result.IsSucceeded = false;
                result.Error = new ServiceError { ErrorCode = -1, Message = ex.Message, Type = ServiceErrorType.BusinessLogicMessage };
            }
            catch (Exception ex)
            {
                result.IsSucceeded = false;
                result.Error = new ServiceError { ErrorCode = -1, Message = ex.Message };
                GetLogger()?.ErrorException(
                    GeneralServiceExceptionMessage
                    //+ this.GetType() + caller
                    , ex);
            }

            return result;
        }

        /// <summary>
        /// Выполнение действия сервиса
        /// </summary>
        /// <typeparam name="TResult">Тип сущности</typeparam>
        /// <param name="userContext">Контекст пользователя</param>
        /// <param name="func">Функция, выполняющаяся при успешных проверках</param>
        /// <returns></returns>
        protected virtual ServiceEnumerationResult<TResult> RunEnumeration<TResult>(
            UserContext userContext,
            ISecurityContext securityContext,
            Func<List<TResult>> func)
        {
            var result = new ServiceEnumerationResult<TResult>();

            if (userContext == null)
            {
                Throws.IfArgumentNull(
                    UserContentNullExceptionMessage, userContext, "userContext");
            }

            try
            {
                result.Result = func();
                result.IsSucceeded = true;
            }
            catch (OperationExecutionException ex)
            {
                result.IsSucceeded = false;
                result.Error = new ServiceError { ErrorCode = -1, Message = ex.Message, Type = ServiceErrorType.BusinessLogicMessage };
            }
            catch (DataException ex)
            {
                result.IsSucceeded = false;
                result.Error = new ServiceError { ErrorCode = -1, Message = ex.Message, Type = ServiceErrorType.BusinessLogicMessage };
            }
            catch (Exception ex)
            {
                result.IsSucceeded = false;
                result.Error = new ServiceError { ErrorCode = -1, Message = ex.Message };
                GetLogger()?.ErrorException(
                    GeneralServiceExceptionMessage, ex);
            }

            return result;
        }

        /// <summary>
        /// Выполнение сервиса
        /// </summary>
        /// <param name="func">Функция, выполняющаяся при успешных проверках</param>
        /// <returns></returns>
        protected virtual ServiceResult<TResult> Run2<TResult>(
            UserContext userContext,
            ISecurityContext securityContext,
            Func<ServiceResult<TResult>> func)
        {
            var result = new ServiceResult<TResult>();

            if (userContext == null)
            {
                Throws.IfArgumentNull(
                    UserContentNullExceptionMessage, userContext, "userContext");
            }

            try
            {
                result = func();
            }
            catch (OperationExecutionException ex)
            {
                result.IsSucceeded = false;
                result.Error = new ServiceError { ErrorCode = -1, Message = ex.Message, Type = ServiceErrorType.BusinessLogicMessage };
            }
            catch (DataException ex)
            {
                result.IsSucceeded = false;
                result.Error = new ServiceError { ErrorCode = -1, Message = ex.Message, Type = ServiceErrorType.BusinessLogicMessage };
            }
            catch (Exception ex)
            {
                result.IsSucceeded = false;
                result.Error = new ServiceError { ErrorCode = -1, Message = ex.Message };
                GetLogger()?.ErrorException(
                    GeneralServiceExceptionMessage, ex);
            }

            return result;
        }

        /// <summary>
        /// Выполнение действия сервиса
        /// </summary>
        /// <typeparam name="TResult">Тип сущности</typeparam>
        /// <param name="userContext">Контекст пользователя</param>
        /// <param name="func">Функция, выполняющаяся при успешных проверках</param>
        /// <returns></returns>
        protected virtual ServiceEnumerationResult<TResult> RunEnumeration<TResult>(
            UserContext userContext,
            ISecurityContext securityContext,
            Func<ServiceEnumerationResult<TResult>> func)
        {
            var subResult = Run2(userContext, securityContext, func);

            ServiceEnumerationResult<TResult> result = null;

            if (subResult.IsSucceeded)
            {
                result = (ServiceEnumerationResult<TResult>)subResult;
            }
            else
            {
                result = new ServiceEnumerationResult<TResult> { Error = subResult.Error };
            }

            return result;
        }

        /// <summary>
        /// Выполнение действия
        /// </summary>
        /// <param name="userContext">Контекст пользователя</param>
        /// <param name="securityContext">Контекст безопасности</param>
        /// <param name="action">Действие</param>
        /// <returns></returns>
        protected virtual ServiceResult RunAction(
            UserContext userContext,
            ISecurityContext securityContext,
            Action action)
        {
            return (ServiceResult)Run(userContext, securityContext, () =>
            {
                var result = new ServiceResult<object> { IsSucceeded = true };
                action();
                return result;
            });
        }

        #endregion

        /// <summary>
        /// Освобождение ресурсов
        /// </summary>
        public virtual void Dispose()
        {

        }

        /// <summary>
        /// Сохраняет изменения сделанные в контексте
        /// <param name="name">Имя UnitOfWork</param>
        /// </summary>
        protected virtual void Commit(
            string name)
        {
            var context = string.IsNullOrEmpty(name) ?
                Resolve<IUnitOfWork>(
                    ) :
                Resolve<IUnitOfWork>(
                    name);

            context.Commit();
        }

        /// <summary>
        /// Текущие данные с клиента сервису
        /// </summary>
        protected virtual ServiceToken CurrentServiceToken
        {
            get
            {
                ServiceToken token = null;

                if (OperationContext.Current != null)
                {
                    int pos = OperationContext.Current.IncomingMessageHeaders.FindHeader(
                        ServiceToken.ServiceTokenKey, ServiceToken.ServiceTokenNs);
                    if (pos >= 0)
                    {
                        token = OperationContext.Current.IncomingMessageHeaders.GetHeader<ServiceToken>(
                            ServiceToken.ServiceTokenKey, ServiceToken.ServiceTokenNs);
                    }
                }

                if (token == null && HttpContext.Current != null)
                {
                    token = HttpContext.Current.Items[ServiceToken.ServiceTokenKey] as ServiceToken;
                }

                if (token == null && ObjectFactoryBase.CacheProvider != null)
                {
                    token = ObjectFactoryBase.CacheProvider.Get(ServiceToken.ServiceTokenKey) as ServiceToken;
                }

                return token;
            }
        }

        /// <summary>
        /// Построение объекта
        /// <param name="T">Тип</param>
        /// </summary>
        protected virtual T Resolve<T>()
        {
            return Resolve<T>(null);
        }

        /// <summary>
        /// Построение объекта
        /// <param name="T">Тип</param>
        /// <param name="name">register name</param>
        /// </summary>
        protected virtual T Resolve<T>(string name)
        {
            var current = CurrentServiceToken;

            return ObjectFactoryBase.ResolveByName<T>(current == null ?
                "Default" :
                current.DependencyContainerName, name);
        }

        //protected virtual T Verify<T>(T result)
        //    where T:ServiceResult
        //{
        //    if (result != null && !result.IsSucceeded)
        //    {

        //    }
        //    return result;
        //}
    }
}
