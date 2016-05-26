using System;
namespace QA.Core.Replacing
{
    /// <summary>
    /// Управление заменами полей, зависящих от установленной языковой культуры (указанными с помощью атрибутов CultureDependent и DependentValue)
    /// Данный объект следует кешировать.
    /// </summary>
    public interface IReplacementProcessor
    {
        /// <summary>
        /// Произвести замену свойств объекта
        /// </summary>
        /// <param name="obj">объект</param>
        /// <param name="cultureKey">культура ("ru-ru", ...)</param>
        /// <returns></returns>
        object Process(object obj, string cultureKey);
        
        /// <summary>
        /// Производит замену свойств объекта для текущей культуры
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        object Process(object obj);

        /// <summary>
        /// Тип
        /// </summary>
        Type TargetType { get; }
    }

    /// <summary>
    /// Управление заменами полей, зависящих от установленной языковой культуры (указанными с помощью атрибутов CultureDependent и DependentValue)
    /// Данный объект следует кешировать.
    /// </summary>
    public interface IReplacementProcessor<TModel> : IReplacementProcessor
        where TModel : class
    {
        /// <summary>
        /// Произвести замену свойств объекта
        /// </summary>
        /// <param name="obj">объект</param>
        /// <param name="cultureKey">культура ("ru-ru", ...)</param>
        /// <returns></returns>
        TModel Process(TModel obj, string cultureKey);
        /// <summary>
        /// Производит замену свойств объекта для текущей культуры
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        TModel Process(TModel obj);
    }
}
