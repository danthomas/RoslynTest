namespace Parsing
{
    public abstract class Pattern
    {
        protected Pattern(string tokenType)
        {
            TokenType = tokenType;
        }

        public string TokenType { get; }
        public abstract MatchType IsMatch(string text);
    }
}