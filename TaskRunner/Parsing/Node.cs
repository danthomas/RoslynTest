using System.Collections.Generic;

namespace TaskRunner.Parsing
{
    public class Node
    {
        public string TokenType { get; }
        public string Text { get; }

        public Node(string tokenType, string text)
        {
            TokenType = tokenType;
            Text = text;
            Nodes = new List<Node>();
        }

        public List<Node> Nodes { get; set; }

        public override string ToString()
        {
            return TokenType + (Text == "" ? "" : " " + Text);
        }
    }
}