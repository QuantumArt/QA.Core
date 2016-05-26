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
    public abstract class HttpContextLifetimeManagerBase : LifetimeManager, IDisposable
    {

        private ThreadLocal<object> _val = new ThreadLocal<object>();


        protected abstract string Key { get; }

        /// <summary>
        /// Возвращает значение
        /// </summary>
        /// <returns></returns>
        public override object GetValue()
        {
            var ctx = HttpContext.Current;

            if (ctx == null)
            {
                return _val == null ? null : _val.Value;
            }

            return ctx.Items[Key];
        }

        /// <summary>
        /// Удаление значения
        /// </summary>
        public override void RemoveValue()
        {
            var disposable = GetValue() as IDisposable;
            var ctx = HttpContext.Current;

            if (ctx != null)
            {
                ctx.Items.Remove(Key);
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
                HttpContext.Current.Items[Key] = newValue;
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
