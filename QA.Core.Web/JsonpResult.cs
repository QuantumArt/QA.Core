using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace System.Web.Mvc
{
    public class JsonpResult : ActionResult
    {
        public string CallbackFunction { get; set; }
        public string CallbackFunctionParamName { get; set; }
        public Encoding ContentEncoding { get; set; }
        public string ContentType { get; set; }
        public object Data { get; set; }
        public JsonpResult(object data) : this(data, null) { }
        public JsonpResult(object data, string callbackFunctionParamName)
        {
            Data = data;
            CallbackFunctionParamName = callbackFunctionParamName;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = string.IsNullOrEmpty(ContentType) ? "application/x-javascript" : ContentType;

            if (ContentEncoding != null) response.ContentEncoding = ContentEncoding;
            {
                var cfn = CallbackFunctionParamName ?? "callback";
                HttpRequestBase request = context.HttpContext.Request;
                var callback = (CallbackFunction ?? request.Params[cfn]) ?? "callback";
                var serializer = new JavaScriptSerializer();
                response.Write(string.Format("{0}({1});", callback, (Data != null ? serializer.Serialize(Data) : "null")));
            }
        }
    }

    public static class ResultControllerExtensions
    {
        public static JsonpResult Jsonp(this Controller controller, object data, string callbackParamName)
        {
            return new JsonpResult(data, callbackParamName ?? "callback");
        }
        public static JsonpResult Jsonp(this Controller controller, object data)
        {
            return controller.Jsonp(data, null);
        }
    }
}
