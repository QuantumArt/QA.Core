using System;
using System.Collections;
using System.Globalization;
using System.Resources;
using System.Web;
using System.Web.Compilation;
#pragma warning disable 1591

namespace QA.Core.Web
{
    public class QpCustomResourceProvider : IResourceProvider
    {
        private string _virtualPath;
        private string _className;

        public QpCustomResourceProvider(string virtualPath, string classname)
        {
            _virtualPath = virtualPath;
            _className = classname;
        }

        private IDictionary GetResourceCache(string culturename)
        {
            return (IDictionary)
                HttpContext.Current.Cache[culturename];
        }

        object IResourceProvider.GetObject
            (string resourceKey, CultureInfo culture)
        {
            object value;

            string cultureName = null;
            if (culture != null)
            {
                cultureName = culture.Name;
            }
            else
            {
                cultureName = CultureInfo.CurrentUICulture.Name;
            }

            value = GetResourceCache(cultureName)[resourceKey];
            if (value == null)
            {
                value = GetResourceCache(null)[resourceKey];
            }
            return value;
        }

        //private void EnsureResourceManager()
        //{
        //    var assembly = typeof(Resources.ResourceInAppToGetAssembly).Assembly;
        //    String resourceFullName = String.Format("{0}.Resources.{1}", assembly.GetName().Name, _className);
        //    _ResourceManager = new global::System.Resources.ResourceManager(resourceFullName, assembly);
        //    _ResourceManager.IgnoreCase = true;
        //}

        IResourceReader IResourceProvider.ResourceReader
        {
            get
            {
                string cultureName = null;
                CultureInfo currentUICulture = CultureInfo.CurrentUICulture;
                if (!String.Equals(currentUICulture.Name,
                    CultureInfo.InstalledUICulture.Name))
                {
                    cultureName = currentUICulture.Name;
                }

                return new QpCustomResourceReader
                    (GetResourceCache(cultureName));
            }
        }
    }
}
