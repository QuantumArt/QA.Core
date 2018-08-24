using System.Configuration;
using System.Web;

#pragma warning disable 1591

namespace QA.Core.Web.Qp
{
    /// <summary>
    /// Методы для взаимойствия c Qp
    /// </summary>
    public class QpHelper
    {
        protected static string QpCustomerCodeParamName
        {
            get
            {
                return ConfigurationManager.AppSettings["QP.CustomerCodeParamName"];
            }
        }

        protected static string QpSiteIdParamName
        {
            get
            {
                return ConfigurationManager.AppSettings["QP.SiteIdParamName"];
            }
        }

        protected static string QpBackendSidParamName
        {
            get
            {
                return ConfigurationManager.AppSettings["QP.BackendSidParamName"];
            }
        }

        protected static string QpHostIdParamName
        {
            get
            {
                return ConfigurationManager.AppSettings["QP.HostIdParamName"] ?? "hostUID";
            }
        }

        private const string CustomerCodeParamName = "CustomerCode";
        private const string SiteIdParamName = "SiteId";
        private const string BackendSidParamName = "BackendSid";
        private const string HostIdParamName = "HostId";

        private static string GetParam(
            string name)
        {
            return string.IsNullOrEmpty(HttpContext.Current.Request.Params[
                name]) ? HttpContext.Current.Request.QueryString[
                    name] : HttpContext.Current.Request.Params[
                        name];
        }

        /// <summary>
        /// Код поставщика
        /// </summary>
        public static string CustomerCode
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    return string.IsNullOrEmpty(GetParam(
                        QpCustomerCodeParamName)) ? GetParam(
                        CustomerCodeParamName) : GetParam(
                            QpCustomerCodeParamName);
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Идентификатор сайта
        /// </summary>
        public static string SiteId
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    return string.IsNullOrEmpty(GetParam(
                       QpSiteIdParamName)) ? GetParam(
                        SiteIdParamName) : GetParam(
                           QpSiteIdParamName);
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Id бэкенда
        /// </summary>
        public static string BackendSid
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    return string.IsNullOrEmpty(GetParam(
                       QpBackendSidParamName)) ? GetParam(
                        BackendSidParamName) : GetParam(
                           QpBackendSidParamName);
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Id хоста
        /// </summary>
        public static string HostId
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    return string.IsNullOrEmpty(GetParam(
                       QpHostIdParamName)) ? GetParam(
                        HostIdParamName) : GetParam(
                           QpHostIdParamName);
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Ключ
        /// </summary>
        public static string QpKey
        {
            get
            {
                return (CustomerCode ?? string.Empty) + "_" + (SiteId ?? string.Empty);
            }
        }

        /// <summary>
        /// Признак запуска через Custom Action Qp
        /// </summary>
        public static bool IsQpMode
        {
            get
            {
                return !string.IsNullOrEmpty(GetParam(
                        QpHostIdParamName)) || string.IsNullOrEmpty(GetParam(
                        HostIdParamName));
            }
        }
    }
}
