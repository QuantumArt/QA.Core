using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;

namespace QA.Core.Replacing
{
    /// <summary>
    /// Управление заменами
    /// </summary>
    public class ReplacementManager
    {
        private readonly ConcurrentDictionary<Type, IReplacementProcessor> _processors = new ConcurrentDictionary<Type, IReplacementProcessor>();

        /// <summary>
        /// Текущий экземпляр класса
        /// </summary>
        public static readonly ReplacementManager Current = new ReplacementManager();


        private ReplacementManager() { }

        #region Process
        /// <summary>
        /// Произвести замену полей
        /// </summary>
        /// <param name="obj">объект</param>
        /// <param name="cultureKey">культура ("ru-ru", ...)</param>
        /// <returns></returns>
        public object Process(object obj, string cultureKey)
        {
            if (obj == null)
            {
                return null;
            }

            IReplacementProcessor processor = Get(obj);

            return processor.Process(obj, cultureKey);
        }

        /// <summary>
        /// Произвести замену полей в коллекции
        /// </summary>
        /// <param name="obj">коллекция объектов</param>
        /// <param name="cultureKey">культура ("ru-ru", ...)</param>
        /// <returns></returns>
        public IEnumerable<object> ProcessCollection(IEnumerable<object> obj, string cultureKey)
        {
            if (obj == null)
            {
                return null;
            }
            
            IReplacementProcessor processor = null;

            foreach (object item in obj)
            {
                if (processor == null) processor = Get(item);
                processor.Process(item, cultureKey);

            }

            return obj;
        }

        /// <summary>
        /// Произвести замену полей
        /// </summary>
        /// <param name="obj">объект</param>
        /// <returns></returns>
        public object Process(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            IReplacementProcessor processor = Get(obj);

            return processor.Process(obj);
        }

        /// <summary>
        /// Произвести замену полей в коллекции, для установленного языка (CultureInfo.CurrentUICulture)
        /// </summary>
        /// <param name="obj">коллекция объектов</param>
        /// <returns></returns>
        public object ProcessCollection(IEnumerable<object> obj)
        {
            return ProcessCollection(obj, CultureInfo.CurrentUICulture.Name.ToLower());
        }

        #endregion

        private IReplacementProcessor Get(object obj)
        {
            return _processors.GetOrAdd(obj.GetType(),
                t => new SwapReplacementProcessor(t));
        }
    }
}
