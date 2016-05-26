// Owners: Karlov Nikolay, Abretov Alexey

using System.Web;

namespace QA.Core.Web
{
    public static class ThreadSafeStorage
    {
        public static void SetInstance<T>(string key, T instance)
        {
            HttpContext.Current.Items[key] = instance;
        }

        public static T GetInstance<T>(string key)
        {
            var instance = HttpContext.Current.Items[key];
            if (instance != null || instance is T)
            {
                return (T)instance;
            }
            else
            {
                return default(T);
            }
        }
    }
}