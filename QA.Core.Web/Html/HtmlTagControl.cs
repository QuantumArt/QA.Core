using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace QA.Core.Web.Html
{
    /// <summary>
    /// Html control
    /// </summary>
    public class HtmlTagControl : IDisposable, IHtmlString
    {
        #region Properties

        /// <summary>
        /// Тип Html элемента
        /// </summary>
        public virtual HtmlTextWriterTag TagType { get; set; }

        /// <summary>
        /// Контекст представления
        /// </summary>
        public virtual ViewContext ViewContext { get; set; }

        /// <summary>
        /// Список внутренних контролов
        /// </summary>
        public virtual List<HtmlTagControl> Controls { get; set; }

        /// <summary>
        /// Дополнительные атрибуты
        /// </summary>
        public virtual object HtmlAttributes { get; set; }

        /// <summary>
        /// Внутренний Html контрола
        /// </summary>
        public virtual string InnerHtml { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public virtual string Value { get; set; }

        /// <summary>
        /// Признак недоступности
        /// </summary>
        public virtual bool IsDisabled { get; set; }

        /// <summary>
        /// Тип Input
        /// </summary>
        public virtual string Type { get; set; }

        /// <summary>
        /// Идентификатор
        /// </summary>
        public virtual string Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Стиль
        /// </summary>
        public virtual string CssClass { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Конструирует объект
        /// </summary>
        /// <param name="viewContext"></param>
        public HtmlTagControl(
            ViewContext viewContext)
        {
            Throws.IfArgumentNull(viewContext, _ => viewContext);

            ViewContext = viewContext;
            Controls = new List<HtmlTagControl>();
        }

        #endregion

        #region Disposing

        /// <summary>
        /// Признак утилизации
        /// </summary>
        protected bool _disposed;

        /// <summary>
        /// Утилизирует объект
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Утилизирует объект
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                HtmlControlExtensions.EndHtmlTag(ViewContext, TagType);
            }
        }

        #endregion

        #region Html

        /// <summary>
        /// Отрисовывает закрывающейся тэг
        /// </summary>
        public virtual void EndTag()
        {
            Dispose(true);
        }

        /// <summary>
        /// Отрисовывает контрол
        /// </summary>
        protected virtual void WriteHtml()
        {
            ViewContext.Writer.Write(GetHtmlString().ToHtmlString());
        }

        /// <summary>
        /// Вызывается до генерации Html строки
        /// </summary>
        protected virtual void OnBeforeGetHtmlString()
        {
        }

        /// <summary>
        /// Возвращает Html контрола
        /// </summary>
        /// <returns></returns>
        public virtual MvcHtmlString GetHtmlString()
        {
            OnBeforeGetHtmlString();
            return HtmlControlHelper.GetHtmlString(this);
        }

        /// <summary>
        /// Отрисовывает контрол
        /// </summary>
        public virtual void Render()
        {
            if (!IsRender)
            {
                WriteHtml();
                IsRender = true;
            }
        }

        protected bool IsRender { get; set; }

        #endregion

        /// <summary>
        /// Контрол в строку
        /// </summary>
        /// <returns></returns>
        public string ToHtmlString()
        {
            return GetHtmlString().ToHtmlString();
        }
    }
}
