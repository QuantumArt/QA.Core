
namespace QA.Core.Web.Html
{
    /// <summary>
    /// Методы расширения контрола Текст
    /// </summary>
    public static class RawTextControlBuilder
    {
        /// <summary>
        /// Создает контрол
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static RawTextControl RawText<TModel>(
            this HtmlControlFactory<TModel> helper)
        {
            var item = new RawTextControl(helper.HtmlHelper.ViewContext);

            return item;
        }

        /// <summary>
        /// Устанаваливает текст
        /// </summary>
        /// <param name="control"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static RawTextControl SetText(
            this RawTextControl control,
            string text)
        {
            control.Text = text;

            return control;
        }
    }
}
