using System.Collections.Generic;
using CommandLineInterface;
using NUnit.Framework;

namespace Tests.CommandLineInterfaceTests
{
    public class RunTaskCommandTests
    {
        [Test]
        public void NamesWithSingleValue()
        {
            var runTaskCommand = new RunTaskCommand("task -bool -byte 123 -short 456 -int 789 -long 123456789 -string ABCD -keyValuePair key:value");

            Assert.AreEqual("task", runTaskCommand.Name);
            Assert.AreEqual(true, runTaskCommand.GetValue<bool>("b", "bool", false));
            Assert.AreEqual((byte)123, runTaskCommand.GetValue<byte>("by", "byte", false));
            Assert.AreEqual((short)456, runTaskCommand.GetValue<short>("sh", "short", false));
            Assert.AreEqual(789, runTaskCommand.GetValue<int>("i", "int", false));
            Assert.AreEqual((long)123456789, runTaskCommand.GetValue<long>("l", "long", false));
            Assert.AreEqual("ABCD", runTaskCommand.GetValue<string>("s", "string", false));
            Assert.AreEqual("key", runTaskCommand.GetKeyValuePair<string, string>("kvp", "keyValuePair", false).Key);
            Assert.AreEqual("value", runTaskCommand.GetKeyValuePair<string, string>("kvp", "keyValuePair", false).Value);
        }

        [Test]
        public void SwitchesWithSingleValue()
        {
            var runTaskCommand = new RunTaskCommand("task -b -by 123 -sh 456 -i 789 -l 123456789 -s ABCD -kvp key:value");

            Assert.AreEqual("task", runTaskCommand.Name);
            Assert.AreEqual(true, runTaskCommand.GetValue<bool>("b", "bool", false));
            Assert.AreEqual((byte)123, runTaskCommand.GetValue<byte>("by", "byte", false));
            Assert.AreEqual((short)456, runTaskCommand.GetValue<short>("sh", "short", false));
            Assert.AreEqual(789, runTaskCommand.GetValue<int>("i", "int", false));
            Assert.AreEqual((long)123456789, runTaskCommand.GetValue<long>("l", "long", false));
            Assert.AreEqual("ABCD", runTaskCommand.GetValue<string>("s", "string", false));
            Assert.AreEqual("key", runTaskCommand.GetKeyValuePair<string, string>("kvp", "keyValuePair", false).Key);
            Assert.AreEqual("value", runTaskCommand.GetKeyValuePair<string, string>("kvp", "keyValuePair", false).Value);
        }

        [Test]
        public void SwitchesWithMultipleStringValues()
        {
            var runTaskCommand = new RunTaskCommand("task -strings [ABC DEF GHI]");

            Assert.AreEqual("task", runTaskCommand.Name);
            
            var values = runTaskCommand.GetValues<string>("s", "strings", false);

            Assert.AreEqual("ABC", values[0]);
            Assert.AreEqual("DEF", values[1]);
            Assert.AreEqual("GHI", values[2]);
        }

        [Test]
        public void SwitchWithMultipleIntValues()
        {
            var runTaskCommand = new RunTaskCommand("task -ints [123 456 789]");

            Assert.AreEqual("task", runTaskCommand.Name);
            
            var values = runTaskCommand.GetValues<int>("i", "ints", false);

            Assert.AreEqual(123, values[0]);
            Assert.AreEqual(456, values[1]);
            Assert.AreEqual(789, values[2]);
        }

        [Test]
        public void SwitchWithMultipleKeyValuePairs()
        {
            var runTaskCommand = new RunTaskCommand("task -kvps [A:123 B:456 C:789]");

            Assert.AreEqual("task", runTaskCommand.Name);
            
            var keyValuePairs = runTaskCommand.GetKeyValuePairs<string, int>("kvps", "keyValuePairs", false);

            Assert.AreEqual(new KeyValuePair<string, int>("A", 123), keyValuePairs[0]);
            Assert.AreEqual(new KeyValuePair<string, int>("B", 456), keyValuePairs[1]);
            Assert.AreEqual(new KeyValuePair<string, int>("C", 789), keyValuePairs[2]);
        }
    }
}