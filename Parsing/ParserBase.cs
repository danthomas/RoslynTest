using System;
using System.Collections.Generic;
using System.Linq;

namespace Parsing
{
    public abstract class ParserBase
    {
        private List<Token> _buffer;
        private readonly Lexer _lexer;

        protected ParserBase(Lexer lexer)
        {
            _lexer = lexer;
            _buffer = new List<Token>();
        }


        public Node Parse(string text)
        {
            _lexer.Init(text.Replace("\n", "\r\n").Replace("\r\r\n", "\r\n"));

            _buffer = new List<Token>();

            var root = new Node("Root", "");

            Root(root);

            if (!_lexer.IsComplete)
            {
                var message = $"Incomplete Parse at {_lexer.RemainingText()}.";

                throw new Exception();
            }

            if (_buffer.Any())
            {
                var message = $"Unexpected token {string.Join(" ", _buffer[0].TokenTypes)}.";

                throw new Exception();
            }

            return root;
        }

        public abstract void Root(Node root);

        protected Token GetToken(int index, bool consume)
        {
            Token token;
            while (_buffer.Count <= index)
            {
                token = _lexer.NextToken();

                if (token != null)
                {
                    _buffer.Add(token);
                }
                else
                {
                    break;
                }
            }

            if (_buffer.Count > index)
            {
                token = _buffer[index];


                if (consume)
                {
                    _buffer.RemoveAt(0);
                }

                return token;
            }

            return null;
        }

        protected Node Required(string tokenType, Node parent = null)
        {
            var token = GetToken(0, true);
            if (token == null)
            {
                throw new Exception(tokenType + " Token expected.");
            }

            if (!token.TokenTypes.Contains(tokenType))
            {
                var message = tokenType + " Token expected but ";
                foreach (var x in token.TokenTypes)
                {
                    message += " " + x;
                }

                message += " found.";
                throw new Exception();
            }

            var node = new Node(tokenType, token.Text);

            parent?.Nodes.Add(node);

            return node;
        }

        protected Node AddNode(Node parent, string tokenType)
        {
            var child = new Node(tokenType, "");
            parent.Nodes.Add(child);
            return child;
        }

        protected bool AreTokens(params string[] tokenTypes)
        {
            for (var i = 0; i < tokenTypes.Length; i++)
            {
                var tokenType = tokenTypes[i];
                var token = GetToken(i, false);

                if (token == null || !token.TokenTypes.Contains(tokenType))
                {
                    return false;
                }
            }

            return true;
        }
    }
}