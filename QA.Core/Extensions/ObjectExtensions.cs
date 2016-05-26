using System.Collections.Generic;
using System.ComponentModel;

namespace QA.Core.Extensions
{
    /// <summary>
    /// Расширения объекта
    /// </summary>
    public static class ObjectExtensions
    {
        #region ObjectToDictionary

        /// <summary>
        /// Преобразование объекта в словарь
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, object> ObjectToDictionary(object instance)
        {
            if (instance is Dictionary<string, object>)
            {
                return (Dictionary<string, object>)instance;
            }

            var dictionary = new Dictionary<string, object>();

            if (instance != null)
            {
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(instance))
                {
                    dictionary.Add(descriptor.Name, descriptor.GetValue(instance));
                }
            }

            return dictionary;
        }

        #endregion
    }
}
