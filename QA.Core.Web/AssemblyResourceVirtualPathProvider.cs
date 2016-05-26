using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

namespace QA.Core.Web
{
    // From http://www.codeproject.com/KB/aspnet/ASP2UserControlLibrary.aspx
    // переписано под mvc 4

    /// <summary>
    /// Провайдер для объектов, являющихся ресурсами сборки.
    /// Обрабатывает адреса, начинающиеся с '~/App_Resource/',
    /// и игнорирует, содержащие  '_ViewStart.'
    /// </summary>
    public class AssemblyResourceVirtualPathProvider : VirtualPathProvider
    {
        private readonly Dictionary<string, Assembly> _nameAssemblyCache;

        public AssemblyResourceVirtualPathProvider()
        {
            _nameAssemblyCache = new Dictionary<string, Assembly>(StringComparer.InvariantCultureIgnoreCase);
        }

        private bool IsAppResourcePath(string virtualPath)
        {
            string checkPath = VirtualPathUtility.ToAppRelative(virtualPath);

            return checkPath.StartsWith("~/App_Resource/",
                StringComparison.InvariantCultureIgnoreCase) &&
                !checkPath.Contains("_ViewStart.");
        }

        public override bool FileExists(string virtualPath)
        {
            return (IsAppResourcePath(virtualPath) ||
                    base.FileExists(virtualPath));
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (IsAppResourcePath(virtualPath))
            {
                return new AssemblyResourceFile(_nameAssemblyCache, virtualPath);
            }

            return base.GetFile(virtualPath);
        }

        public override CacheDependency GetCacheDependency(
            string virtualPath,
            IEnumerable virtualPathDependencies,
            DateTime utcStart)
        {
            if (IsAppResourcePath(virtualPath))
            {
                return null;
            }

            return base.GetCacheDependency(virtualPath,
                                           virtualPathDependencies, utcStart);
        }

        #region Nested classes
        private class AssemblyResourceFile : VirtualFile
        {
            private readonly IDictionary<string, Assembly> _nameAssemblyCache;
            private readonly string _assemblyPath;

            public AssemblyResourceFile(IDictionary<string, Assembly> nameAssemblyCache, string virtualPath) :
                base(virtualPath)
            {
                _nameAssemblyCache = nameAssemblyCache;
                _assemblyPath = VirtualPathUtility.ToAppRelative(virtualPath);
            }

            public override Stream Open()
            {
                // ~/App_Resource/WikiExtension.dll/WikiExtension/Presentation/Views/Wiki/Index.aspx
                string[] parts = _assemblyPath.Split(new[] { '/' }, 4);

                // TODO: should assert and sanitize 'parts' first
                string assemblyName = parts[2];
                var assemblyNameShort = assemblyName
                    .EndsWith(".dll") ? assemblyName.Substring(0, assemblyName.Length - 4)
                    : assemblyName;

                string resourceName = parts[3].Replace('/', '.');
                resourceName = assemblyNameShort + "." + resourceName;

                Assembly assembly;

                lock (_nameAssemblyCache)
                {
                    assembly = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith(assemblyNameShort + ",", StringComparison.InvariantCultureIgnoreCase))
                        .FirstOrDefault();

                    //if (!_nameAssemblyCache.TryGetValue(assemblyName, out assembly))
                    //{
                    //    var assemblyPath = Path.Combine(HttpRuntime.BinDirectory, assemblyName);
                    //    assembly = Assembly.LoadFrom(assemblyPath);

                    //    // TODO: Assert is not null
                    //    _nameAssemblyCache[assemblyName] = assembly;
                    //}
                }

                Stream resourceStream = null;

                if (assembly != null)
                {
                    resourceStream = assembly.GetManifestResourceStream(resourceName);
                }

                return resourceStream;
            }
        } 
        #endregion
    }
}