using System.Web.Mvc;

namespace QA.Core.Web.Html
{
    /// <summary>
    /// Фабрика контролов
    /// </summary>
    public class HtmlControlFactory<TModel>
    {
        #region HtmlHelper

        /// <summary>
        /// HtmlHelper страницы
        /// </summary>
        public HtmlHelper<TModel> HtmlHelper { get; set; }

        #endregion

        #region Factory

        /// <summary>
        /// Фабрика контролов
        /// </summary>
        /// <param name="helper">HtmlHelper страницы</param>
        public HtmlControlFactory(HtmlHelper<TModel> helper)
        {
            HtmlHelper = helper;
        }

        #endregion
    }
}
