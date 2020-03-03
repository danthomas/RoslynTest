namespace Parsing
{
    class TokenPattern : Pattern
    {
        public string Text { get; }

        public override MatchType IsMatch(string text)
        {
            if (CaseSensitive && Text == text)
            {
                return MatchType.Yes;
            }

            if (!CaseSensitive && Text.ToLower() == text.ToLower())
            {
                return MatchType.Yes;
            }

            if (CaseSensitive && Text.IndexOf(text) == 0)
            {
                return MatchType.Partial;
            }

            if (!CaseSensitive && Text.ToLower().IndexOf(text.ToLower()) == 0)
            {
                return MatchType.Partial;
            }

            return MatchType.No;
        }

        public bool CaseSensitive { get; }

        public TokenPattern(string tokenType, string text, bool caseSensitive)
        : base(tokenType)
        {
            Text = text;
            CaseSensitive = caseSensitive;
        }
    }
}