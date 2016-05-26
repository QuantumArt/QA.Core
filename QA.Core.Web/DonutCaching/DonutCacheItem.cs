// Owners: Karlov Nikolay

using System.Collections.Generic;

namespace QA.Core.Web
{
    /// <summary>
    /// Элемент кеша
    /// </summary>
    internal class DonutCacheItem
    {
        private string _result;
        private List<ReplacementBase> _replacements;

        /// <summary>
        /// Неизмененный результат
        /// </summary>
        internal string Result
        {
            get { return _result; }
            set { _result = value; }
        }

        /// <summary>
        /// Автозамены
        /// </summary>
        internal List<ReplacementBase> Replacements
        {
            get { return _replacements; }
            set { _replacements = value; }
        }

        internal DonutCacheItem(string result, List<ReplacementBase> list)
        {
            // TODO: Complete member initialization
            this._result = result;
            this._replacements = list;
        }
    }
}
