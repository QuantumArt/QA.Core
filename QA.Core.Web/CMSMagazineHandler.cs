using System.Configuration;
using System.Web;
#pragma warning disable 1591

namespace QA.Core.Web
{
    public class CMSMagazineHandler : IHttpHandler
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public CMSMagazineHandler()
        {

        }

        public void ProcessRequest(HttpContext context)
        {
            var code = ConfigurationManager.AppSettings["CMSMagazine.Code"];

            if (!string.IsNullOrEmpty(code))
            {
                context.Response.Write(code);
            }
            else
            {
                //cmsmagazine a3e61137bd39dc74c61accb0cbde8eee.txt
                // получим из реквеста
                context.Response.Write("a3e61137bd39dc74c61accb0cbde8eee");
            }
        }

        #endregion
    }
}
