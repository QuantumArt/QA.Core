using Microsoft.Web.Infrastructure.DynamicModuleHelper;

namespace QA.Core.Web.Properties
{
    /// <summary>
    /// Выполнение логики при инициализации веб-приложения
    /// </summary>
    public static class PreApplicationInitializer
    {
        public static void Start()
        {
            // регистрируем модуль, вызывающий dispose у всех IDisposable 
            DynamicModuleUtility.RegisterModule(typeof(RequestFinalizerModule));
        }
    }
}
