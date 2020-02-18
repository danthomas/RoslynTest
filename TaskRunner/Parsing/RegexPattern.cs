using System.Text.RegularExpressions;

namespace TaskRunner.Parsing
{
    class RegexPattern : Pattern
    {
        public RegexPattern(string tokenType, string full, string partial, bool caseSensitive)
        : base(tokenType)
        {
            Full = new Regex(full);
            Partial = new Regex(partial);
            CaseSensitive = caseSensitive;
        }

        public Regex Full { get; }
        public Regex Partial { get; }
        public bool CaseSensitive { get; }
        
        public override MatchType IsMatch(string text)
        {
            return this.Full.IsMatch(text)
                ? MatchType.Yes
                : this.Partial.IsMatch(text)
                    ? MatchType.Partial
                    : MatchType.No;
        }
    }
}