using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QA.Core.Data.QP;

namespace QA.Core.Data.Tests.QP
{
    // [TestClass] /* commented by KarlovN */
    public class QPContentManagerTests
    {
        #region Settings tests

       //  [TestMethod] /* commented by KarlovN */
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetContentByDefaultQuery_Get()
        {
            new QPContentManager(null, null)
                .Get();
        }

       //  [TestMethod] /* commented by KarlovN */
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetContentByDefaultQuery_Archive()
        {
            new QPContentManager(null, null)
                .Archive();
        }

       //  [TestMethod] /* commented by KarlovN */
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetContentByDefaultQuery_Restore()
        {
            new QPContentManager(null, null)
                .Restore();
        }

       //  [TestMethod] /* commented by KarlovN */
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetContentByDefaultQuery_Delete()
        {
            new QPContentManager(null, null)
                .Delete();
        }

       //  [TestMethod] /* commented by KarlovN */
        public void GetContentByDefaultQuery2()
        {
            try
            {
                new QPContentManager(null, null)
                    .Get();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: DbConnection", ex.Message);
            }

            try
            {
                new QPContentManager(null, null)
                    .Archive();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: DbConnection", ex.Message);
            }

            try
            {
                new QPContentManager(null, null)
                    .Restore();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: DbConnection", ex.Message);
            }

            try
            {
                new QPContentManager(null, null)
                    .Delete();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: DbConnection", ex.Message);
            }
        }

