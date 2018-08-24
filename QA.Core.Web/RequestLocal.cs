using System;
using System.Threading;
using System.Web;
#pragma warning disable 1591

namespace QA.Core.Web
{
    /// <summary>
    /// Класс, предоставляющий доступ к локальному хранилищу текущего запроса.
    /// В случае использования в не web-приложении, ведет себя как ThreadLocal&lt;T&gt;.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RequestLocal<T> : IDisposable
    {
        private Guid _key;
        private Func<T> _defaultValueProvider;

        /// <summary>
        /// Используется только в не web-приложениях
        /// </summary>
        private ThreadLocal<T> threadStaticInstance = new ThreadLocal<T>();

        /// <summary>
        /// Создание экземпляра класса с фабрикой
        /// </summary>
        public RequestLocal()
            : this(null)
        {
        }

        /// <summary>
        /// Создание экземпляра класса с фабрикой
        /// </summary>
        /// <param name="defaultValueProvider">фабрика значения по умолчанию</param>
        public RequestLocal(Func<T> defaultValueProvider)
        {
            _key = Guid.NewGuid();
            _defaultValueProvider = defaultValueProvider;
        }

        /// <summary>
        /// Значение
        /// </summary>
        public T Value
        {
            get
            {
                object item = null;

                // Проверяем контекст использования
                if (HttpContext.Current != null)
                {
                    // web
                    if (!HttpContext.Current.Items.Contains(_key))
                    {
                        if (_defaultValueProvider != null)
                        {
                            // возвращаем значение по умолчанию
                            item = _defaultValueProvider.Invoke();

                            HttpContext.Current.Items[_key] = item;
                        }
                        else
                        {
                            return default(T);
                        }
                    }
                    else
                    {
                        item = HttpContext.Current.Items[_key];
                    }
                }
                else
                {
                    // не web
                    if (threadStaticInstance.IsValueCreated)
                    {
                        return threadStaticInstance.Value;
                    }
                    else
                    {
                        if (_defaultValueProvider != null)
                        {
                            // возвращаем значение по умолчанию
                            threadStaticInstance.Value = _defaultValueProvider.Invoke();

                            return threadStaticInstance.Value;
                        }
                        else
                        {
                            return default(T);
                        }
                    }
                }

                return (T)item;
            }
            set
            {
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Items[_key] = value;
                }
                else
                {
                    threadStaticInstance.Value = value;
                }
            }
        }

        public void Dispose()
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Items.Remove(_key);
            }
        }
    }
}
