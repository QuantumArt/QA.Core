using System.Web.Compilation;

namespace QA.Core.Web
{
    public class QpCustomResourceProviderFactory : ResourceProviderFactory
    {
        public override IResourceProvider CreateGlobalResourceProvider(string classname)
        {
            return new QpCustomResourceProvider(null, classname);
        }

        public override IResourceProvider CreateLocalResourceProvider(string virtualPath)
        {
            return new QpCustomResourceProvider(virtualPath, null);
        }
    }
}