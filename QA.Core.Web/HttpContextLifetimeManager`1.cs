// Owners: Alexey Abretov, Nikolay Karlov

using System;
using System.Threading;
using System.Web;
using Microsoft.Practices.Unity;

namespace QA.Core.Web
{
    /// <summary>
    /// Реализует жизненный цикл объекта в текущем запросе.
    /// Если контекст запроса пуст, то используется стратегия ThreadLocal<T>.
    /// Зависит от сборки System.Web. Перенести в QA.Core.Web
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HttpContextLifetimeManager<T> : LifetimeManager, IDisposable
    {
        /// <summary>
        /// TODO: разобраться, какое именование лучше: _itemName = Guid.NewGuid().ToString() или 
        /// _itemName = typeof(T).AssemblyQualifiedName;
        /// Второе именование не позволяет создавать именнованые экземпляры одного типа
        /// </summary>
        private readonly string _itemName = Guid.NewGuid().ToString();
        private static ThreadLocal<T> _val = new ThreadLocal<T>();

        /// <summary>
        /// Возвращает значение
        /// </summary>
        /// <returns></returns>
        public override object GetValue()
        {
            if (HttpContext.Current == null)
            {
                return _val.Value;
            }

            return HttpContext.Current.Items[_itemName];
        }

        /// <summary>
        /// Удаление значения
        /// </summary>
        public override void RemoveValue()
        {
            var disposable = GetValue() as IDisposable;
            HttpContext.Current.Items.Remove(_itemName);

            if (disposable != null)
                disposable.Dispose();
        }

        /// <summary>
        /// Устанавливает значение
        /// </summary>
        /// <param name="newValue"></param>
        public override void SetValue(object newValue)
        {
            if (HttpContext.Current == null)
            {
                _val.Value = (T)newValue;
            }
            else
            {
                HttpContext.Current.Items[_itemName] = newValue;
            }
        }

        /// <summary>
        /// Освобождение ресурсов
        /// </summary>
        public void Dispose()
        {
            RemoveValue();
        }
    }
}
