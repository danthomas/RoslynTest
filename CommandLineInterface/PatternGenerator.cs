using System.Collections.Generic;
using System.Linq;

namespace CommandLineInterface
{
    public class PatternGenerator
    {
        public List<List<string>> Generate(string pattern)
        {
            var parent = new Node(null, "");
            Recurse(pattern, parent);
            var list = new List<List<string>>();
            Walk(parent, list);
            return list;
        }

        private void Walk(Node parent, List<List<string>> list)
        {
            if (!parent.Children.Any())
            {
                var n = parent;
                var texts = new List<string>();
                list.Add(texts);
                while (n.Text != "")
                {
                    texts.Insert(0, n.Text);
                    n = n.Parent;
                }
            }
            else
            {
                foreach (var child in parent.Children)
                {
                    Walk(child, list);
                }
            }
        }

        private void Recurse(string pattern, Node parent)
        {
            for (var i = 1; i <= pattern.Length; ++i)
            {
                var child = new Node(parent, pattern.Substring(0, i));
                parent.Children.Add(child);
                Recurse(pattern.Substring(i), child);
            }
        }

        class Node
        {
            public Node Parent { get; }

            public Node(Node parent, string text)
            {
                Parent = parent;
                Text = text;
                Children = new List<Node>();
            }

            public string Text { get; }
            public List<Node> Children { get; }
        }
    }
}