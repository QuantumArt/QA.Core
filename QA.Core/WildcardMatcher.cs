using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QA.Core
{
    /// <summary>
    /// Класс, анализирующий соответствие строки и wildcard. 
    /// Примеры:  *.js
    /// </summary>
    public class WildcardMatcher : IWildcardMatcher
    {
        private Dictionary<string, Regex> _dictionary;
        private WildcardMatchingOption _option;

        /// <summary>
        /// Создать объект
        /// </summary>
        /// <param name="items">список шаблонов</param>
        public WildcardMatcher(params string[] items)
            : this(WildcardMatchingOption.FullMatch)
        { }

        /// <summary>
        /// Создать объект
        /// </summary>
        /// <param name="items">список шаблонов</param>
        /// <param name="option">Тип наложения шаблона</param>
        public WildcardMatcher(WildcardMatchingOption option, params string[] items)
            : this(option, (IEnumerable<string>) items)
        { }

        /// <summary>
        /// Создать объект
        /// </summary>
        /// <param name="items">список шаблонов</param>
        /// <param name="option">Тип наложения шаблона</param>
        public WildcardMatcher(WildcardMatchingOption option, IEnumerable<string> items)
        {
            _dictionary = new Dictionary<string, Regex>();
            _option = option;
            foreach (var item in items.Distinct())
            {
                _dictionary.Add(item, PrepareExpression(item));
            }
        }

        /// <summary>
        /// Получить полный список шаблонов, под которые подходит строка
        /// </summary>
        /// <param name="text">строка для проверки</param>
        /// <returns></returns>
        public virtual IEnumerable<string> Match(string text)
        {
            foreach (var item in _dictionary)
            {
                if (item.Value.IsMatch(text))
                {
                    if (item.Value.IsMatch(text))
                        yield return item.Key;
                }
            }
        }

        /// <summary>
        /// Возвращает наименее общее правило (самое самы длинный шаблон), которому удовлетворяет строка
        /// </summary>
        /// <param name="text">строка для проверки</param>
        /// <returns></returns>
        public string MatchLongest(string text)
        {
            return Match(text)
                .OrderByDescending(x => x.Length)
                .OrderByDescending(x => x)
                .FirstOrDefault();
        }

        private Regex PrepareExpression(string pattern)
        {
            const string placeholder = "__fake_123_";
            string escaped = "";

            if ((_option & WildcardMatchingOption.StartsWith) != 0)
            {
                escaped = @"^" + escaped;
            }

            escaped +=  Regex
                           .Escape(pattern.Replace("*", placeholder))
                           .Replace(placeholder, @"[\w+-_]*");

            if ((_option & WildcardMatchingOption.EndsWith) != 0)
            {
                escaped += "$";
            }


            var expr = new Regex(escaped, ((_option & WildcardMatchingOption.CaseSensitive) == 0) ?
                RegexOptions.IgnoreCase :
                RegexOptions.None
            );

            return expr;
        }

    }
    /// <summary>
    /// Тип наложения шаблона
    /// </summary>
    [Flags]
    public enum WildcardMatchingOption
    {
        None = 0,
        StartsWith = 1,
        EndsWith = 2,
        /// <summary>
        /// Полное соответствие
        /// </summary>
        FullMatch = WildcardMatchingOption.StartsWith | WildcardMatchingOption.EndsWith,
        CaseSensitive = 4,
    }
}
