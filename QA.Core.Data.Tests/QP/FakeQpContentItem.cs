using System;
using System.Data;
using System.Data.SqlClient;
using QA.Core.Data.QP;
using Quantumart.QPublishing.Info;
using Quantumart.QPublishing.Info;

namespace QA.Core.Data.Tests.QP
{
    public class FakeQpContentItem : IQpContentItem
    {
        public ContentItem New(int contentId, IQpDbConnector cnn)
        {
            throw new NotImplementedException();
        }

        public ContentItem Read(int id, IQpDbConnector cnn)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id, IQpDbConnector cnn)
        {
            throw new NotImplementedException();
        }
    }
}
