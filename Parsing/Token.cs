using System.Collections.Generic;

namespace Parsing
{
    public class Token
    {
        public List<string> TokenTypes { get; }
        public string Text { get; }

        public Token(List<string> tokenTypes, string text)
        {
            TokenTypes = tokenTypes;
            Text = text;
        }
    }
}