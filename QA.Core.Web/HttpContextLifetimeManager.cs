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
    /// </summary>
    public class HttpContextLifetimeManager : LifetimeManager, IDisposable
    {
        private readonly string _itemName = Guid.NewGuid().ToString();

        private ThreadLocal<object> _val = new ThreadLocal<object>();

        /// <summary>
        /// Возвращает значение
        /// </summary>
        /// <returns></returns>
        public override object GetValue()
        {
            if (HttpContext.Current == null)
            {
                return _val == null ? null : _val.Value;
            }

            return HttpContext.Current.Items[_itemName];
        }

        /// <summary>
        /// Удаление значения
        /// </summary>
        public override void RemoveValue()
        {
            var disposable = GetValue() as IDisposable;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Items.Remove(_itemName);
            }
            _val = null;

            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        /// <summary>
        /// Устанавливает значение
        /// </summary>
        /// <param name="newValue"></param>
        public override void SetValue(object newValue)
        {
            if (HttpContext.Current == null)
            {
                _val.Value = newValue;
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
