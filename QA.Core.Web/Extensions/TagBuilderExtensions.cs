using System.Web.Mvc;
using System.Web.Routing;
#pragma warning disable 1591

namespace QA.Core.Web
{
    public static  class TagBuilderExtensions
    {
        public static void MergeAttributes(this TagBuilder tag, object htmlAttributes)
        {
            if (htmlAttributes == null)
                return;
            tag.MergeAttributes(new RouteValueDictionary(htmlAttributes));
        }

        public static TagBuilder AddAttributeUnlessEmpty(this TagBuilder tag, string attribute, object value)
        {
            if (value == null)
                return tag;

            return tag.AddAttributeUnlessEmpty(attribute, value.ToString());
        }

        public static TagBuilder AddAttributeUnlessEmpty(this TagBuilder tag, string attribute, string value)
        {
            if (string.IsNullOrEmpty(value))
                return tag;

            tag.Attributes[attribute] = value;

            return tag;
        }
    }
}
