using System;
using System.Data;
using System.Data.SqlClient;
using QA.Core.Data.QP;
using Quantumart.QPublishing.Info;
using Quantumart.QPublishing.Info;

namespace QA.Core.Data.Tests.QP
{
    public class FakeQpDbConnector : IQpDbConnector
    {
        public DataTable GetContentData(ContentDataQueryObject query, ref long totalRecords)
        {
            throw new NotImplementedException();
        }

        public DataTable GetRealData(SqlCommand command)
        {
            throw new NotImplementedException();
        }

        public string InstanceConnectionString
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string GetContentName(int contentId)
        {
            throw new NotImplementedException();
        }

        public string GetContentItemLinkIDs(string fieldName, int itemId)
        {
            throw new NotImplementedException();
        }

        public void DeleteContentItem(int contentItemId)
        {
            throw new NotImplementedException();
        }

        public string GetContentItemLinkIDs(string fieldName, string values)
        {
            throw new NotImplementedException();
        }

        public object Connector
        {
            get { throw new NotImplementedException(); }
        }

        #region IQpDbConnector Members

        public Quantumart.QPublishing.Database.DBConnector DbConnector
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
