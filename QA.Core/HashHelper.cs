using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA.Core
{
	public class HashHelper
	{
		public static int CombineHashCodes(int h1, int h2, params int[] hs)
		{
			int resultHashCode = CombineTwoHashCodes(h1, h2);

			foreach (int hashCode in hs)
				resultHashCode = CombineTwoHashCodes(resultHashCode, hashCode);

			return resultHashCode;
		}

		/// <summary>
		/// реализация взята из Tuple
		/// </summary>
		private static int CombineTwoHashCodes(int h1, int h2)
		{
			return ((h1 << 5) + h1) ^ h2;
		}
	}
}
