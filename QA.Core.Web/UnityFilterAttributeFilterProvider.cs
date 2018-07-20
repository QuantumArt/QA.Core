// Owners: Alexey Abretov, Nikolay Karlov

using System.Collections.Generic;
using System.Web.Mvc;
using Unity;

namespace QA.Core.Web
{
    /// <summary>
    /// Провайдер атрибутов
    /// </summary>
    public class UnityFilterAttributeFilterProvider : FilterAttributeFilterProvider
    {
        /// <summary>
        /// Контейнер объектов
        /// </summary>
        private IUnityContainer _container;

        /// <summary>
        /// Конструирует класс с контейнером объектов
        /// </summary>
        /// <param name="_container">Unity-контейнер</param>
        public UnityFilterAttributeFilterProvider(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Обрабатывает атрибуты примененные к контроллерам
        /// </summary>
        /// <param name="controllerContext">Контекст контроллера</param>
        /// <param name="actionDescriptor">Описание действия</param>
        /// <returns>Коллекция атрибутов</returns>
        protected override IEnumerable<FilterAttribute> GetControllerAttributes(
            ControllerContext controllerContext,
            ActionDescriptor actionDescriptor)
        {
            var attributes = base.GetControllerAttributes(
                controllerContext,
                actionDescriptor);

            foreach (var attribute in attributes)
            {
                _container.BuildUp(attribute.GetType(), attribute);
            }

            return attributes;
        }

        /// <summary>
        /// Обрабатывает атрибуты примененные к действиям контроллера
        /// </summary>
        /// <param name="controllerContext">Контекст контроллера</param>
        /// <param name="actionDescriptor">Описание действия</param>
        /// <returns>Коллекция атрибутов</returns>
        protected override IEnumerable<FilterAttribute> GetActionAttributes(
            ControllerContext controllerContext,
            ActionDescriptor actionDescriptor)
        {
            var attributes = base.GetActionAttributes(
                controllerContext,
                actionDescriptor);

            foreach (var attribute in attributes)
            {
                _container.BuildUp(attribute.GetType(), attribute);
            }

            return attributes;
        }
    }

}
