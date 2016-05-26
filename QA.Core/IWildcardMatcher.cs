using System;
using System.Collections.Generic;
namespace QA.Core
{
    public interface IWildcardMatcher
    {
        IEnumerable<string> Match(string text);
        string MatchLongest(string text);
    }
}
