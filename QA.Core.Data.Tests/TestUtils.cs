using System.Configuration;
using QA.Core.Data.QP;
using QA.Core.Data.Tests.QP;

namespace QA.Core.Data.Tests
{
    public static class TestUtils
    {
        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["qp_database"].ConnectionString;
            }
        }

        public static string SiteName
        {
            get
            {
                return "main_site";
            }
        }

        public static int AbstractItemContentId
        {
            get
            {
                return 293;
            }
        }

        public static IQpDbConnector FakeDbConnector
        {
            get
            {
                return new FakeQpDbConnector();
            }
        }

        public static IQpDbConnector RealDbConnector
        {
            get
            {
                return new QpDbConnector("qp_database");
            }
        }

        public static IQpContentItem FakeContentItem
        {
            get
            {
                return new FakeQpContentItem();
            }
        }

        public static IQpContentItem RealContentItem
        {
            get
            {
                return new QpContentItem();
            }
        }
    }
}
