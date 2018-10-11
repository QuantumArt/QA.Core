using System.Runtime.CompilerServices;
using System.Web;
using QA.Core;
using QA.Core.Web.Properties;


[assembly: PreApplicationStartMethod(typeof(PreApplicationInitializer), "Start")]

[assembly:TypeForwardedTo(typeof(IContentInvalidator))]
