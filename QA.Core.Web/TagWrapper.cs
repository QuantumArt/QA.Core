using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
#pragma warning disable 1591

namespace QA.Core.Web
{
    /// <summary>
    /// Класс, записывающий html-теги в поток
    /// </summary>
    public class TagWrapper : IDisposable
    {
        TagBuilder _tag;
        TextWriter _writer;

        public TagWrapper(TagBuilder tag, TextWriter writer)
        {
            _tag = tag;
            _writer = writer;

            writer.Write(tag.ToString(TagRenderMode.StartTag));
        }

        public const string NoBreakSpace = @"&nbsp;";

        //public static TagWrapper Begin(string tagName, HierarchyNode<AbstractItem> node, Action<HierarchyNode<AbstractItem>, TagBuilder> tagModifier, TextWriter writer)
        //{
        //    var tag = new TagBuilder(tagName);
        //    if (tagModifier != null)
        //        tagModifier(node, tag);

        //    return new TagWrapper(tag, writer);
        //}

        public static IDisposable Begin(string tagName, TextWriter writer, object htmlAttributes)
        {
            var tag = new TagBuilder(tagName);
            tag.MergeAttributes<string, object>(new RouteValueDictionary(htmlAttributes), true);

            return new TagWrapper(tag, writer);
        }

        public static IDisposable BeginLink(TextWriter writer, string href, object htmlAttributes)
        {
            var tag = new TagBuilder("a");
            tag.MergeAttributes(htmlAttributes);
            tag.Attributes["href"] = href;

            return new TagWrapper(tag, writer);
        }

        public static IDisposable Begin(string tagName, TextWriter writer, RouteValueDictionary htmlAttributes)
        {
            var tag = new TagBuilder(tagName);
            tag.MergeAttributes(htmlAttributes);

            return new TagWrapper(tag, writer);
        }

        public static IDisposable Begin(string tagName, TextWriter writer, IDictionary<string, object> htmlAttributes)
        {
            var tag = new TagBuilder(tagName);
            tag.MergeAttributes(htmlAttributes);

            return new TagWrapper(tag, writer);
        }

        public static TagWrapper Begin(string tagName, string id, string cssClass, TextWriter writer)
        {
            var tag = new TagBuilder(tagName);
            tag.AddAttributeUnlessEmpty("id", id);
            tag.AddAttributeUnlessEmpty("class", cssClass);

            return new TagWrapper(tag, writer);
        }

        #region IDisposable Members

        public void Dispose()
        {
            _writer.Write(_tag.ToString(TagRenderMode.EndTag));
        }

        #endregion
    }
}
