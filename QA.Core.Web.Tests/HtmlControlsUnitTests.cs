using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.WebPages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QA.Core.Web.Html;

namespace QA.Core.Web.Tests
{
    [TestClass]
    public class HtmlControlsUnitTests
    {
        [TestMethod]
        public void RawTextControlTest()
        {
            var factory = new HtmlControlFactory<object>(
                new System.Web.Mvc.HtmlHelper<object>(
                    new ViewContext(), new ViewPage()));

            var control = factory.RawText();

            string str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreEqual(string.Empty, str.Trim());

            control.SetText("Test Raw Control");

            str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreEqual("Test Raw Control", str.Trim());

            control.SetText("<span>Helloooo!</span>");

            str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreEqual("<span>Helloooo!</span>", str.Trim());
        }

        [TestMethod]
        public void ImageControlTest()
        {
            var factory = new HtmlControlFactory<object>(
                new System.Web.Mvc.HtmlHelper<object>(
                    new ViewContext(), new ViewPage()));

            var image = factory.Image();

            string str = image.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<img alt=\"\" src=\"\" title=\"\" />", str);

            image.SetAlt("Hello").SetSrc("http://img1").SetTitle("Hi");

            str = image.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<img alt=\"Hello\" src=\"http://img1\" title=\"Hi\" />", str);

            image.SetId("img1");

