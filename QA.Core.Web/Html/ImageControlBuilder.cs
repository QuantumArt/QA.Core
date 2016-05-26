
namespace QA.Core.Web.Html
{
    /// <summary>
    /// Методы расширения контрола картинка
    /// </summary>
    public static class ImageControlBuilder
    {
        /// <summary>
        /// Создает контрол
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static ImageControl Image<TModel>(
            this HtmlControlFactory<TModel> helper)
        {
            var item = new ImageControl(helper.HtmlHelper.ViewContext);

            return item;
        }

        /// <summary>
        /// Устанаваливает Url картинки
        /// </summary>
        /// <param name="control"></param>
        /// <param name="src"></param>
        /// <returns></returns>
        public static ImageControl SetSrc(
            this ImageControl control,
            string src)
        {
            control.Src = src;

            return control;
        }

        /// <summary>
        /// Устанаваливает альтернативный текст
        /// </summary>
        /// <param name="control"></param>
        /// <param name="alt"></param>
        /// <returns></returns>
        public static ImageControl SetAlt(
            this ImageControl control,
            string alt)
        {
            control.Alt = alt;

            return control;
        }

        /// <summary>
        /// Устанавливает всплывающую подсказку
        /// </summary>
        /// <param name="control"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static ImageControl SetTitle(
            this ImageControl control,
            string title)
        {
            control.Title = title;

            return control;
        }
    }
}
