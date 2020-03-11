using System;
using System.Collections.Generic;
using System.Linq;
using Parsing;

namespace CommandLineInterface
{
    public class RunTaskCommand : IRunTaskCommand
    {
        private readonly List<Argument> _arguments;

        public RunTaskCommand(string commandLine)
        {
            var node = new CommandLineParser().Parse(commandLine);

            Name = node.Nodes[0].Nodes[0].Text;

            _arguments = node.Nodes[0].Nodes.Where(x => x.TokenType == "Argument")
                .Select(x => new Argument
                {
                    Name = GetName(x),
                    Value = GetValue(x),
                    KeyValuePair = GetKeyValuePair(x),
                    Values = GetValues(x),
                    KeyValuePairs = GetKeyValuePairs(x)
                })
                .ToList();
        }

        public T GetValue<T>(string @switch, string name, bool isDefault)
        {
            var argument = GetArgument(@switch, name, isDefault);

            if (argument == null)
            {
                return default;
            }

            if (typeof(T) == typeof(bool) && string.IsNullOrWhiteSpace(argument.Value))
            {
                argument.Value = "true";
            }

            return (T)Convert.ChangeType(argument.Value, typeof(T));
        }

        public bool HasSwitch(string @switch, string name, bool isDefault)
        {
            return GetArgument(@switch, name, isDefault) != null;
        }

        private Argument GetArgument(string @switch, string name, bool isDefault)
        {
            return _arguments.SingleOrDefault(x => x.Name == @switch
                                                   || x.Name == name
                                                   || x.Name == "" && isDefault);
        }

        public KeyValuePair<K, V> GetKeyValuePair<K, V>(string @switch, string name, bool isDefault)
        {
            var argument = GetArgument(@switch, name, isDefault);

            if (argument == null)
            {
                return default;
            }

            return new KeyValuePair<K, V>((K)Convert.ChangeType(argument.KeyValuePair.Key, typeof(K)),
                (V)Convert.ChangeType(argument.KeyValuePair.Value, typeof(V)));
        }

        public List<T> GetValues<T>(string @switch, string name, bool isDefault)
        {
            return GetArgument(@switch, name, isDefault)?
                       .Values
                       .Select(x => (T)Convert.ChangeType(x, typeof(T)))
                       .ToList()
                   ?? new List<T>();
        }

        public List<KeyValuePair<K, V>> GetKeyValuePairs<K, V>(string @switch, string name, bool isDefault)
        {
            return GetArgument(@switch, name, isDefault)?
                       .KeyValuePairs?
                       .Select(x => new KeyValuePair<K, V>((K)Convert.ChangeType(x.Key, typeof(K)),
                           (V)Convert.ChangeType(x.Value, typeof(V))))
                       .ToList()
                   ?? new List<KeyValuePair<K, V>>();
        }

        private string GetName(Node node)
        {
            return node.Nodes.SingleOrDefault(x => x.TokenType == "Identifier")?.Text ?? "";
        }

        private string GetValue(Node node)
        {
            return node.Nodes.SingleOrDefault(x => x.TokenType == "Value" || x.TokenType == "StringValue")?.Text;
        }

        private KeyValuePair<string, string> GetKeyValuePair(Node node)
        {
            var valueNode = node.Nodes.SingleOrDefault(x => x.TokenType == "KeyValuePair");

            return valueNode == null
                ? new KeyValuePair<string, string>()
                : new KeyValuePair<string, string>(valueNode.Nodes[0].Text, valueNode.Nodes[1].Text);
        }

        private List<KeyValuePair<string, string>> GetKeyValuePairs(Node node)
        {
            var multipleValuesNode = node.Nodes.SingleOrDefault(x => x.TokenType == "MultipleValues");

            return multipleValuesNode?.Nodes
                .Where(x => x.TokenType == "KeyValuePair")
                .Select(x => new KeyValuePair<string, string>(x.Nodes[0].Text, x.Nodes[1].Text))
                .ToList();
        }

        private List<string> GetValues(Node node)
        {
            var multipleValuesNode = node.Nodes.SingleOrDefault(x => x.TokenType == "MultipleValues");

            return multipleValuesNode?.Nodes
                       .Where(x => x.TokenType == "Value" || x.TokenType == "StringValue")
                       .Select(x => x.Text)
                       .ToList() ?? new List<string>();
        }

        public string Name { get; }

        class Argument
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public List<string> Values { get; set; }
            public KeyValuePair<string, string> KeyValuePair { get; set; }
            public List<KeyValuePair<string, string>> KeyValuePairs { get; set; }
        }
    }
}