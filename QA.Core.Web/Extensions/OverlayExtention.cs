using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
#pragma warning disable 1591

namespace QA.Core.Web.Extensions
{
    /// <summary>
    /// Блокирующий слой
    /// </summary>
    public class OverlayExtention : IHtmlString
    {
        private HtmlHelper _htmlHelper;

        bool showLoading = false;
        string id = "";
        string @class = "";
        string hiddenClass = "hidden";
        int? zIndex = null;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="htmlHelper"></param>
        public OverlayExtention(HtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        public OverlayExtention IsMainPageOverlay(bool isMainPageOverlay = true)
        {
            this.id = "whole-page-overlay";
            return this;
        }

        public OverlayExtention WithHiddenClass(string hiddenClass)
        {
            this.hiddenClass = hiddenClass;
            return this;
        }

        public OverlayExtention WithId(string id)
        {
            this.id = id;
            return this;
        }

        public OverlayExtention WithLoading(bool showLoading = true)
        {
            this.showLoading = showLoading;
            return this;
        }

        public OverlayExtention WithClassName(string @class)
        {
            this.@class = @class;
            return this;
        }

        public OverlayExtention WithZIndex(int zIndex)
        {
            this.zIndex = zIndex;
            return this;
        }

        public MvcHtmlString Render()
        {
            /*
             <div id="@(id ?? "")"
                 class="b-loader page-overlay @(className ?? string.Empty)
                 @HiddenIf(isHidden)"
                 style='@(zIndex.HasValue ? "z-index: " + zIndex : string.Empty)'>
                 <div class="load-overlay">
                     &nbsp;
                 </div>
             </div>
             */
            Predicate<Nullable<int>> declareZIndex = z => z.HasValue;

            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            {
                using (TagWrapper.Begin("div", writer,
                    new
                    {
                        id,
                        style = declareZIndex(zIndex) ? "z-index: " + zIndex.Value : string.Empty,
                        @class = "b-loader page-overlay" + (@class ?? string.Empty) + " " + hiddenClass,
                    }))
                {
                    using (TagWrapper.Begin("div", writer, new { @class = "load-overlay" }))
                    {
                        writer.Write(TagWrapper.NoBreakSpace);
                    }

                    if (showLoading)
                    {
                        using (TagWrapper.Begin("div", writer, new { @class = "load" }))
                        {
                            writer.Write(TagWrapper.NoBreakSpace);
                        }
                    }
                }

            }

            return MvcHtmlString.Create(sb.ToString());
        }

        #region IHtmlString Members

        string IHtmlString.ToHtmlString()
        {
            return Render().ToHtmlString();
        }

        #endregion
    }
}
