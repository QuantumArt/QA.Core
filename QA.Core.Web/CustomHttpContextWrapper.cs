using System.Web;

namespace QA.Core.Web
{
    /// <summary>
    /// заглушка контекста запроса, заменяющая адрес запроса. 
    /// Используется в роутинге ContentRoute&lt;T&gt; для нестандартный роутов.
    /// </summary>
    public class CustomHttpContextWrapper : HttpContextWrapper
    {
        private readonly CustomRequestContextWrapper _request;

        public CustomHttpContextWrapper(HttpContext httpContext) : base(httpContext)
        {
            _request = new CustomRequestContextWrapper(httpContext.Request);
        }

        public CustomHttpContextWrapper(HttpContext httpContext, string rewrittenUrl) : base(httpContext)
        {
            _request = new CustomRequestContextWrapper(httpContext.Request, rewrittenUrl);
        }

        public override HttpRequestBase Request
        {
            get
            {
                return _request;
            }
        }
    }
    /// <summary>
    /// заглушка контекста запроса, заменяющая адрес запроса. 
    /// Используется в роутинге ContentRoute&lt;T&gt; для нестандартный роутов.
    /// </summary>
    public class CustomRequestContextWrapper : HttpRequestWrapper
    {
        private readonly string _rewrittenUrl;
        private readonly string _appRelativeCurrentExecutionFilePath;
        private readonly string _path;

        public CustomRequestContextWrapper(HttpRequest httpRequest) : base(httpRequest)
        {

        }

        public CustomRequestContextWrapper(HttpRequest httpRequest, string rewrittenUrl) : base(httpRequest)
        {
            _rewrittenUrl = rewrittenUrl.TrimStart('/');
            _appRelativeCurrentExecutionFilePath = "~/" + _rewrittenUrl;
            _path = "/" + _rewrittenUrl;
        }

        /// <summary>
        /// 
        /// </summary>
        public override string AppRelativeCurrentExecutionFilePath
        {
            get
            {
                return _appRelativeCurrentExecutionFilePath;
            }
        }

        public override string ApplicationPath
        {
            get
            {
                return base.ApplicationPath;
            }
        }
        
        public override string PathInfo
        {
            get
            {
                return base.PathInfo;
            }
        }

        public override string CurrentExecutionFilePath
        {
            get
            {
                return base.CurrentExecutionFilePath;
            }
        }
        public override string CurrentExecutionFilePathExtension
        {
            get
            {
                return base.CurrentExecutionFilePathExtension;
            }
        }

        public override string FilePath
        {
            get
            {
                return base.FilePath;
            }
        }

        public override string Path
        {
            get
            {
                return _path;
            }
        }

    }
}
