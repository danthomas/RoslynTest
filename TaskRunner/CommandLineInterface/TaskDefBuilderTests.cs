using System.Text.Json;
using CommandLineInterface;
using NUnit.Framework;

namespace Tests.CommandLineInterface
{
    public class TaskDefBuilderTests
    {
        [Test]
        public void NoArgsNoParams()
        {
            var taskDef = new TaskDefBuilder().Build(typeof(TaskA))[0];

            var actual = JsonSerializer.Serialize(taskDef, new JsonSerializerOptions
            {
                WriteIndented = true
            }).Replace("\"", "'");

            Assert.AreEqual(@"{
  'Name': 'TaskA',
  'ArgsType': '',
  'ArgsPropDefs': null,
  'ParamDefs': []
}", actual);
        }

        [Test]
        public void ArgsAndParams()
        {
            var taskDef = new TaskDefBuilder().Build(typeof(TaskB))[0];

            var actual = JsonSerializer.Serialize(taskDef, new JsonSerializerOptions
            {
                WriteIndented = true,
            }).Replace("\"", "'");

            Assert.AreEqual(@"{
  'Name': 'TaskB',
  'ArgsType': 'Tests.CommandLineInterface.TaskB.Args',
  'ArgsPropDefs': [
    {
      'Name': 'BoolProp',
      'Type': 'Boolean'
    },
    {
      'Name': 'StringProp',
      'Type': 'String'
    }
  ],
  'ParamDefs': [
    {
      'Name': 'args',
      'Type': 'Tests.CommandLineInterface.TaskB.Args',
      'IsArgs': true
    },
    {
      'Name': 'solution',
      'Type': 'Tests.CommandLineInterface.Solution',
      'IsArgs': false
    }
  ]
}", actual);
        }
    }
}