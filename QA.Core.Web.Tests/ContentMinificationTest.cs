// Owners: Karlov Nikolay, Abretov Alexey

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using Moq;
using System.Web.Routing;

namespace QA.Core.Web.Tests
{
    /// <summary>
    /// Тестирование механизма версионирования контента
    /// </summary>
    [TestClass()]
    public class ContentMinificationTest
    {
        #region Additional test attributes

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            RouteTable.Routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}", // URL with parameters
                new { controller = "Home", action = "Index" } // Parameter defaults
            );
        }

        private static UrlHelper GetUrlHelper(bool isDebuggingEnabled)
        {
            UrlHelper urlhelper;
            var _urlHelperMock = new Mock<RequestContext>();

            urlhelper = new UrlHelper(_urlHelperMock.Object);

            _urlHelperMock.Setup(x => x.HttpContext.IsDebuggingEnabled)
                .Returns(isDebuggingEnabled);
            return urlhelper;
        }

        #endregion
        
        [TestMethod()]
        public void Test_MinifiedContent_Versioning_InReleaseMode()
        {
            bool isDebug = false;

            UrlHelper urlHelper = GetUrlHelper(isDebug);

            ContentMinification.ShouldMinify = false;
            ContentMinification.IgnoreScripts = true;
            ContentMinification.IgnoreCSS = true;
            ContentMinification.UseVersioning = true;

            VersionProvider.SetVersionProvider(() => "1.12.233.32");

            string path = "content/site.js";
            string expected = "versioned/1.12.233.32/content/site.js";
            string actual;

            actual = ContentMinification.MinifiedContent(urlHelper, path);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void Test_MinifiedContent_Versioning_And_Minification_InReleaseMode()
        {
            bool isDebug = false;

            UrlHelper urlHelper = GetUrlHelper(isDebug);

            ContentMinification.ShouldMinify = true;
            ContentMinification.IgnoreScripts = false;
            ContentMinification.IgnoreCSS = false;
            ContentMinification.UseVersioning = true;

            VersionProvider.SetVersionProvider(() => "1.12.233.32");

            string path = "content/site.js";
            string expected = "versioned/1.12.233.32/content/site.min.js";
            string actual;

            actual = urlHelper.MinifiedContent(path);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void Test_MinifiedContent_Versioning_And_Minification_InDebugMode()
        {
            bool isDebug = true;

            UrlHelper urlHelper = GetUrlHelper(isDebug);

            ContentMinification.ShouldMinify = true;
            ContentMinification.IgnoreScripts = false;
            ContentMinification.IgnoreCSS = false;
            ContentMinification.UseVersioning = true;

            VersionProvider.SetVersionProvider(() => "11223332");

            string path = "content/site.js";
            string expected = "content/site.min.js?version=11223332";
            string actual;

            actual = ContentMinification.MinifiedContent(urlHelper, path);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void Test_MinifiedContent_Versioning_InDebugMode()
        {
            bool isDebug = true;

            UrlHelper urlHelper = GetUrlHelper(isDebug);

            ContentMinification.ShouldMinify = false;
            ContentMinification.IgnoreScripts = true;
            ContentMinification.IgnoreCSS = true;
            ContentMinification.UseVersioning = true;

            VersionProvider.SetVersionProvider(() => "11223332");

            string path = "content/site.js";
            string expected = "content/site.js?version=11223332";
            string actual;

            actual = ContentMinification.MinifiedContent(urlHelper, path);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void Test_MinifiedContent_Should_Minify_Scripts_And_Styles()
        {
            bool isDebug = true;

            UrlHelper urlHelper = GetUrlHelper(isDebug);

            ContentMinification.ShouldMinify = true;
            ContentMinification.IgnoreScripts = false;
            ContentMinification.IgnoreCSS = false;
            ContentMinification.UseVersioning = false;

            string path = "vm.mts.ru/content/site.js";
            string expected = "vm.mts.ru/content/site.min.js";

            Assert.AreEqual(expected, ContentMinification.MinifiedContent(urlHelper, path));

            path = "vm.mts.ru/content/site.min.js";
            expected = "vm.mts.ru/content/site.min.min.js";

            Assert.AreEqual(expected, ContentMinification.MinifiedContent(urlHelper, path));

            path = "vm.mts.ru/content/site.js.js";
            expected = "vm.mts.ru/content/site.js.min.js";

            Assert.AreEqual(expected, ContentMinification.MinifiedContent(urlHelper, path));

            path = "vm.mts.ru/content/site.css";
            expected = "vm.mts.ru/content/site.min.css";

            Assert.AreEqual(expected, ContentMinification.MinifiedContent(urlHelper, path));
        }

        [TestMethod()]
        public void Test_MinifiedContent_Should_Not_Minify_Not_Scripts()
        {
            bool isDebug = true;

            UrlHelper urlHelper = GetUrlHelper(isDebug);

            ContentMinification.ShouldMinify = true;
            ContentMinification.IgnoreScripts = false;
            ContentMinification.IgnoreCSS = false;
            ContentMinification.UseVersioning = false;

            string path = "vm.mts.ru/content/site.jms";
            string expected = "vm.mts.ru/content/site.jms";

            Assert.AreEqual(expected, ContentMinification.MinifiedContent(urlHelper, path));

            path = "vm.mts.ru/content/img.png";
            expected = "vm.mts.ru/content/img.png";

            Assert.AreEqual(expected, ContentMinification.MinifiedContent(urlHelper, path));
        }

        [TestMethod()]
        public void Test_MinifiedContent_Should_Not_Minify_By_Config()
        {
            bool isDebug = true;

            UrlHelper urlHelper = GetUrlHelper(isDebug);

            ContentMinification.ShouldMinify = true;
            ContentMinification.IgnoreScripts = true;
            ContentMinification.IgnoreCSS = true;
            ContentMinification.UseVersioning = false;

            string path = "vm.mts.ru/content/site.js";
            string expected = "vm.mts.ru/content/site.js";

            Assert.AreEqual(expected, ContentMinification.MinifiedContent(urlHelper, path));

            path = "vm.mts.ru/content/img.css";
            expected = "vm.mts.ru/content/img.css";

            Assert.AreEqual(expected, ContentMinification.MinifiedContent(urlHelper, path));

            ContentMinification.ShouldMinify = false;
            ContentMinification.IgnoreScripts = false;
            ContentMinification.IgnoreCSS = false;
            ContentMinification.UseVersioning = false;

            path = "vm.mts.ru/content/site.js";
            expected = "vm.mts.ru/content/site.js";

            Assert.AreEqual(expected, ContentMinification.MinifiedContent(urlHelper, path));

            path = "vm.mts.ru/content/img.css";
            expected = "vm.mts.ru/content/img.css";

            Assert.AreEqual(expected, ContentMinification.MinifiedContent(urlHelper, path));
        }
    }
}
