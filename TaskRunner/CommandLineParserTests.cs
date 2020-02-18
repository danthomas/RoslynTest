using System.Text;
using NUnit.Framework;
using TaskRunner.Parsing;

namespace TaskRunner
{
    public class CommandLineParserTests
    {
        [Test]
        public void TaskNameOnly()
        {
            var node = new CommandLineParser().Parse("TaskName");

            var actual = NodeToString(node);

            Assert.AreEqual(@"CommandLine 
  Identifier TaskName", actual);
        }

        [Test]
        public void DefaultValue()
        {
            var node = new CommandLineParser().Parse("TaskName value");

            var actual = NodeToString(node);

            Assert.AreEqual(@"CommandLine 
  Identifier TaskName
  Argument 
    Value value", actual);
        }

        [Test]
        public void DefaultStringValue()
        {
            var node = new CommandLineParser().Parse("TaskName \"value\"");

            var actual = NodeToString(node);

            Assert.AreEqual(@"CommandLine 
  Identifier TaskName
  Argument 
    StringValue ""value""", actual);
        }

        [Test]
        public void DefaultValues()
        {
            var node = new CommandLineParser().Parse("TaskName [value1 \"value2\" value3]");

            var actual = NodeToString(node);

            Assert.AreEqual(@"CommandLine 
  Identifier TaskName
  Argument 
    MultipleValues 
      Value value1
      StringValue ""value2""
      Value value3", actual);
        }

        [Test]
        public void Switch()
        {
            var node = new CommandLineParser().Parse("TaskName -switch");

            var actual = NodeToString(node);

            Assert.AreEqual(@"CommandLine 
  Identifier TaskName
  Argument 
    Identifier switch", actual);
        }

        [Test]
        public void SwitchValue()
        {
            var node = new CommandLineParser().Parse("TaskName -s value");

            var actual = NodeToString(node);

            Assert.AreEqual(@"CommandLine 
  Identifier TaskName
  Argument 
    Identifier s
    Value value", actual);
        }

        [Test]
        public void SwitchStringValue()
        {
            var node = new CommandLineParser().Parse("TaskName -s \"value\"");

            var actual = NodeToString(node);

            Assert.AreEqual(@"CommandLine 
  Identifier TaskName
  Argument 
    Identifier s
    StringValue ""value""", actual);
        }

        [Test]
        public void SwitchValues()
        {
            var node = new CommandLineParser().Parse("TaskName -s [value1 \"value2\" value3]");

            var actual = NodeToString(node);

            Assert.AreEqual(@"CommandLine 
  Identifier TaskName
  Argument 
    Identifier s
    MultipleValues 
      Value value1
      StringValue ""value2""
      Value value3", actual);
        }

        [Test]
        public void KeyValuePair()
        {
            var node = new CommandLineParser().Parse("TaskName -s key:value");

            var actual = NodeToString(node);

            Assert.AreEqual(@"CommandLine 
  Identifier TaskName
  Argument 
    Identifier s
    KeyValuePair 
      Identifier key
      Value value", actual);
        }

        [Test]
        public void KeyValuePairs()
        {
            var node = new CommandLineParser().Parse("TaskName -s [key1:value2 key2:value2 key3:value3]");

            var actual = NodeToString(node);

            Assert.AreEqual(@"CommandLine 
  Identifier TaskName
  Argument 
    Identifier s
    MultipleValues 
      KeyValuePair 
        Identifier key1
        Value value2
      KeyValuePair 
        Identifier key2
        Value value2
      KeyValuePair 
        Identifier key3
        Value value3", actual);
        }

        [Test]
        public void Switches()
        {
            var node = new CommandLineParser().Parse("TaskName -s1 -s2 value6 -s3 \"value7\" -s4 [value8 \"value9\" value10]");

            var actual = NodeToString(node);

            Assert.AreEqual(@"CommandLine 
  Identifier TaskName
  Argument 
    Identifier s1
  Argument 
    Identifier s2
    Value value6
  Argument 
    Identifier s3
    StringValue ""value7""
  Argument 
    Identifier s4
    MultipleValues 
      Value value8
      StringValue ""value9""
      Value value10", actual);
        }

        private string NodeToString(Node node)
        {
            var stringBuilder = new StringBuilder();

            void NodeToStringChild(Node parent, int indent = 0)
            {
                foreach (var child in parent.Nodes)
                {
                    stringBuilder.AppendLine($"{new string(' ', indent * 2)}{child.TokenType} {child.Text}");
                    NodeToStringChild(child, indent + 1);
                }
            }

            NodeToStringChild(node);
            return stringBuilder.ToString().Trim();
        }
    }
}