            str = image.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<img alt=\"Hello\" id=\"img1\" name=\"img1\" src=\"http://img1\" title=\"Hi\" />", str);

            image.SetName("image1");

            str = image.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<img alt=\"Hello\" id=\"img1\" name=\"image1\" src=\"http://img1\" title=\"Hi\" />", str);

            image.SetId("img2");

            str = image.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<img alt=\"Hello\" id=\"img2\" name=\"image1\" src=\"http://img1\" title=\"Hi\" />", str);

            image.SetDisabled();

            str = image.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<img alt=\"Hello\" disabled=\"disabled\" id=\"img2\" name=\"image1\" src=\"http://img1\" title=\"Hi\" />", str);
        }

        [TestMethod]
        public void HtmlTagControl_Empty_TagType_Test()
        {
            var control = new HtmlTagControl(new ViewContext());

            string str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreEqual(string.Empty, str.Trim());
        }

        [TestMethod]
        public void Image_Render_Test()
        {
            var sb = new StringBuilder();
            var context = new ViewContext();
            context.ViewData = new ViewDataDictionary(new object());
            context.Writer = new StringWriter(sb);
            var page = new ViewPage<object>();

            var factory = new HtmlControlFactory<object>(
                new HtmlHelper<object>(context, page));

            var image = factory.Image();

            image.Render();

            string str = sb.ToString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<img alt=\"\" src=\"\" title=\"\" />", str);

            sb = new StringBuilder();
            context = new ViewContext();
            context.ViewData = new ViewDataDictionary(new object());
            context.Writer = new StringWriter(sb);
            page = new ViewPage<object>();
            factory = new HtmlControlFactory<object>(
                new HtmlHelper<object>(context, page));

            image = factory.Image();
            image.SetAlt("Hello").SetSrc("http://img1").SetTitle("Hi");

            image.Render();

            str = sb.ToString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<img alt=\"Hello\" src=\"http://img1\" title=\"Hi\" />", str);
        }

        [TestMethod]
        public void Image_ToHtmlString_Test()
        {
            var sb = new StringBuilder();
            var context = new ViewContext();
            context.ViewData = new ViewDataDictionary(new object());
            context.Writer = new StringWriter(sb);
            var page = new ViewPage<object>();

            var factory = new HtmlControlFactory<object>(
                new HtmlHelper<object>(context, page));

            var image = factory.Image();

            string str = image.ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<img alt=\"\" src=\"\" title=\"\" />", str);

            image.SetAlt("Hello").SetSrc("http://img1").SetTitle("Hi");

            str = image.ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<img alt=\"Hello\" src=\"http://img1\" title=\"Hi\" />", str);
        }

        [TestMethod]
        public void HtmlTagControl_Img_TagType_Test()
        {
            var control = new HtmlTagControl(new ViewContext());

            control.SetTagType(HtmlTextWriterTag.Img);

            string str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<img />", str);
        }

        [TestMethod]
        public void HtmlTagControl_Input_TagType_Test()
        {
            var control = new HtmlTagControl(new ViewContext());

            control.SetTagType(HtmlTextWriterTag.Input);

            string str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<input />", str);
        }

        [TestMethod]
        public void HtmlTagControl_Span_TagType_Test()
        {
            var control = new HtmlTagControl(new ViewContext());

            control.SetTagType(HtmlTextWriterTag.Span);

            string str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span></span>", str);
        }

        [TestMethod]
        public void HtmlTagControl_Span_Id_Name_Test()
        {
            var control = new HtmlTagControl(new ViewContext());

            control.SetTagType(HtmlTextWriterTag.Span);

            string str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span></span>", str);

            control.SetId("span1");

            str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span id=\"span1\" name=\"span1\"></span>", str);

            control.SetName("span111");

            str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span id=\"span1\" name=\"span111\"></span>", str);

            control.SetId("span2");

            str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span id=\"span2\" name=\"span111\"></span>", str);
        }

        [TestMethod]
        public void HtmlTagControl_Span_Disabled_Test()
        {
            var control = new HtmlTagControl(new ViewContext());

            control.SetTagType(HtmlTextWriterTag.Span);

            string str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span></span>", str);

            control.SetDisabled();

            str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span disabled=\"disabled\"></span>", str);
        }

        [TestMethod]
        public void HtmlTagControl_Span_Class_Test()
        {
            var control = new HtmlTagControl(new ViewContext());

            control.SetTagType(HtmlTextWriterTag.Span);

            string str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span></span>", str);

            control.AddClass("class1");

            str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span class=\"class1\"></span>", str);

            control.AddClass("class2");

            str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span class=\"class1 class2\"></span>", str);

            control.SetClass("class3");

            str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span class=\"class3\"></span>", str);

            control.AddClass("class4");

            str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span class=\"class3 class4\"></span>", str);
        }

        [TestMethod]
        public void HtmlTagControl_Span_InnerHtml_Test()
        {
            var control = new HtmlTagControl(new ViewContext());

            control.SetTagType(HtmlTextWriterTag.Span);

            string str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span></span>", str);

            control.SetInnerHtml("Hello");

            str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span>Hello</span>", str);

            control.SetInnerHtml("<span>Hello</span>");

            str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span><span>Hello</span></span>", str);
        }

        [TestMethod]
        public void HtmlTagControl_Span_InnerControls_Add_Test()
        {
            var control = new HtmlTagControl(new ViewContext());

            control.SetTagType(HtmlTextWriterTag.Span);

            string str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span></span>", str);

            control.Add(new HtmlTagControl(control.ViewContext)
                .SetTagType(HtmlTextWriterTag.Input));

            str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span><input /></span>", str);

            control.Add(HtmlTextWriterTag.Span, null);

            str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span><input /><span></span></span>", str);
        }

        [TestMethod]
        public void HtmlTagControl_Span_InnerControls_AddRange_Test()
        {
            var control = new HtmlTagControl(new ViewContext());

            control.SetTagType(HtmlTextWriterTag.Span);

            string str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span></span>", str);

            control.AddRange(new [] {
                new HtmlTagControl(control.ViewContext)
                    .SetTagType(HtmlTextWriterTag.Input),
                new HtmlTagControl(control.ViewContext)
                    .SetTagType(HtmlTextWriterTag.Span)
            });

            str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span><input /><span></span></span>", str);
        }

        [TestMethod]
        public void HtmlTagControl_Input_Type_Test()
        {
            var control = new HtmlTagControl(new ViewContext());

            control.SetTagType(HtmlTextWriterTag.Input);

            string str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<input />", str);

            control.SetType("checkbox");

            str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<input type=\"checkbox\" />", str);

            control.SetType("text");

            str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<input type=\"text\" />", str);
        }

        [TestMethod]
        public void HtmlTagControl_Input_Value_Test()
        {
            var control = new HtmlTagControl(new ViewContext());

            control.SetTagType(HtmlTextWriterTag.Input);

            string str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<input />", str);

            control.SetType("text");
            control.SetValue("hello");

            str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<input type=\"text\" value=\"hello\" />", str);

            control.SetValue("hello1");

            str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<input type=\"text\" value=\"hello1\" />", str);
        }

        [TestMethod]
        public void HtmlTagControl_Span_InnerHtml_Func_Test()
        {
            var control = new HtmlTagControl(new ViewContext());

            control.SetTagType(HtmlTextWriterTag.Span);

            string str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span></span>", str);

            Action<TextWriter> action = tw => tw.Write("Hello");
            var helper = new HelperResult(action);

            control.SetInnerHtml((o) => { return helper; });

            str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span>Hello</span>", str);

            action = tw => tw.Write("<span>Hello</span>");
            helper = new HelperResult(action);

            control.SetInnerHtml((o) => { return helper; });

            str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span><span>Hello</span></span>", str);
        }

        [TestMethod]
        public void HtmlTagControl_Span_Attributes_Test()
        {
            var control = new HtmlTagControl(new ViewContext());

            control.SetTagType(HtmlTextWriterTag.Span);

            string str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span></span>", str);

            control.SetHtmlAttributes(new { id = "span1", title = "span_title1" });

            str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span id=\"span1\" title=\"span_title1\"></span>", str);

            control.SetHtmlAttributes(new Dictionary<string, object> { { "id", "span2" }, { "title", "span_title2" }});

            str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span id=\"span2\" title=\"span_title2\"></span>", str);
        }

        [TestMethod]
        public void HtmlTagControl_Span_Attributes_Replace_Test()
        {
            var control = new HtmlTagControl(new ViewContext());

            control.SetTagType(HtmlTextWriterTag.Span);

            string str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span></span>", str);

            control.SetHtmlAttributes(new { id = "span1", title = "span_title1" })
                .SetId("span2");

            str = control.GetHtmlString().ToHtmlString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span id=\"span2\" name=\"span2\" title=\"span_title1\"></span>", str);
        }

        [TestMethod]
        public void HtmlTagControl_Span_BeginHtmlTag_Test()
        {
            var sb = new StringBuilder();
            var context = new ViewContext();
            context.ViewData = new ViewDataDictionary(new object());
            context.Writer = new StringWriter(sb);
            var page = new ViewPage<object>();

            var factory = new HtmlControlFactory<object>(
                new HtmlHelper<object>(context, page));

            using (factory.BeginHtmlTag(HtmlTextWriterTag.Span, false, null))
            {

            }

            string str = sb.ToString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span></span>", str);
        }

        [TestMethod]
        public void HtmlTagControl_Span_BeginHtmlTag_WithDisabled_Test()
        {
            var sb = new StringBuilder();
            var context = new ViewContext();
            context.ViewData = new ViewDataDictionary(new object());
            context.Writer = new StringWriter(sb);
            var page = new ViewPage<object>();

            var factory = new HtmlControlFactory<object>(
                new HtmlHelper<object>(context, page));

            using (factory.BeginHtmlTag(HtmlTextWriterTag.Span, true, null))
            {

            }

            string str = sb.ToString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span disabled=\"disabled\"></span>", str);
        }

        [TestMethod]
        public void HtmlTagControl_Span_BeginHtmlTagFor_Test()
        {
            var sb = new StringBuilder();
            var context = new ViewContext();
            context.ViewData = new ViewDataDictionary(new object());
            context.Writer = new StringWriter(sb);
            var page = new ViewPage<object>();

            var factory = new HtmlControlFactory<object>(
                new HtmlHelper<object>(context, page));

            using (factory.BeginHtmlTagFor(HtmlTextWriterTag.Span, x=> x, false, null))
            {

            }

            string str = sb.ToString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span></span>", str);
        }

        [TestMethod]
        public void HtmlTagControl_Span_BeginHtmlTagFor_Disabled_Test()
        {
            var sb = new StringBuilder();
            var context = new ViewContext();
            context.ViewData = new ViewDataDictionary(new object());
            context.Writer = new StringWriter(sb);
            var page = new ViewPage<object>();

            var factory = new HtmlControlFactory<object>(
                new HtmlHelper<object>(context, page));

            using (factory.BeginHtmlTagFor(HtmlTextWriterTag.Span, x => x, true, null))
            {

            }

            string str = sb.ToString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span disabled=\"disabled\"></span>", str);
        }

        [TestMethod]
        public void HtmlTagControl_Span_BeginHtmlTagFor_Value_Test()
        {
            var sb = new StringBuilder();
            var context = new ViewContext();
            context.ViewData = new ViewDataDictionary(new object());
            context.Writer = new StringWriter(sb);
            var page = new ViewPage<object>();

            var factory = new HtmlControlFactory<object>(
                new HtmlHelper<object>(context, page));

            var obj = new { val = "val1" };

            using (factory.BeginHtmlTagFor(HtmlTextWriterTag.Span, x => obj.val, false, null))
            {

            }

            string str = sb.ToString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span value=\"val1\"></span>", str);
        }

        [TestMethod]
        public void HtmlTagControl_Span_EndTag_Test()
        {
            var sb = new StringBuilder();
            var context = new ViewContext();
            context.ViewData = new ViewDataDictionary(new object());
            context.Writer = new StringWriter(sb);
            var page = new ViewPage<object>();

            var factory = new HtmlControlFactory<object>(
                new HtmlHelper<object>(context, page));

            var control = factory.BeginHtmlTag(HtmlTextWriterTag.Span, false, null);
            string str = sb.ToString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span>", str);

            control.EndTag();

            str = sb.ToString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span></span>", str);
        }

        [TestMethod]
        public void HtmlTagControl_Generic_Span_Model_Test()
        {
            var sb = new StringBuilder();
            var context = new ViewContext();
            context.ViewData = new ViewDataDictionary(new object());
            context.Writer = new StringWriter(sb);
            var page = new ViewPage();
            page.ViewData = context.ViewData;

            var factory = new HtmlControlFactory<object>(
                new HtmlHelper<object>(context, page));

            var control = factory.BeginHtmlTag(HtmlTextWriterTag.Span, false, null);

            Assert.IsNotNull(control.Model);
            Assert.IsInstanceOfType(control.Model, typeof(object));
        }

        [TestMethod]
        public void HtmlTagControl_Span_HtmlTag_Test()
        {
            var sb = new StringBuilder();
            var context = new ViewContext();
            context.ViewData = new ViewDataDictionary(new object());
            context.Writer = new StringWriter(sb);
            var page = new ViewPage();
            page.ViewData = context.ViewData;

            var factory = new HtmlControlFactory<object>(
                new HtmlHelper<object>(context, page));

            var control = factory.HtmlTag(HtmlTextWriterTag.Span, null);

            Assert.IsNotNull(control);
            Assert.AreEqual(HtmlTextWriterTag.Span, control.TagType);

            string str = sb.ToString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span></span>", str);
        }

        [TestMethod]
        public void HtmlTagControl_Span_HtmlTagFor_Test()
        {
            var sb = new StringBuilder();
            var context = new ViewContext();
            context.ViewData = new ViewDataDictionary(new object());
            context.Writer = new StringWriter(sb);
            var page = new ViewPage();
            page.ViewData = context.ViewData;

            var factory = new HtmlControlFactory<object>(
                new HtmlHelper<object>(context, page));

            var control = factory.HtmlTagFor(HtmlTextWriterTag.Span, x => (object)null, null);

            Assert.IsNotNull(control);
            Assert.AreEqual(HtmlTextWriterTag.Span, control.TagType);

            string str = sb.ToString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span></span>", str);
        }

        [TestMethod]
        public void HtmlTagControl_Span_HtmlTagFor_Value_Test()
        {
            var sb = new StringBuilder();
            var context = new ViewContext();
            context.ViewData = new ViewDataDictionary(new object());
            context.Writer = new StringWriter(sb);
            var page = new ViewPage();
            page.ViewData = context.ViewData;

            var factory = new HtmlControlFactory<object>(
                new HtmlHelper<object>(context, page));

            var obj = new { val = "val1" };
            var control = factory.HtmlTagFor(HtmlTextWriterTag.Span, x => obj.val, null);

            Assert.IsNotNull(control);
            Assert.AreEqual(HtmlTextWriterTag.Span, control.TagType);

            string str = sb.ToString();

            Assert.IsNotNull(str);
            Assert.AreNotEqual(string.Empty, str.Trim());
            Assert.AreEqual("<span value=\"val1\"></span>", str);
        }

        [TestMethod]
        public void HtmlTagControl_Span_RenderTag_Test()
        {
            var str = HtmlControlExtensions.RenderTag(HtmlTextWriterTag.Input, null, "Koo-Koo", false, TagRenderMode.SelfClosing);

            string html = str.ToHtmlString();

            Assert.IsNotNull(html);
            Assert.AreNotEqual(string.Empty, html);
            Assert.AreEqual("<input value=\"Koo-Koo\" />", html);

            str = HtmlControlExtensions.RenderTag(HtmlTextWriterTag.Input, null, "Koo-Koo", true, TagRenderMode.SelfClosing);

            html = str.ToHtmlString();

            Assert.IsNotNull(html);
            Assert.AreNotEqual(string.Empty, html);
            Assert.AreEqual("<input disabled=\"disabled\" value=\"Koo-Koo\" />", html);

            str = HtmlControlExtensions.RenderTag(HtmlTextWriterTag.Input, null, "Koo-Koo", true, TagRenderMode.Normal);

            html = str.ToHtmlString();

            Assert.IsNotNull(html);
            Assert.AreNotEqual(string.Empty, html);
            Assert.AreEqual("<input disabled=\"disabled\" value=\"Koo-Koo\"></input>", html);

            str = HtmlControlExtensions.RenderTag(HtmlTextWriterTag.Input, null, "Koo-Koo", true, TagRenderMode.StartTag);

            html = str.ToHtmlString();

            Assert.IsNotNull(html);
            Assert.AreNotEqual(string.Empty, html);
            Assert.AreEqual("<input disabled=\"disabled\" value=\"Koo-Koo\">", html);

            str = HtmlControlExtensions.RenderTag(HtmlTextWriterTag.Input, null, "Koo-Koo", true, TagRenderMode.EndTag);

            html = str.ToHtmlString();

            Assert.IsNotNull(html);
            Assert.AreNotEqual(string.Empty, html);
            Assert.AreEqual("</input>", html);

            str = HtmlControlExtensions.RenderTag(HtmlTextWriterTag.Input, new { id = "input1"}, "Koo-Koo", true, TagRenderMode.SelfClosing);

            html = str.ToHtmlString();

            Assert.IsNotNull(html);
            Assert.AreNotEqual(string.Empty, html);
            Assert.AreEqual("<input disabled=\"disabled\" id=\"input1\" value=\"Koo-Koo\" />", html);
        }
    }
}
