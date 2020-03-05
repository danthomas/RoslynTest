using System.Text.Json;
using CommandLineInterface;
using NUnit.Framework;

namespace Tests.CommandLineInterfaceTests
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
  'Namespace': 'Tests.CommandLineInterfaceTests'
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
  'ArgsType': 'Tests.CommandLineInterfaceTests.TaskB.Args',
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
      'Type': 'Tests.CommandLineInterfaceTests.TaskB.Args',
      'IsArgs': true,
      'Namespace': 'Tests.CommandLineInterfaceTests'
    },
    {
      'Name': 'solution',
      'Type': 'Tests.CommandLineInterfaceTests.Solution',
      'IsArgs': false,
      'Namespace': 'Tests.CommandLineInterfaceTests'
    }
  ],
  'Namespace': 'Tests.CommandLineInterfaceTests'
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
  'Namespace': 'Tests.CommandLineInterfaceTests'
}", actual);
        }
    }
}