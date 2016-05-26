// Owners: Karlov Nikolay, Abretov Alexey

using System;
using System.Web.WebPages;

namespace QA.Core.Web
{
    /// <summary>
    /// Класс замены некешируемых областей
    /// </summary>
    internal class MarkupReplacement : ReplacementBase
    {
        /// <summary>
        /// Автозамена
        /// </summary>
        internal Func<object, HelperResult> Action { get; set; }

        public MarkupReplacement(string key, Func<object, HelperResult> action)
        {
            Key = key;
            Action = action;
        }
    }
}
