using System.Configuration;
using System.ServiceModel;
using System.Web;
using QA.Core.Service.Interaction;
using Quantumart.QPublishing;
using Quantumart.QPublishing.Database;

namespace QA.Core.Web.Qp
{
    /// <summary>
    /// Проверка авторизации для администрирования
    /// </summary>
    public class QPSecurityCheckerSingleDB : QPSecurityChecker
    {
        public const string DefaultQPConnectionName = "qp_database";
        protected override ServiceToken CurrentServiceToken
        {

            get
            {
                return new ServiceToken { ConnectionName = DefaultQPConnectionName };
            }
        }
    }
}
