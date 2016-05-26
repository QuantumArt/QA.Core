using System;
using System.Web.Mvc;

namespace QA.Core.Web.Html
{
    /// <summary>
    /// Html control
    /// </summary>
    public class HtmlTagControl<TModel> : HtmlTagControl
    {
        #region Constructors

        /// <summary>
        /// Конструирует объект
        /// </summary>
        /// <param name="viewContext"></param>
        public HtmlTagControl(
            ViewContext viewContext)
            : base(viewContext)
        {
        }

        #endregion

        #region Factory

        /// <summary>
        /// Ссылка на фабрику
        /// </summary>
        public HtmlControlFactory<TModel> Factory { get; set; }

        /// <summary>
        /// Ссылка на модель
        /// </summary>
        public TModel Model
        {
            get { return Factory.HtmlHelper.ViewData.Model; }
        }

        #endregion
    }
}
