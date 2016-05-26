using System;
using System.Collections.Generic;
using System.Linq;

namespace QA.Core
{
	/// <summary>
	/// Транслитерация строк
	/// </summary>
	public static class Transliterator
	{
		private static Dictionary<char, string> symbolMap = new Dictionary<char, string>
		{
			{'а', "a"},
			{'б', "b"},
			{'в', "v"},
			{'г', "g"},
			{'д', "d"},
			{'е', "e"},
			{'ё', "e"},
			{'ж', "zh"},
			{'з', "z"},
			{'и', "i"},
			{'й', "y"},
			{'к', "k"},
			{'л', "l"},
			{'м', "m"},
			{'н', "n"},
			{'о', "o"},
			{'п', "p"},
			{'р', "r"},
			{'с', "s"},
			{'т', "t"},
			{'у', "u"},
			{'ф', "f"},
			{'х', "h"},
			{'ц', "c"},
			{'ч', "ch"},
			{'ш', "sh"},
			{'щ', "shh"},
			{'ы', "i"},
			{'э', "e"},
			{'ю', "yu"},
			{'я', "ya"},
			{' ', "-"}
		};

		/// <summary>
		/// Транслитерация
		/// </summary>
		/// <param name="source">строка для транслитерации</param>
		/// <returns>результат транслитерации</returns>
		public static string Transliterate(string source)
		{
			var transliteration = new string(source.ToLower().SelectMany(c => Transliterate(c)).ToArray());
			var segments =	transliteration.Split(new[]{'-'}, StringSplitOptions.RemoveEmptyEntries);
			return string.Join("-", segments);
		}

		private static string Transliterate(char source)
		{
			string result;

			if (symbolMap.TryGetValue(source, out result))
			{
				return result;
			}
			else if (source == '-' || source >= 'a' && source <= 'z' || char.IsDigit(source))
			{
				return source.ToString();
			}
			else
			{
				return string.Empty;
			}
		}
	}
}
