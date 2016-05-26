// Owners: Karlov Nikolay, Abretov Alexey

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Mvc;

namespace QA.Core.Web
{
    /// <summary>
    /// Провайдер хранения временных данных в скрытом поле
    /// </summary>
    public class HiddenInputTempDataProvider : ITempDataProvider
    {
        HttpContextBase _httpContext;

        public HiddenInputTempDataProvider(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }
            _httpContext = httpContext;
        }

        public HttpContextBase HttpContext
        {
            get
            {
                return _httpContext;
            }
        }

        protected virtual IDictionary<string, object> LoadTempData(ControllerContext controllerContext)
        {
            var value = controllerContext.HttpContext.Request.Form[ValidationHtmlHelpers.TempDataViewDataKey];

            if (!string.IsNullOrEmpty(value))
            {
                return HiddenInputTempDataProvider
                    .DeserializeTempData(value) as IDictionary<string, object>;
            }

            return new Dictionary<string, object>();
        }

        protected virtual void SaveTempData(ControllerContext controllerContext, IDictionary<string, object> values)
        {
            if (values != null)
            {
                controllerContext.Controller.ViewData[ValidationHtmlHelpers.TempDataViewDataKey] =
                    HiddenInputTempDataProvider.SerializeToBase64EncodedString(values);
            }
        }

        public static IDictionary<string, object> DeserializeTempData(string base64EncodedSerializedTempData)
        {
            byte[] bytes = Convert.FromBase64String(base64EncodedSerializedTempData);
            using (var memStream = new MemoryStream(bytes))
            {
                var binFormatter = new BinaryFormatter();
                return binFormatter.Deserialize(memStream, null) as IDictionary<string, object>;
            }
        }

        public static string SerializeToBase64EncodedString(IDictionary<string, object> values)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                memStream.Seek(0, SeekOrigin.Begin);
                var binFormatter = new BinaryFormatter();
                binFormatter.Serialize(memStream, values);
                memStream.Seek(0, SeekOrigin.Begin);
                byte[] bytes = memStream.ToArray();

                return Convert.ToBase64String(bytes, Base64FormattingOptions.None);
            }
        }

        IDictionary<string, object> ITempDataProvider.LoadTempData(ControllerContext controllerContext)
        {
            return LoadTempData(controllerContext);
        }

        void ITempDataProvider.SaveTempData(ControllerContext controllerContext, IDictionary<string, object> values)
        {
            SaveTempData(controllerContext, values);
        }
    }
}
