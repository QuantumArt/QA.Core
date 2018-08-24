using System.Collections.Generic;
#pragma warning disable 1591

namespace QA.Core
{
    public interface IWildcardMatcher
    {
        IEnumerable<string> Match(string text);
        string MatchLongest(string text);
    }
}
