using Microsoft.VisualStudio.TestTools.UnitTesting;
using QA.Core.Data.Repository;


namespace QA.Core.Data.Tests.Repository
{
    [TestClass]
    public class ConnectionManagerTests
    {
        [TestMethod]
        public void ConnectionStringFromConfig()
        {
            const string connectionStringName = "test";
            ConnectionManager connectionManager = new ConnectionManager();
            var connection = connectionManager.GetConnection(connectionStringName);
            Assert.IsNotNull(connection);
        }

        [TestMethod]
        public void ConnectionString()
        {
            const string connectionString = "Application Name=QP7.Qa_Beeline_Main;Initial Catalog=qp_beeline_main_cis;Data Source=qwe;Integrated Security=True;";
            ConnectionManager connectionManager = new ConnectionManager();
            var connection = connectionManager.GetConnection(connectionString);
            Assert.IsNotNull(connection);
            Assert.AreEqual(connection.ConnectionString, connectionString);
        }
    }
}
