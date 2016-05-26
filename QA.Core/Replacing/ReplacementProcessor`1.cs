using System;

namespace QA.Core.Replacing
{
    /// <summary>
    /// Управление заменами полей, зависящих от установленной языковой культуры (указанными с помощью атрибутов CultureDependent и DependentValue)
    /// Данный объект следует кешировать.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class ReplacementProcessor<TModel> : IReplacementProcessor<TModel>
        where TModel : class
    {
        IReplacementProcessor processor = new SwapReplacementProcessor(typeof(TModel));

        /// <summary>
        /// Тип
        /// </summary>
        public Type TargetType
        {
            get { return typeof(TModel); }
        }

        #region IReplacementProcessor<TModel> Members

        /// <summary>
        /// Произвести замену свойств объекта
        /// </summary>
        /// <param name="obj">объект</param>
        /// <param name="cultureKey">культура ("ru-ru", ...)</param>
        /// <returns></returns>
        public TModel Process(TModel obj, string cultureKey)
        {
            return (TModel)processor.Process((TModel)obj, cultureKey);
        }

        /// <summary>
        /// Производит замену свойств объекта для текущей культуры
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public TModel Process(TModel obj)
        {
            return (TModel)processor.Process((TModel)obj);
        }

        #endregion

        #region IReplacementProcessor Members

        object IReplacementProcessor.Process(object obj, string cultureKey)
        {
            return processor.Process(obj, cultureKey);
        }

        object IReplacementProcessor.Process(object obj)
        {
            return processor.Process(obj);
        }

        #endregion
    }
}
