using QA.Core.Service.Interaction;

#pragma warning disable 1591

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
