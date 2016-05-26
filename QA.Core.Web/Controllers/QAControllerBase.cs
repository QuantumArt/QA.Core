// Owners: Karlov Nikolay, Abretov Alexey
using System.Web.Mvc;
using FluentValidation.Results;

namespace QA.Core.Web
{
    /// <summary>
    /// Базовый контроллер клиента, не требует инстанцирования через Unity
    /// </summary>
    [QAHandleErrorAttribute]
    public class QAControllerBase : Controller
    {
        /// <summary>
        /// Отправка информации о валидации
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isSucceeded"></param>
        /// <returns></returns>
        protected virtual JsonResult JsonValidation(JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet)
        {
            var result = new JsonServerResult<ValidationDictionary> { IsSucceeded = ModelState.IsValid };

            foreach (var key in ModelState.Keys)
            {
                if (ModelState[key].Errors.Count > 0)
                {
                    var firstError = ModelState[key].Errors[0].ErrorMessage;

                    result.Result.Add(key, firstError);
                }
            }

            return Json(result, behavior);
        }

        /// <summary>
        /// Отправка информации о валидации
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="isSucceeded"></param>
        /// <returns></returns>
        protected virtual JsonResult JsonValidation(ValidationResult results, JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet)
        {
            var result = new JsonServerResult<ValidationDictionary> { IsSucceeded = results.IsValid };

            foreach (var error in results.Errors)
            {
                if (!result.Result.ContainsKey(error.PropertyName))
                {
                    result.Result.Add(error.PropertyName, error.ErrorMessage);
                }
            }

            return Json(result, behavior);
        }

        /// <summary>
        /// Отправка стандартного ответа
        /// </summary>
        protected virtual JsonResult JsonSuccess(object data, string message, JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet)
        {
            var result = new JsonServerResult<object>
            {
                IsSucceeded = true,
                Result = data,
                Message = message
            };

            return Json(result, behavior);
        }

        /// <summary>
        /// Отправка стандартного ответа
        /// </summary>
        protected virtual JsonResult JsonError(object data, JsonError error, JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet)
        {
            var result = new JsonServerResult<object>
            {
                IsSucceeded = false,
                Result = data,
                Error = error,
                Message = error != null ?
                    error.ErrorMessage : string.Empty
            };

            return Json(result, behavior);
        }

        /// <summary>
        /// Отправка стандартного ответа
        /// </summary>
        protected virtual JsonResult JsonError(object data, string errorMessage, JsonRequestBehavior behavior = JsonRequestBehavior.DenyGet)
        {
            return JsonError(data, new JsonError
            {
                ErrorMessage = errorMessage
            }, behavior);
        }

        /// <summary>
        /// Отправка стандартного ответа
        /// </summary>
        protected virtual JsonResult JsonError(ViewModelBase viewModel)
        {
            return JsonResult(new ViewModelBase
            {
                IsSucceeded = false,
                Error = viewModel.Error
            });
        }

        /// <summary>
        /// Возвращает Json ViewModel
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        protected virtual JsonResult JsonResult(ViewModelBase viewModel)
        {
            if (!viewModel.IsSucceeded)
            {
                return Json(new ViewModelBase
                {
                    IsSucceeded = false,
                    Error = viewModel.Error
                });
            }

            return Json(viewModel);
        }

        /// <summary>
        /// Вызов глобальных фильтров типа IInitilizeFilter
        /// </summary>
        /// <param name="requestContext"></param>
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            var filters = GlobalFilters.Filters;

            foreach (var filter in filters)
            {
                if (filter.Instance is IInitilizeFilter)
                {
                    ((IInitilizeFilter)(filter.Instance)).OnInitializing(requestContext);
                }
            }

            base.Initialize(requestContext);

            foreach (var filter in filters)
            {
                if (filter.Instance is IInitilizeFilter)
                {
                    ((IInitilizeFilter)(filter.Instance)).OnInitialized(ControllerContext);
                }
            }
        }
    }
}
