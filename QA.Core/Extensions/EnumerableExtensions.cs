// Owners: Alexey Abretov, Nikolay Karlov

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace QA.Core.Extensions
{
    /// <summary>
    /// Расширения для перечислений
    /// </summary>
    public static class EnumerableExtensions
    {
        #region Methods

        /// <summary>
        /// Performs the given <see cref="Action{T}" /> on each item in the enumerable.
        /// </summary>
        /// <typeparam name="T">The type of item in the enumerable.</typeparam>
        /// <param name="items">The enumerable of items.</param>
        /// <param name="action">The action to perform on each item.</param>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            Throws.IfArgumentNull(items, _ => items);
            Throws.IfArgumentNull(action, _ => action);

            foreach (T item in items)
                action(item);
        }

        public static void Merge(
            this IDictionary<string, object> instance,
            IDictionary<string, object> from,
            bool replaceExisting)
        {
            Throws.IfArgumentNull(instance, _ => instance);
            Throws.IfArgumentNull(from, _ => from);

            foreach (KeyValuePair<string, object> keyValuePair in (IEnumerable<KeyValuePair<string, object>>)from)
            {
                if (replaceExisting || !instance.ContainsKey(keyValuePair.Key))
                {
                    instance[keyValuePair.Key] = keyValuePair.Value;
                }
            }
        }

        public static void Merge(
            this IDictionary<string, object> instance,
            string name,
            object val,
            bool replaceExisting)
        {
            Throws.IfArgumentNull(instance, _ => instance);
            Throws.IfArgumentNot(!string.IsNullOrEmpty(name), _ => name);

            if (!instance.ContainsKey(name))
            {
                instance.Add(name, val);
                return;
            }

            if (instance.ContainsKey(name) & replaceExisting)
            {
                instance[name] = val;
                return;
            }

            if (instance.ContainsKey(name) & !replaceExisting)
            {
                instance[name] += " " + val;
                return;
            }
        }

        /// <summary>
        /// Возвращает локализованное значение перечисления
        /// </summary>
        /// <param name="enum"></param>
        /// <returns></returns>
        public static string GetLocalizedDescription(this Enum @enum)
        {
            if (@enum == null)
            {
                return null;
            }

            string description = @enum.ToString();
            FieldInfo fieldInfo = @enum.GetType().GetField(description);
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes.Any())
            {
                return attributes[0].Description;
            }

            return description;
        }

        /// <summary>
        /// Сериализация перечисления в формат JSON
        /// </summary>
        /// <param name="enum">Перечисление для сериализации</param>
        public static string ToJsonString(this Enum @enum)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");

            foreach (FieldInfo fInfo in
                @enum.GetType().GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                sb.AppendFormat("{0}:\"{1}\",\r\n",
                    fInfo.Name,
                    fInfo.GetValue(null).ToString());
            }
            sb.Append("}");

            return sb.ToString();
        }

        #endregion
    }
}
