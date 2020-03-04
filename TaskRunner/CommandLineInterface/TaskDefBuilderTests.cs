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
  'ArgsPropDefs': [],
  'ParamDefs': [],
  'Namespace': 'Tests.CommandLineInterface'
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
      'Type': 'Boolean',
      'Namespace': 'System'
    },
    {
      'Name': 'StringProp',
      'Type': 'String',
      'Namespace': 'System'
    }
  ],
  'ParamDefs': [
    {
      'Name': 'args',
      'Type': 'Tests.CommandLineInterface.TaskB.Args',
      'IsArgs': true,
      'Namespace': 'Tests.CommandLineInterface'
    },
    {
      'Name': 'solution',
      'Type': 'Tests.CommandLineInterface.Solution',
      'IsArgs': false,
      'Namespace': 'Tests.CommandLineInterface'
    }
  ],
  'Namespace': 'Tests.CommandLineInterface'
}", actual);
        }

        [Test]
        public void NoArgsParams()
        {
            var taskDef = new TaskDefBuilder().Build(typeof(TaskC))[0];

            var actual = JsonSerializer.Serialize(taskDef, new JsonSerializerOptions
            {
                WriteIndented = true,
            }).Replace("\"", "'");

            Assert.AreEqual(@"{
  'Name': 'TaskC',
  'ArgsType': '',
  'ArgsPropDefs': [],
  'ParamDefs': [],
  'Namespace': 'Tests.CommandLineInterface'
}", actual);
        }
    }
}