       //  [TestMethod] /* commented by KarlovN */
        public void ConfigureDatabaseManagerWithEmptyConnection()
        {
            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(string.Empty)
                    .Get();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: connectionString", ex.Message);
            }

            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(string.Empty)
                    .Archive();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: connectionString", ex.Message);
            }

            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(string.Empty)
                    .Restore();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: connectionString", ex.Message);
            }

            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(string.Empty)
                    .Delete();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: connectionString", ex.Message);
            }
        }

       //  [TestMethod] /* commented by KarlovN */
        public void ConfigureDatabaseManagerWithEmptyConnection2()
        {
            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(null)
                    .Get();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: connectionString", ex.Message);
            }

            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(null)
                    .Archive();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: connectionString", ex.Message);
            }

            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(null)
                    .Delete();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: connectionString", ex.Message);
            }

            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(null)
                    .Restore();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: connectionString", ex.Message);
            }
        }

       //  [TestMethod] /* commented by KarlovN */
        public void ConfigureDatabaseManagerWithEmptySiteName()
        {
            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(TestUtils.ConnectionString)
                    .Get();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: Query.SiteName", ex.Message);
            }

            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(TestUtils.ConnectionString)
                    .Archive();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: Query.SiteName", ex.Message);
            }

            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(TestUtils.ConnectionString)
                    .Restore();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: Query.SiteName", ex.Message);
            }

            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(TestUtils.ConnectionString)
                    .Delete();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: Query.SiteName", ex.Message);
            }
        }

       //  [TestMethod] /* commented by KarlovN */
        public void ConfigureDatabaseManagerWithEmptySiteName1()
        {
            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(TestUtils.ConnectionString)
                    .SiteName(string.Empty)
                    .Get();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: siteName", ex.Message);
            }

            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(TestUtils.ConnectionString)
                    .SiteName(string.Empty)
                    .Archive();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: siteName", ex.Message);
            }

            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(TestUtils.ConnectionString)
                    .SiteName(string.Empty)
                    .Restore();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: siteName", ex.Message);
            }

            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(TestUtils.ConnectionString)
                    .SiteName(string.Empty)
                    .Delete();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: siteName", ex.Message);
            }
        }

       //  [TestMethod] /* commented by KarlovN */
        public void ConfigureDatabaseManagerWithEmptySiteName2()
        {
            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(TestUtils.ConnectionString)
                    .SiteName(null)
                    .Get();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: siteName", ex.Message);
            }

            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(TestUtils.ConnectionString)
                    .SiteName(null)
                    .Restore();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: siteName", ex.Message);
            }

            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(TestUtils.ConnectionString)
                    .SiteName(null)
                    .Archive();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: siteName", ex.Message);
            }

            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(TestUtils.ConnectionString)
                    .SiteName(null)
                    .Delete();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: siteName", ex.Message);
            }
        }

       //  [TestMethod] /* commented by KarlovN */
        public void ConfigureDatabaseManagerWithEmptyContentName()
        {
            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(TestUtils.ConnectionString)
                    .SiteName(TestUtils.SiteName)
                    .Get();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: Query.ContentName", ex.Message);
            }

            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(TestUtils.ConnectionString)
                    .SiteName(TestUtils.SiteName)
                    .Archive();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: Query.ContentName", ex.Message);
            }

            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(TestUtils.ConnectionString)
                    .SiteName(TestUtils.SiteName)
                    .Delete();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: Query.ContentName", ex.Message);
            }

            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(TestUtils.ConnectionString)
                    .SiteName(TestUtils.SiteName)
                    .Restore();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: Query.ContentName", ex.Message);
            }
        }

       //  [TestMethod] /* commented by KarlovN */
        public void ConfigureDatabaseManagerWithEmptyContentName1()
        {
            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(TestUtils.ConnectionString)
                    .SiteName(TestUtils.SiteName)
                    .ContentName(string.Empty)
                    .Get();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: contentName", ex.Message);
            }

            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(TestUtils.ConnectionString)
                    .SiteName(TestUtils.SiteName)
                    .ContentName(string.Empty)
                    .Delete();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: contentName", ex.Message);
            }

            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(TestUtils.ConnectionString)
                    .SiteName(TestUtils.SiteName)
                    .ContentName(string.Empty)
                    .Restore();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: contentName", ex.Message);
            }

            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(TestUtils.ConnectionString)
                    .SiteName(TestUtils.SiteName)
                    .ContentName(string.Empty)
                    .Archive();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: contentName", ex.Message);
            }
        }

       //  [TestMethod] /* commented by KarlovN */
        public void ConfigureDatabaseManagerWithEmptyContentName2()
        {
            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(TestUtils.ConnectionString)
                    .SiteName(TestUtils.SiteName)
                    .ContentName(null)
                    .Get();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: contentName", ex.Message);
            }

            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(TestUtils.ConnectionString)
                    .SiteName(TestUtils.SiteName)
                    .ContentName(null)
                    .Archive();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: contentName", ex.Message);
            }

            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(TestUtils.ConnectionString)
                    .SiteName(TestUtils.SiteName)
                    .ContentName(null)
                    .Restore();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: contentName", ex.Message);
            }

            try
            {
                new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                    .Connection(TestUtils.ConnectionString)
                    .SiteName(TestUtils.SiteName)
                    .ContentName(null)
                    .Delete();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: contentName", ex.Message);
            }
        }

       //  [TestMethod] /* commented by KarlovN */
        public void ConfigureField()
        {
            var result = new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                .Connection(TestUtils.ConnectionString)
                .SiteName(TestUtils.SiteName)
                .ContentName("AbstractItem")
                .Fields("*");

            Assert.IsNotNull(result);
            Assert.AreEqual("*", result.Query.Fields);

            result.Fields("[name]");

            Assert.AreEqual("*,[name]", result.Query.Fields);

            result.Fields(",[id]");

            Assert.AreEqual("*,[name],[id]", result.Query.Fields);

            result.Fields(",[id2],");

            Assert.AreEqual("*,[name],[id],[id2]", result.Query.Fields);

            result.Fields("[id3]");

            Assert.AreEqual("*,[name],[id],[id2],[id3]", result.Query.Fields);
        }

       //  [TestMethod] /* commented by KarlovN */
        public void ConfigureQuery()
        {
            var result = new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                .Connection(TestUtils.ConnectionString)
                .SiteName(TestUtils.SiteName)
                .ContentName("AbstractItem")
                .IsCacheResult(true)
                .IsIncludeArchive(true)
                .CacheInterval(50)
                .IsResetCache(true)
                .IsShowSplittedArticle(true)
                .IsUseClientSelection(true)
                .IsUseSchedule(true)
                .OrderBy("order by fields")
                .Where("where by")
                .StatusName(ContentItemStatus.Created)
                .StartIndex(1)
                .PageSize(20)
                .Fields("*");

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Query);
            Assert.AreEqual(TestUtils.SiteName, result.Query.SiteName);
            Assert.AreEqual("AbstractItem", result.Query.ContentName);
            Assert.AreEqual(true, result.Query.CacheResult);
            Assert.AreEqual((double)50, result.Query.CacheInterval);
            Assert.AreEqual(true, result.Query.IncludeArchive == 1 ? true : false);
            Assert.AreEqual(true, result.Query.WithReset);
            Assert.AreEqual(true, result.Query.ShowSplittedArticle == 1 ? true : false);
            Assert.AreEqual(true, result.Query.UseClientSelection);
            Assert.AreEqual(true, result.Query.UseSchedule == 1 ? true : false);
            Assert.AreEqual("order by fields", result.Query.OrderBy);
            Assert.AreEqual("where by", result.Query.Where);
            Assert.AreEqual(ContentItemStatus.Created.GetDescription(), result.Query.StatusName);
            Assert.AreEqual(1, result.Query.StartRow);
            Assert.AreEqual(20, result.Query.PageSize);
            Assert.AreEqual("*", result.Query.Fields);

            result
                 .Connection(TestUtils.ConnectionString)
                 .SiteName(TestUtils.SiteName)
                 .ContentName("AbstractItem")
                 .IsCacheResult(false)
                 .IsIncludeArchive(false)
                 .IsResetCache(false)
                 .IsShowSplittedArticle(false)
                 .IsUseClientSelection(false)
                 .IsUseSchedule(false)
                 .OrderBy("order by fields 1")
                 .Where("where by 1")
                 .StatusName(ContentItemStatus.Published)
                 .StartIndex(0)
                 .PageSize(0)
                 .Fields("[name]");

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Query);
            Assert.AreEqual(TestUtils.SiteName, result.Query.SiteName);
            Assert.AreEqual("AbstractItem", result.Query.ContentName);
            Assert.AreEqual(false, result.Query.CacheResult);
            Assert.AreEqual(false, result.Query.IncludeArchive == 1 ? true : false);
            Assert.AreEqual(false, result.Query.WithReset);
            Assert.AreEqual(false, result.Query.ShowSplittedArticle == 1 ? true : false);
            Assert.AreEqual(false, result.Query.UseClientSelection);
            Assert.AreEqual(false, result.Query.UseSchedule == 1 ? true : false);
            Assert.AreEqual("order by fields 1", result.Query.OrderBy);
            Assert.AreEqual("where by 1", result.Query.Where);
            Assert.AreEqual(ContentItemStatus.Published.GetDescription(), result.Query.StatusName);
            Assert.AreEqual(0, result.Query.StartRow);
            Assert.AreEqual(0, result.Query.PageSize);
            Assert.AreEqual("*,[name]", result.Query.Fields);
        }

        #endregion

        #region Data tests

       //  [TestMethod] /* commented by KarlovN */
        public void GetContent()
        {
            var result = new QPContentManager(TestUtils.RealDbConnector, TestUtils.RealContentItem)
                .Connection(TestUtils.ConnectionString)
                .SiteName(TestUtils.SiteName)
                .ContentName("AbstractItem")
                .StatusName(ContentItemStatus.None)
                .Fields("*")
                .Where("[Title] = 'QPContentManagerTests_GetContent'")
                .Get();

            foreach (DataRow row in result.PrimaryContent.Rows)
            {
                TestUtils.RealContentItem.Remove(int.Parse(row["CONTENT_ITEM_ID"].ToString()), TestUtils.RealDbConnector);
            }

            var item = TestUtils.RealContentItem.New(TestUtils.AbstractItemContentId,
                TestUtils.RealDbConnector);
            item.FieldValues["Title"].Data = "QPContentManagerTests_GetContent";
            item.FieldValues["Discriminator"].Data = "55070";
            item.FieldValues["IsVisible"].Data = "1";
            item.FieldValues["IsInSiteMap"].Data = "0";

            item.Save();

            result = new QPContentManager(TestUtils.RealDbConnector, TestUtils.RealContentItem)
                .Connection(TestUtils.ConnectionString)
                .SiteName(TestUtils.SiteName)
                .ContentName("AbstractItem")
                .StatusName(ContentItemStatus.None)
                .Fields("*")
                .Where("[CONTENT_ITEM_ID] = " + item.Id)
                .Get();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.PrimaryContent);
            Assert.AreEqual(1, result.PrimaryContent.Rows.Count);

            TestUtils.RealContentItem.Remove(item.Id, TestUtils.RealDbConnector);
        }

       //  [TestMethod] /* commented by KarlovN */
        public void ContentResult_Get_NoExists_Content()
        {
            var result = new QPContentManager(TestUtils.RealDbConnector, TestUtils.RealContentItem)
               .Connection(TestUtils.ConnectionString)
               .SiteName(TestUtils.SiteName)
               .ContentName("AbstractItem")
               .StatusName(ContentItemStatus.None)
               .Fields("*")
               .Where("[Title] like '%QPContentManagerTests_EmptyResult%'")
               .Get();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.PrimaryContent);

            var getResult = result.GetContent("Culture22");
            Assert.IsNull(getResult);
        }

       //  [TestMethod] /* commented by KarlovN */
        public void ContentResult_NoContents()
        {
            var result = new QPContentManager(TestUtils.RealDbConnector, TestUtils.RealContentItem)
               .Connection(TestUtils.ConnectionString)
               .SiteName(TestUtils.SiteName)
               .ContentName("AbstractItem")
               .StatusName(ContentItemStatus.None)
               .Fields("*")
               .Where("[Title] like '%QPContentManagerTests_EmptyResult%'")
               .Get();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.PrimaryContent);

            var getResult = result.GetContent("AbstractItem");
            Assert.IsNotNull(getResult);

            result.Contents.Clear();

            getResult = result.GetContent("AbstractItem");
            Assert.IsNull(getResult);
        }

       //  [TestMethod] /* commented by KarlovN */
        public void GetContentRowById_NoContents()
        {
            var result = new QPContentManager(TestUtils.RealDbConnector, TestUtils.RealContentItem)
               .Connection(TestUtils.ConnectionString)
               .SiteName(TestUtils.SiteName)
               .ContentName("AbstractItem")
               .StatusName(string.Join(",", new[] { ContentItemStatus.None, ContentItemStatus.Published }))
               .Fields("*")
               .Get();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.PrimaryContent);

            var testRow = result.GetContentRowById("AbstractItem", 55071);
            Assert.IsNotNull(testRow);
            Assert.AreEqual((decimal)55071, testRow["CONTENT_ITEM_ID"]);
            Assert.AreEqual("Корневая страница", testRow["Title"]);

            result.Contents.Clear();

            testRow = result.GetContentRowById("AbstractItem", 55071);
            Assert.IsNull(testRow);
        }

       //  [TestMethod] /* commented by KarlovN */
        public void GetReferenceRows_NoContents()
        {
            var result = new QPContentManager(TestUtils.RealDbConnector, TestUtils.RealContentItem)
               .Connection(TestUtils.ConnectionString)
               .SiteName(TestUtils.SiteName)
               .ContentName("AbstractItem")
               .Include("Culture")
               .StatusName(string.Join(",", new[] { ContentItemStatus.None, ContentItemStatus.Published }))
               .Fields("[CONTENT_ITEM_ID]")
               .Get();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.PrimaryContent);

            var testRow = result.GetReferenceRows("Culture", 232189);
            Assert.IsNotNull(testRow);
            Assert.AreEqual(1, testRow.Count);

            result.Contents.Clear();

            testRow = result.GetReferenceRows("Culture", 232189);
            Assert.IsNull(testRow);
        }

       //  [TestMethod] /* commented by KarlovN */
        public void GetReferenceRows_NotExistsLink()
        {
            var result = new QPContentManager(TestUtils.RealDbConnector, TestUtils.RealContentItem)
               .Connection(TestUtils.ConnectionString)
               .SiteName(TestUtils.SiteName)
               .ContentName("AbstractItem")
               .Include("Culture")
               .StatusName(string.Join(",", new[] { ContentItemStatus.None, ContentItemStatus.Published }))
               .Fields("[CONTENT_ITEM_ID]")
               .Get();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.PrimaryContent);

            var testRow = result.GetReferenceRows("Culture", -232189);
            Assert.IsNotNull(testRow);
            Assert.AreEqual(0, testRow.Count);
        }

       //  [TestMethod] /* commented by KarlovN */
        public void GetReferenceRows_GetNotExistsField()
        {
            var result = new QPContentManager(TestUtils.RealDbConnector, TestUtils.RealContentItem)
               .Connection(TestUtils.ConnectionString)
               .SiteName(TestUtils.SiteName)
               .ContentName("AbstractItem")
               .StatusName(string.Join(",", new[] { ContentItemStatus.None, ContentItemStatus.Published }))
               .Fields("*")
               .Get();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.PrimaryContent);

            var testRow = result.GetReferenceRows("fieldx", 55071);
            Assert.IsNull(testRow);
        }

       //  [TestMethod] /* commented by KarlovN */
        public void GetContentRowById_GetNoExistsRow()
        {
            var result = new QPContentManager(TestUtils.RealDbConnector, TestUtils.RealContentItem)
               .Connection(TestUtils.ConnectionString)
               .SiteName(TestUtils.SiteName)
               .ContentName("AbstractItem")
               .StatusName(string.Join(",", new[] { ContentItemStatus.None, ContentItemStatus.Published }))
               .Fields("*")
               .Get();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.PrimaryContent);

            var testRow = result.GetContentRowById("AbstractItem", -1);
            Assert.IsNull(testRow);
        }

       //  [TestMethod] /* commented by KarlovN */
        public void IncludeTest_Exists()
        {
            var result = new QPContentManager(TestUtils.FakeDbConnector, TestUtils.RealContentItem)
                .Connection(TestUtils.ConnectionString)
                .SiteName(TestUtils.SiteName)
                .ContentName("AbstractItem")
                .Include("Culture")
                .Include("Region")
                .Include("Culture");

            Assert.IsNotNull(result.Includes);
            Assert.AreEqual(2, result.Includes.Count);

            Assert.IsTrue(result.Includes.Contains("Culture"));
            Assert.IsTrue(result.Includes.Contains("Region"));
        }

       //  [TestMethod] /* commented by KarlovN */
        public void Include_For_AbstractItem()
        {
            #region init

            var result = new QPContentManager(TestUtils.RealDbConnector, TestUtils.RealContentItem)
                .Connection(TestUtils.ConnectionString)
                .SiteName(TestUtils.SiteName)
                .ContentName("AbstractItem")
                .StatusName(ContentItemStatus.None)
                .Fields("*")
                .Where("[Title] like '%QPContentManagerTests_Include_For_AbstractItem%'")
                .Get();

            foreach (DataRow row in result.PrimaryContent.Rows)
            {
                TestUtils.RealContentItem.Remove(int.Parse(row["CONTENT_ITEM_ID"].ToString()), TestUtils.RealDbConnector);
            }

            var item1 = TestUtils.RealContentItem.New(TestUtils.AbstractItemContentId,
                TestUtils.RealDbConnector);
            item1.FieldValues["Title"].Data = "QPContentManagerTests_Include_For_AbstractItem0";
            item1.FieldValues["Discriminator"].Data = "55070";
            item1.FieldValues["IsVisible"].Data = "1";
            item1.FieldValues["IsInSiteMap"].Data = "0";
            item1.FieldValues["Culture"].Data = "55065";

            item1.Save();

            var item2 = TestUtils.RealContentItem.New(TestUtils.AbstractItemContentId,
                TestUtils.RealDbConnector);
            item2.FieldValues["Title"].Data = "QPContentManagerTests_Include_For_AbstractItem1";
            item2.FieldValues["Discriminator"].Data = "55070";
            item2.FieldValues["IsVisible"].Data = "1";
            item2.FieldValues["IsInSiteMap"].Data = "0";
            item2.FieldValues["Culture"].Data = "55065";

            item2.Save();

            var item3 = TestUtils.RealContentItem.New(TestUtils.AbstractItemContentId,
                TestUtils.RealDbConnector);
            item3.FieldValues["Title"].Data = "QPContentManagerTests_Include_For_AbstractItem2";
            item3.FieldValues["Discriminator"].Data = "55070";
            item3.FieldValues["IsVisible"].Data = "1";
            item3.FieldValues["IsInSiteMap"].Data = "0";
            item3.FieldValues["Regions"].LinkedItems.Add(98081);
            item3.FieldValues["Regions"].LinkedItems.Add(98344);
            item3.FieldValues["Culture"].Data = "55065";

            item3.Save();

            var item4 = TestUtils.RealContentItem.New(TestUtils.AbstractItemContentId,
                TestUtils.RealDbConnector);
            item4.FieldValues["Title"].Data = "QPContentManagerTests_Include_For_AbstractItem4";
            item4.FieldValues["Discriminator"].Data = "55070";
            item4.FieldValues["IsVisible"].Data = "1";
            item4.FieldValues["IsInSiteMap"].Data = "0";
            item4.FieldValues["Culture"].Data = "55065";
            item4.FieldValues["Parent"].Data = item1.Id.ToString();

            item4.Save();

            #endregion

            result = new QPContentManager(TestUtils.RealDbConnector, TestUtils.RealContentItem)
                .Connection(TestUtils.ConnectionString)
                .SiteName(TestUtils.SiteName)
                .ContentName("AbstractItem")
                .Fields("[Name], [Title], [CONTENT_ITEM_ID]")
                .Include("Parent")
                .Include("Culture")
                .Include("Regions")
                .StatusName(ContentItemStatus.None)
                .Where("[Title] like '%QPContentManagerTests_Include_For_AbstractItem%'")
                .Get();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.PrimaryContent);
            Assert.AreEqual(4, result.PrimaryContent.Rows.Count);

            var contentCulture = result.GetContent("Culture");

            Assert.IsNotNull(contentCulture);
            Assert.IsTrue(0 != contentCulture.Rows.Count);

            var testRow = result.GetContentRowById("AbstractItem", item1.Id);
            Assert.IsNotNull(testRow);
            Assert.AreEqual((decimal)item1.Id, testRow["CONTENT_ITEM_ID"]);
            Assert.AreEqual("QPContentManagerTests_Include_For_AbstractItem0", testRow["Title"]);

            var rows = result.GetReferenceRows("Parent", item4.Id);

            Assert.IsNotNull(rows);
            Assert.AreEqual(1, rows.Count);
            Assert.AreEqual("QPContentManagerTests_Include_For_AbstractItem0", rows[0]["Title"]);

            rows = result.GetReferenceRows("Culture", item2.Id);

            Assert.IsNotNull(rows);
            Assert.AreEqual(1, rows.Count);
            Assert.IsTrue(string.Equals( "ru-RU", rows[0]["Name"] as string, StringComparison.InvariantCultureIgnoreCase));

            rows = result.GetReferenceRows("Regions", item1.Id);

            Assert.IsNotNull(rows);
            Assert.AreEqual(0, rows.Count);

            rows = result.GetReferenceRows("Regions", item3.Id);

            Assert.IsNotNull(rows);
            Assert.AreEqual(2, rows.Count);
            Assert.AreEqual("Центральный", rows[0]["Title"].ToString());
            Assert.AreEqual("Санкт-Петербург и Ленинградская область", rows[1]["Title"].ToString());

            TestUtils.RealContentItem.Remove(item1.Id, TestUtils.RealDbConnector);
            TestUtils.RealContentItem.Remove(item2.Id, TestUtils.RealDbConnector);
            TestUtils.RealContentItem.Remove(item3.Id, TestUtils.RealDbConnector);
            TestUtils.RealContentItem.Remove(item4.Id, TestUtils.RealDbConnector);
        }

       //  [TestMethod] /* commented by KarlovN */
        public void ArchiveTest()
        {
            var result = new QPContentManager(TestUtils.RealDbConnector, TestUtils.RealContentItem)
                .Connection(TestUtils.ConnectionString)
                .SiteName(TestUtils.SiteName)
                .ContentName("AbstractItem")
                .StatusName(ContentItemStatus.None)
                .Fields("*")
                .Where("[Title] like '%QPContentManagerTests_Archive%'")
                .Get();

            foreach (DataRow row in result.PrimaryContent.Rows)
            {
                TestUtils.RealContentItem.Remove(int.Parse(row["CONTENT_ITEM_ID"].ToString()), TestUtils.RealDbConnector);
            }

            var item1 = TestUtils.RealContentItem.New(TestUtils.AbstractItemContentId,
                TestUtils.RealDbConnector);
            item1.FieldValues["Title"].Data = "QPContentManagerTests_Archive0";
            item1.FieldValues["Discriminator"].Data = "55070";
            item1.FieldValues["IsVisible"].Data = "1";
            item1.FieldValues["IsInSiteMap"].Data = "0";
            item1.FieldValues["Culture"].Data = "55065";

            item1.Save();

            new QPContentManager(TestUtils.RealDbConnector, TestUtils.RealContentItem)
                .Connection(TestUtils.ConnectionString)
                .SiteName(TestUtils.SiteName)
                .ContentName("AbstractItem")
                .StatusName(ContentItemStatus.None)
                .Fields("[CONTENT_ITEM_ID]")
                .Where(string.Format("([CONTENT_ITEM_ID] = {0})", item1.Id))
                .Archive();

            result = new QPContentManager(TestUtils.RealDbConnector, TestUtils.RealContentItem)
                .Connection(TestUtils.ConnectionString)
                .SiteName(TestUtils.SiteName)
                .ContentName("AbstractItem")
                .StatusName(ContentItemStatus.None)
                .IsIncludeArchive(true)
                .Fields("*")
                .Where(string.Format("([CONTENT_ITEM_ID] = {0})", item1.Id))
                .Get();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.PrimaryContent);
            Assert.AreEqual(1, result.PrimaryContent.Rows.Count);
            Assert.AreEqual(true, result.PrimaryContent.Rows[0]["Archive"].ToString() == "0" ? false : true);

            TestUtils.RealContentItem.Remove(item1.Id, TestUtils.RealDbConnector);
        }

       //  [TestMethod] /* commented by KarlovN */
        public void RestoreTest()
        {
            var result = new QPContentManager(TestUtils.RealDbConnector, TestUtils.RealContentItem)
                .Connection(TestUtils.ConnectionString)
                .SiteName(TestUtils.SiteName)
                .ContentName("AbstractItem")
                .StatusName(ContentItemStatus.None)
                .Fields("*")
                .Where("[Title] like '%QPContentManagerTests_Restore%'")
                .Get();

            foreach (DataRow row in result.PrimaryContent.Rows)
            {
                TestUtils.RealContentItem.Remove(int.Parse(row["CONTENT_ITEM_ID"].ToString()), TestUtils.RealDbConnector);
            }

            var item1 = TestUtils.RealContentItem.New(TestUtils.AbstractItemContentId,
                TestUtils.RealDbConnector);
            item1.FieldValues["Title"].Data = "QPContentManagerTests_Restore0";
            item1.FieldValues["Discriminator"].Data = "55070";
            item1.FieldValues["IsVisible"].Data = "1";
            item1.FieldValues["IsInSiteMap"].Data = "0";
            item1.FieldValues["Culture"].Data = "55065";

            item1.Save();

            new QPContentManager(TestUtils.RealDbConnector, TestUtils.RealContentItem)
                .Connection(TestUtils.ConnectionString)
                .SiteName(TestUtils.SiteName)
                .ContentName("AbstractItem")
                .StatusName(ContentItemStatus.None)
                .Fields("[CONTENT_ITEM_ID]")
                .Where(string.Format("([CONTENT_ITEM_ID] = {0})", item1.Id))
                .Archive();

            result = new QPContentManager(TestUtils.RealDbConnector, TestUtils.RealContentItem)
                .Connection(TestUtils.ConnectionString)
                .SiteName(TestUtils.SiteName)
                .ContentName("AbstractItem")
                .StatusName(ContentItemStatus.None)
                .IsIncludeArchive(true)
                .Fields("*")
                .Where(string.Format("([CONTENT_ITEM_ID] = {0})", item1.Id))
                .Get();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.PrimaryContent);
            Assert.AreEqual(1, result.PrimaryContent.Rows.Count);
            Assert.AreEqual(true, result.PrimaryContent.Rows[0]["Archive"].ToString() == "0" ? false : true);

            new QPContentManager(TestUtils.RealDbConnector, TestUtils.RealContentItem)
                .Connection(TestUtils.ConnectionString)
                .SiteName(TestUtils.SiteName)
                .ContentName("AbstractItem")
                .StatusName(ContentItemStatus.None)
                .Fields("[CONTENT_ITEM_ID]")
                .IsIncludeArchive(true)
                .Where(string.Format("([CONTENT_ITEM_ID] = {0})", item1.Id))
                .Restore();

            result = new QPContentManager(TestUtils.RealDbConnector, TestUtils.RealContentItem)
               .Connection(TestUtils.ConnectionString)
               .SiteName(TestUtils.SiteName)
               .ContentName("AbstractItem")
               .StatusName(ContentItemStatus.None)
               .Fields("*")
               .Where(string.Format("([CONTENT_ITEM_ID] = {0})", item1.Id))
               .Get();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.PrimaryContent);
            Assert.AreEqual(1, result.PrimaryContent.Rows.Count);
            Assert.AreEqual(false, result.PrimaryContent.Rows[0]["Archive"].ToString() == "0" ? false : true);

            TestUtils.RealContentItem.Remove(item1.Id, TestUtils.RealDbConnector);
        }

       //  [TestMethod] /* commented by KarlovN */
        public void DeleteTest()
        {
            var result = new QPContentManager(TestUtils.RealDbConnector, TestUtils.RealContentItem)
                .Connection(TestUtils.ConnectionString)
                .SiteName(TestUtils.SiteName)
                .ContentName("AbstractItem")
                .StatusName(ContentItemStatus.None)
                .Fields("*")
                .Where("[Title] like '%QPContentManagerTests_Delete%'")
                .Get();

            foreach (DataRow row in result.PrimaryContent.Rows)
            {
                TestUtils.RealContentItem.Remove(int.Parse(row["CONTENT_ITEM_ID"].ToString()), TestUtils.RealDbConnector);
            }

            var item1 = TestUtils.RealContentItem.New(TestUtils.AbstractItemContentId,
                TestUtils.RealDbConnector);
            item1.FieldValues["Title"].Data = "QPContentManagerTests_Delete0";
            item1.FieldValues["Discriminator"].Data = "55070";
            item1.FieldValues["IsVisible"].Data = "1";
            item1.FieldValues["IsInSiteMap"].Data = "0";
            item1.FieldValues["Culture"].Data = "55065";

            item1.Save();

            result = new QPContentManager(TestUtils.RealDbConnector, TestUtils.RealContentItem)
                .Connection(TestUtils.ConnectionString)
                .SiteName(TestUtils.SiteName)
                .ContentName("AbstractItem")
                .StatusName(ContentItemStatus.None)
                .Fields("*")
                .Where(string.Format("([CONTENT_ITEM_ID] = {0})", item1.Id))
                .Get();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.PrimaryContent);
            Assert.AreEqual(1, result.PrimaryContent.Rows.Count);

            new QPContentManager(TestUtils.RealDbConnector, TestUtils.RealContentItem)
               .Connection(TestUtils.ConnectionString)
               .SiteName(TestUtils.SiteName)
               .ContentName("AbstractItem")
               .StatusName(ContentItemStatus.None)
               .Fields("[CONTENT_ITEM_ID]")
               .Where(string.Format("([CONTENT_ITEM_ID] = {0})", item1.Id))
               .Delete();

            result = new QPContentManager(TestUtils.RealDbConnector, TestUtils.RealContentItem)
               .Connection(TestUtils.ConnectionString)
               .SiteName(TestUtils.SiteName)
               .ContentName("AbstractItem")
               .StatusName(ContentItemStatus.None)
               .Fields("*")
               .Where(string.Format("([CONTENT_ITEM_ID] = {0})", item1.Id))
               .Get();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.PrimaryContent);
            Assert.AreEqual(0, result.PrimaryContent.Rows.Count);

            TestUtils.RealContentItem.Remove(item1.Id, TestUtils.RealDbConnector);
        }

       //  [TestMethod] /* commented by KarlovN */
        public void GetRealDataTest()
        {
            var result = new QPContentManager(TestUtils.RealDbConnector, TestUtils.RealContentItem)
                .Connection(TestUtils.ConnectionString)
                .SiteName(TestUtils.SiteName)
                .ContentName("BACKEND_ACTION")
                .Fields("ID, NAME, CODE")
                .Where("[Code] = 'refresh_profile'")
                .GetRealData();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.PrimaryContent);
            Assert.AreEqual(1, result.PrimaryContent.Rows.Count);

            Assert.AreEqual("refresh_profile", result.PrimaryContent.Rows[0]["CODE"].ToString());
            Assert.AreEqual("7", result.PrimaryContent.Rows[0]["ID"].ToString());
            Assert.AreEqual("Refresh Profile", result.PrimaryContent.Rows[0]["NAME"].ToString());
        }

       //  [TestMethod] /* commented by KarlovN */
        public void GetRealDataTest_EmptyWhere()
        {
            var result = new QPContentManager(TestUtils.RealDbConnector, TestUtils.RealContentItem)
                .Connection(TestUtils.ConnectionString)
                .SiteName(TestUtils.SiteName)
                .ContentName("BACKEND_ACTION")
                .Fields("ID, NAME, CODE")
                .GetRealData();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.PrimaryContent);
            Assert.IsTrue(1 < result.PrimaryContent.Rows.Count);
        }

       //  [TestMethod] /* commented by KarlovN */
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetRealDataTest_EmptyFields()
        {
            new QPContentManager(TestUtils.RealDbConnector, TestUtils.RealContentItem)
                .Connection(TestUtils.ConnectionString)
                .SiteName(TestUtils.SiteName)
                .ContentName("BACKEND_ACTION")
                .GetRealData();
        }

       //  [TestMethod] /* commented by KarlovN */
        public void GetRealDataTest_EmptyFields_CheckExceptionText()
        {
            try
            {
                new QPContentManager(TestUtils.RealDbConnector, TestUtils.RealContentItem)
                   .Connection(TestUtils.ConnectionString)
                   .SiteName(TestUtils.SiteName)
                   .ContentName("BACKEND_ACTION")
                   .GetRealData();

                Assert.Fail("The exception was not thrown.");
            }
            catch (ArgumentNullException ex)
            {
                Assert.IsInstanceOfType(ex, typeof(ArgumentNullException));
                Assert.AreEqual("Value cannot be null.\r\nParameter name: Query.Fields", ex.Message);
            }
        }

        #endregion
    }
}
