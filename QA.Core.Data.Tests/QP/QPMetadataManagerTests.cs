using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QA.Core.Data.QP;

namespace QA.Core.Data.Tests.QP
{

    public class QPMetadataManagerTests
    {
        #region Constructor

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerConstructorWithNullConnectionStringTest()
        {
            new QPMetadataManager(null);
        }

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerConstructorWithEmptyConnectionStringTest()
        {
            new QPMetadataManager(string.Empty);
        }

      //  [TestMethod]
        public void QPMetadataManagerConstructorTest()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);

            Assert.IsNotNull(instance);
            Assert.IsNotNull(instance.DbConnection);
            Assert.AreEqual(TestUtils.ConnectionString, instance.DbConnection.InstanceConnectionString);
        }

      //  [TestMethod]
        public void QPMetadataManagerConstructorWithNullConnectionStringTest2()
        {
            try
            {
                new QPMetadataManager(string.Empty);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: connectionString", ex.Message);

                return;
            }

            Assert.Fail("The exception was not thrown.");
        }

      //  [TestMethod]
        public void QPMetadataManagerConstructorWithEmptyConnectionStringTest2()
        {
            try
            {
                new QPMetadataManager(string.Empty);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: connectionString", ex.Message);

                return;
            }

            Assert.Fail("The exception was not thrown.");
        }

        #endregion

        #region GetContentId

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetContentIdWithNullParametersTest()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetContentId(null, null);
        }

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetContentIdWithNullParametersTest2()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetContentId(TestUtils.SiteName, null);
        }

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetContentIdWithNullParametersTest3()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetContentId(null, "AbstractItem");
        }

      //  [TestMethod]
        public void QPMetadataManagerGetContentIdWithNullParametersTest4()
        {
            try
            {
                var instance = new QPMetadataManager(TestUtils.ConnectionString);
                instance.GetContentId(null, null);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: siteName", ex.Message);

                return;
            }

            Assert.Fail("The exception was not thrown.");
        }

      //  [TestMethod]
        public void QPMetadataManagerGetContentIdWithNullParametersTest5()
        {
            try
            {
                var instance = new QPMetadataManager(TestUtils.ConnectionString);
                instance.GetContentId(TestUtils.SiteName, null);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: contentName", ex.Message);

                return;
            }

            Assert.Fail("The exception was not thrown.");
        }

      //  [TestMethod]
        public void QPMetadataManagerGetContentIdWithNullParametersTest6()
        {
            try
            {
                var instance = new QPMetadataManager(TestUtils.ConnectionString);
                instance.GetContentId(null, "AbstractItem");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: siteName", ex.Message);

                return;
            }

            Assert.Fail("The exception was not thrown.");
        }

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetContentIdWithEmptyParametersTest()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetContentId(string.Empty, string.Empty);
        }

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetContentIdWithEmptylParametersTest2()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetContentId(TestUtils.SiteName, string.Empty);
        }

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetContentIdWithEmptyParametersTest3()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetContentId(string.Empty, "AbstractItem");
        }

      //  [TestMethod]
        public void QPMetadataManagerGetContentIdWithEmptyParametersTest4()
        {
            try
            {
                var instance = new QPMetadataManager(TestUtils.ConnectionString);
                instance.GetContentId(string.Empty, string.Empty);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: siteName", ex.Message);

                return;
            }

            Assert.Fail("The exception was not thrown.");
        }

      //  [TestMethod]
        public void QPMetadataManagerGetContentIdWithEmptyParametersTest5()
        {
            try
            {
                var instance = new QPMetadataManager(TestUtils.ConnectionString);
                instance.GetContentId(TestUtils.SiteName, string.Empty);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: contentName", ex.Message);

                return;
            }

            Assert.Fail("The exception was not thrown.");
        }

      //  [TestMethod]
        public void QPMetadataManagerGetContentIdWithEmptyParametersTest6()
        {
            try
            {
                var instance = new QPMetadataManager(TestUtils.ConnectionString);
                instance.GetContentId(string.Empty, "AbstractItem");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: siteName", ex.Message);

                return;
            }

            Assert.Fail("The exception was not thrown.");
        }

      //  [TestMethod]
        public void QPMetadataManagerGetContentIdTest()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            int id = instance.GetContentId(TestUtils.SiteName, "AbstractItem");

            Assert.AreEqual(293, id);
        }

        #endregion

        #region GetContentName

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetContentNameWithNullParametersTest()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetContentName(0);
        }

      //  [TestMethod]
        public void QPMetadataManagerGetContentNameWithNullParametersTest2()
        {
            try
            {
                var instance = new QPMetadataManager(TestUtils.ConnectionString);
                instance.GetContentName(0);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: contentId", ex.Message);

                return;
            }

            Assert.Fail("The exception was not thrown.");
        }

      //  [TestMethod]
        public void QPMetadataManagerGetContentNameTest()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            string name = instance.GetContentName(295);

            Assert.AreEqual("Culture", name);
        }

      //  [TestMethod]
        public void QPMetadataManagerGetContentNameTest_NotExists()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            string name = instance.GetContentName(1);

            Assert.AreEqual("", name);
        }

        #endregion

        #region GetSiteId

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetSiteIdWithNullParametersTest()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetSiteId(null);
        }

      //  [TestMethod]
        public void QPMetadataManagerGetSiteIdWithNullParametersTest2()
        {
            try
            {
                var instance = new QPMetadataManager(TestUtils.ConnectionString);
                instance.GetSiteId(null);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: siteName", ex.Message);

                return;
            }

            Assert.Fail("The exception was not thrown.");
        }

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetSiteIdWithEmptyParametersTest()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetSiteId(string.Empty);
        }

      //  [TestMethod]
        public void QPMetadataManagerGetSiteIdWithEmptyParametersTest2()
        {
            try
            {
                var instance = new QPMetadataManager(TestUtils.ConnectionString);
                instance.GetSiteId(string.Empty);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: siteName", ex.Message);

                return;
            }

            Assert.Fail("The exception was not thrown.");
        }

      //  [TestMethod]
        public void QPMetadataManagerGetSiteIdTest()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            int id = instance.GetSiteId(TestUtils.SiteName);

            Assert.AreEqual(35, id);
        }

      //  [TestMethod]
        public void QPMetadataManagerGetSiteIdTest_NotExists()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            int siteId = instance.GetSiteId("dsdsd");

            Assert.AreEqual(0, siteId);
        }

        #endregion

        #region GetSiteName

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetSiteNameWithNullParametersTest()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetSiteName(0);
        }

      //  [TestMethod]
        public void QPMetadataManagerGetSiteNameWithNullParametersTest2()
        {
            try
            {
                var instance = new QPMetadataManager(TestUtils.ConnectionString);
                instance.GetSiteName(0);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: siteId", ex.Message);

                return;
            }

            Assert.Fail("The exception was not thrown.");
        }

      //  [TestMethod]
        public void QPMetadataManagerGetSiteNameTest()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            string name = instance.GetSiteName(35);

            Assert.AreEqual(TestUtils.SiteName, name);
        }

      //  [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void QPMetadataManagerGetSiteNameTest_NotExists()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetSiteName(350);
        }

      //  [TestMethod]
        public void QPMetadataManagerGetSiteNameTest_NotExists_CheckExceptionText()
        {
            try
            {
                var instance = new QPMetadataManager(TestUtils.ConnectionString);
                instance.GetSiteName(350);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOfType(ex, typeof(Exception));
                Assert.AreEqual("Site (id = 350) is not found", ex.Message);

                return;
            }

            Assert.Fail("The exception was not thrown.");
        }

        #endregion

        #region GetContentAttribute

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetContentAttributeWithNullParametersTest()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetContentAttribute(null, null, null);
        }

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetContentAttributeWithNullParametersTest2()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetContentAttribute(TestUtils.SiteName, null, null);
        }

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetContentAttributeWithNullParametersTest3()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetContentAttribute(TestUtils.SiteName, "AbstractItem", null);
        }

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetContentAttributeWithNullParametersTest4()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetContentAttribute(TestUtils.SiteName, null, "Name");
        }

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetContentAttributeWithNullParametersTest5()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetContentAttribute(null, null, "Name");
        }

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetContentAttributeWithNullParametersTest6()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetContentAttribute(null, "AbstractItem", null);
        }

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetContentAttributeWithEmptyParametersTest()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetContentAttribute(string.Empty, string.Empty, string.Empty);
        }

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetContentAttributeWithEmptyParametersTest2()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetContentAttribute(TestUtils.SiteName, string.Empty, string.Empty);
        }

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetContentAttributeWithEmptyParametersTest3()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetContentAttribute(TestUtils.SiteName, "AbstractItem", string.Empty);
        }

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetContentAttributeWithEmptyParametersTest4()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetContentAttribute(TestUtils.SiteName, string.Empty, "Name");
        }

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetContentAttributeWithEmptyParametersTest5()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetContentAttribute(string.Empty, string.Empty, "Name");
        }

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetContentAttributeWithEmptyParametersTest6()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetContentAttribute(null, "AbstractItem", string.Empty);
        }

      //  [TestMethod]
        public void QPMetadataManagerGetContentAttributeWithNullParametersTest7()
        {
            try
            {
                var instance = new QPMetadataManager(TestUtils.ConnectionString);
                instance.GetContentAttribute(null, null, null);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: siteName", ex.Message);

                return;
            }

            Assert.Fail("The exception was not thrown.");
        }

      //  [TestMethod]
        public void QPMetadataManagerGetContentAttributeWithNullParametersTest8()
        {
            try
            {
                var instance = new QPMetadataManager(TestUtils.ConnectionString);
                instance.GetContentAttribute(TestUtils.SiteName, null, null);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: contentName", ex.Message);

                return;
            }

            Assert.Fail("The exception was not thrown.");
        }

      //  [TestMethod]
        public void QPMetadataManagerGetContentAttributeWithNullParametersTest9()
        {
            try
            {
                var instance = new QPMetadataManager(TestUtils.ConnectionString);
                instance.GetContentAttribute(TestUtils.SiteName, "AbstractItem", null);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: fieldName", ex.Message);

                return;
            }

            Assert.Fail("The exception was not thrown.");
        }

      //  [TestMethod]
        public void QPMetadataManagerGetContentAttributeWithEmptyParametersTest7()
        {
            try
            {
                var instance = new QPMetadataManager(TestUtils.ConnectionString);
                instance.GetContentAttribute(string.Empty, string.Empty, string.Empty);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: siteName", ex.Message);

                return;
            }

            Assert.Fail("The exception was not thrown.");
        }

      //  [TestMethod]
        public void QPMetadataManagerGetContentAttributeWithEmptyParametersTest8()
        {
            try
            {
                var instance = new QPMetadataManager(TestUtils.ConnectionString);
                instance.GetContentAttribute(TestUtils.SiteName, string.Empty, string.Empty);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: contentName", ex.Message);

                return;
            }

            Assert.Fail("The exception was not thrown.");
        }

      //  [TestMethod]
        public void QPMetadataManagerGetContentAttributeWithEmptyParametersTest9()
        {
            try
            {
                var instance = new QPMetadataManager(TestUtils.ConnectionString);
                instance.GetContentAttribute(TestUtils.SiteName, "AbstractItem", string.Empty);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: fieldName", ex.Message);

                return;
            }

            Assert.Fail("The exception was not thrown.");
        }

      //  [TestMethod]
        public void QPMetadataManagerGetContentAttributeTest()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            var attr = instance.GetContentAttribute(TestUtils.SiteName, "AbstractItem", "Name");

            Assert.IsNotNull(attr);
            Assert.AreEqual("Name", attr.Name);
            Assert.AreEqual(293, attr.ContentId);

            attr = instance.GetContentAttribute(TestUtils.SiteName, "AbstractItem", "Name1");
            Assert.IsNull(attr);
        }

        #endregion

        #region GetContentAttributes

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetContentAttributesWithNullParametersTest()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetContentAttributes(null, null);
        }

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetContentAttributesWithNullParametersTest2()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetContentAttributes(TestUtils.SiteName, null);
        }

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetContentAttributesWithNullParametersTest3()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetContentAttributes(null, "AbstractItem");
        }

      //  [TestMethod]
        public void QPMetadataManagerGetContentAttributesWithNullParametersTest4()
        {
            try
            {
                var instance = new QPMetadataManager(TestUtils.ConnectionString);
                instance.GetContentAttributes(null, null);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: siteName", ex.Message);

                return;
            }

            Assert.Fail("The exception was not thrown.");
        }

      //  [TestMethod]
        public void QPMetadataManagerGetContentAttributesWithNullParametersTest5()
        {
            try
            {
                var instance = new QPMetadataManager(TestUtils.ConnectionString);
                instance.GetContentAttributes(TestUtils.SiteName, null);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: contentName", ex.Message);

                return;
            }

            Assert.Fail("The exception was not thrown.");
        }

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetContentAttributesWithEmptyParametersTest()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetContentAttributes(string.Empty, string.Empty);
        }

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetContentAttributesWithEmptyParametersTest2()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetContentAttributes(TestUtils.SiteName, string.Empty);
        }

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetContentAttributesWithEmptyParametersTest3()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetContentAttributes(string.Empty, "AbstractItem");
        }

      //  [TestMethod]
        public void QPMetadataManagerGetContentAttributesWithEmptyParametersTest4()
        {
            try
            {
                var instance = new QPMetadataManager(TestUtils.ConnectionString);
                instance.GetContentAttributes(string.Empty, string.Empty);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: siteName", ex.Message);

                return;
            }

            Assert.Fail("The exception was not thrown.");
        }

      //  [TestMethod]
        public void QPMetadataManagerGetContentAttributesWithEmptyParametersTest5()
        {
            try
            {
                var instance = new QPMetadataManager(TestUtils.ConnectionString);
                instance.GetContentAttributes(TestUtils.SiteName, string.Empty);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: contentName", ex.Message);

                return;
            }

            Assert.Fail("The exception was not thrown.");
        }

        //[TestMethod]
        //public void QPMetadataManagerGetContentAttributesTest()
        //{
        //    var instance = new QPMetadataManager(TestUtils.ConnectionString);
        //    var attrs = instance.GetContentAttributes(TestUtils.SiteName, "AbstractItem");

        //    Assert.IsNotNull(attrs);
        //    //Assert.AreEqual(18, attrs.Count);

        //    foreach (var item in attrs)
        //    {
        //        if (item.ContentId != 293)
        //        {
        //            Assert.Fail("ContentId is incorect.");
        //        }
        //    }
        //}

        //[TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetContentAttributesNotExistsTest()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetContentAttributes(TestUtils.SiteName, "AbstractItem1");
        }

        #region ById

      //  [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void QPMetadataManagerGetContentAttributesByIdWithNullParametersTest()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            instance.GetContentAttributes(0);
        }

      //  [TestMethod]
        public void QPMetadataManagerGetContentAttributesByIdWithNullParametersTest2()
        {
            try
            {
                var instance = new QPMetadataManager(TestUtils.ConnectionString);
                instance.GetContentAttributes(0);
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: contentId", ex.Message);

                return;
            }

            Assert.Fail("The exception was not thrown.");
        }

        //[TestMethod]
        public void QPMetadataManagerGetContentAttributesByIdTest()
        {
            var instance = new QPMetadataManager(TestUtils.ConnectionString);
            var attrs = instance.GetContentAttributes(293);

            Assert.IsNotNull(attrs);
            Assert.AreEqual(18, attrs.Count);

            foreach (var item in attrs)
            {
                if (item.ContentId != 293)
                {
                    Assert.Fail("ContentId is incorrect.");
                }
            }

            attrs = instance.GetContentAttributes(int.MaxValue);
            Assert.IsNotNull(attrs);
            Assert.AreEqual(0, attrs.Count);
        }

        #endregion

        #endregion
    }
}
