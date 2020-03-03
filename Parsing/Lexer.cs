using System;
using System.Collections.Generic;
using System.Linq;

namespace Parsing
{
    public class Lexer
    {
        private readonly List<Pattern> _patterns;
        private string _text;
        private int _index;
        private char[] _ignoreChars;

        public Lexer()
        {
            _index = 0;
            _patterns = new List<Pattern>();
            _ignoreChars = new char[0];
        }

        public Lexer TokenPattern(string tokenType, string text, bool caseSensitive = false)
        {
            _patterns.Add(new TokenPattern(tokenType, text, caseSensitive));
            return this;
        }

        public Lexer RegexPattern(string tokenType, string full, string partial, bool caseSensitive = false)
        {
            _patterns.Add(new RegexPattern(tokenType, full, partial, caseSensitive));
            return this;
        }

        public Lexer Init(string text)
        {
            _text = text;
            _index = 0;
            return this;
        }

        public string CurrentText()
        {
            var from = _index - 20;
            var to = _index + 20;
            from = Math.Max(from, 0);
            to = Math.Min(to, _text.Length - 1);

            return _text.Substring(from, to - from);
        }

        public string RemainingText()
        {
            return _text.Substring(_index);
        }

        public bool IsComplete => _index == _text.Length;

        public Token NextToken()
        {

            while (_index < _text.Length && _ignoreChars.Contains(_text[_index]))
            {
                _index++;
            }

            var matches = new List<Match>();

            var length = 1;

            while (_index + length <= _text.Length)
            {
                var text = _text.Substring(_index, length);
                // // //console.log('matching \'' + text + '\'')
                var newMatches = new List<Match>();

                foreach (var pattern in _patterns)
                {

                    var matchType = pattern.IsMatch(text);

                    if (matchType != MatchType.No)
                    {
                        // // //console.log('found ' + matchType + ' ' + pattern.TokenType)
                        newMatches.Add(new Match(pattern.TokenType, text, pattern, matchType));
                    }
                }

                if (newMatches.Count == 0)
                {
                    break;
                }

                matches = MergeMatches(newMatches, matches);

                length++;
            }

            matches = GetYesMatches(matches);

            if (matches.Count > 0)
            {
                matches = GetMaxTextLengthMatches(matches);
                // // //console.log('matches')
                // for (var x in matches) {
                //   // //console.log('  ' + matches[x].TokenType + ' ' + matches[x].matchType)
                // }

                _index += matches[0].Text.Length;
                return GetToken(matches);
            }

            return null;
        }

        private Token GetToken(List<Match> matches)
        {
            var text = "";
            var tokenTypes = new List<string>();

            if (matches.Count > 0)
            {
                foreach (var match in matches)
                {

                    text = match.Text;
                    tokenTypes.Add(match.TokenType);
                }

                if (tokenTypes.Count > 0)
                {
                    return new Token(tokenTypes, text);
                }
            }

            return null;
        }

        List<Match> GetYesMatches(List<Match> matches)
        {
            var yesMatches = new List<Match>();
            foreach (var match in matches)
            {

                if (match.MatchType == MatchType.Yes)
                {
                    yesMatches.Add(match);
                }
            }

            return yesMatches;
        }

        List<Match> GetMaxTextLengthMatches(List<Match> matches)
        {
            var maxLength = 0;

            foreach (var match in matches)
            {
                if (match.Text.Length > maxLength)
                {
                    maxLength = match.Text.Length;
                }
            }

            var maxTextLengthMatches = new List<Match>();

            foreach (var match in matches)
            {
                if (match.Text.Length == maxLength)
                {
                    maxTextLengthMatches.Add(match);
                }
            }

            return maxTextLengthMatches;
        }

        private List<Match> MergeMatches(List<Match> newMatches, List<Match> oldMatches)
        {
            var matches = new List<Match>();
            // // //console.log('merging  ')

            foreach (var oldMatch in oldMatches)
            {

                var found = false;
                foreach (var newMatch in newMatches)
                {

                    if (oldMatch.Pattern == newMatch.Pattern)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    matches.Add(oldMatch);
                }
            }

            foreach (var newMatch in newMatches)
            {

                Match oldMatch = null;

                foreach (var y in oldMatches)
                {

                    if (newMatch.Pattern == y.Pattern)
                    {
                        oldMatch = y;
                        break;
                    }
                }

                if (oldMatch != null && oldMatch.MatchType == MatchType.Yes && newMatch.MatchType == MatchType.Partial)
                {
                    matches.Add(oldMatch);
                }
                else
                {
                    matches.Add(newMatch);
                }
            }

            return matches;
        }
    }
}

