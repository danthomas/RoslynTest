namespace Parsing
{
    public class Match
    {
        public string TokenType { get; }
        public string Text { get; }
        public Pattern Pattern { get; }
        public MatchType MatchType { get; }

        public Match(string tokenType, string text, Pattern pattern, MatchType matchType)
        {
            TokenType = tokenType;
            Text = text;
            Pattern = pattern;
            MatchType = matchType;
        }
    }
}