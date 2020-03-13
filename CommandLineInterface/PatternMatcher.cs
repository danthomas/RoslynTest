using System;

namespace CommandLineInterface
{
    public class PatternMatcher
    {
        public MatchType Match(string name, string pattern)
        {
            var patternGen = new PatternGenerator();

            var patterns = patternGen.Generate(pattern).ToArray();

            var words = name.ToWords();

            foreach (var p in patterns)
            {
                if (p.Count > words.Length)
                {
                    continue;
                }

                var match = true;

                for (var i = 0; i < p.Count; i++)
                {
                    if (!words[i].StartsWith(p[i], StringComparison.CurrentCultureIgnoreCase))
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                {
                    return words.Length == p.Count 
                        ? MatchType.Full
                        : MatchType.Partial;
                }
            }

            return MatchType.None;
        }
    }
}