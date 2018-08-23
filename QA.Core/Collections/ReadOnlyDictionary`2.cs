using System.Collections.Generic;
#pragma warning disable 1591

namespace QA.Core.Collections
{
    /// <summary>
    /// Словарь, предоставляющий методы чтения
    /// </summary>
    public class ReadOnlyDictionary<TKey, TValue>
    {
        private IDictionary<TKey, TValue> _innerDictionary;
        public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary)
        {
            _innerDictionary = dictionary;
        }

        public ICollection<TKey> Keys
        {
            get
            {
                return _innerDictionary.Keys;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                return _innerDictionary[key];
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _innerDictionary.TryGetValue(key, out value);
        }

        public bool ContainsKey(TKey key)
        {
            return _innerDictionary.ContainsKey(key);
        }

        public IEnumerable<TValue> Values { get { return _innerDictionary.Values; } }
    }
}
