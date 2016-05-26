using QA.Core.Service.Interaction;

namespace QA.Core.Web
{
    /// <summary>
    /// Базовая модель данных
    /// </summary>
    public class ModelBase
    {
        /// <summary>
        /// Контекст пользователя
        /// </summary>
        protected virtual UserContext UserContext
        {
            get
            {
                return new UserContext();
            }
        }

        /// <summary>
        /// Пользователь аутентифицировался в WebSSO и зарегистрирован в Банке Идей
        /// </summary>
        protected virtual bool IsAuthenticated
        {
            get { return false; }
        }

        /// <summary>
        /// Возвращает модель представления с ошибкой
        /// </summary>
        /// <typeparam name="TModel">Тип модели</typeparam>
        /// <param name="serviceError">Ошибка</param>
        /// <returns></returns>
        protected virtual TModel Error<TModel>(
            ServiceError serviceError) where TModel : ViewModelBase, new()
        {
            var result = new TModel();
            result.IsSucceeded = false;
            if (serviceError != null)
            {
                result.Error = new JsonError
                {
                    ErrorMessage = serviceError.Message
                };
            }
            else
            {
                result.Error = new JsonError
                {
                    ErrorMessage = "Произошла внутренняя ошибка."
                };
            }

            return result;
        }

        /// <summary>
        /// Возвращает модель представления с ошибкой
        /// </summary>
        /// <typeparam name="TModel">Тип модели</typeparam>
        /// <param name="errorMessage">Ошибка</param>
        /// <returns></returns>
        protected virtual TModel Error<TModel>(
            string errorMessage) where TModel : ViewModelBase, new()
        {
            var result = new TModel();
            result.IsSucceeded = false;
            result.Error = new JsonError
            {
                ErrorMessage = errorMessage
            };
            return result;
        }

        /// <summary>
        /// Возвращает модель представления с ошибкой
        /// </summary>
        /// <typeparam name="TModel">Тип модели</typeparam>
        /// <param name="serviceError">Ошибка</param>
        /// <returns></returns>
        protected virtual TModel BusinessError<TModel>(
            ServiceError serviceError) where TModel : ViewModelBase, new()
        {
            var result = new TModel();
            result.IsSucceeded = false;
            if (serviceError != null && serviceError.Type == ServiceErrorType.BusinessLogicMessage)
            {
                result.Error = new JsonError
                {
                    ErrorMessage = serviceError.Message
                };
            }
            else
            {
                result.Error = new JsonError
                {
                    ErrorMessage = "Произошла внутренняя ошибка."
                };
            }

            return result;
        }
    }
}
