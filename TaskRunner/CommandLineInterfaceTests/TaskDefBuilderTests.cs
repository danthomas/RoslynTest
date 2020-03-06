using System.Text.Json;
using CommandLineInterface;
using NUnit.Framework;
using Tests.Tasks;

namespace Tests.CommandLineInterfaceTests
{
    public class TaskDefBuilderTests
    {

        [Test]
        public void TaskWithNoArgs()
        {
            var taskDef = new TaskDefBuilder().Build(typeof(TaskWithNoArgs))[0];

            var actual = JsonSerializer.Serialize(taskDef, new JsonSerializerOptions
            {
                WriteIndented = true,
            }).Replace("\"", "'");

            Assert.AreEqual(@"{
  'Name': 'TaskWithNoArgs',
  'ArgsType': '',
  'ArgsPropDefs': [],
  'ParamDefs': [],
  'Namespace': 'Tests.Tasks'
}", actual);
        }

        [Test]
        public void TaskWithArgParam()
        {
            var taskDef = new TaskDefBuilder().Build(typeof(TaskWithArgParam))[0];

            var actual = JsonSerializer.Serialize(taskDef, new JsonSerializerOptions
            {
                WriteIndented = true
            }).Replace("\"", "'");

            Assert.AreEqual(@"{
  'Name': 'TaskWithArgParam',
  'ArgsType': 'Tests.Tasks.TaskWithArgParam.Args',
  'ArgsPropDefs': [
    {
      'Name': 'BoolProp',
      'Type': 'Boolean',
      'Namespace': 'System',
      'Switch': 'bp',
      'IsDefault': false
    },
    {
      'Name': 'StringProp',
      'Type': 'String',
      'Namespace': 'System',
      'Switch': 'sp',
      'IsDefault': false
    }
  ],
  'ParamDefs': [
    {
      'Name': 'args',
      'Type': 'Tests.Tasks.TaskWithArgParam.Args',
      'IsArgs': true,
      'Namespace': 'Tests.Tasks'
    }
  ],
  'Namespace': 'Tests.Tasks'
}", actual);
        }

        [Test]
        public void TaskWithArgDefsAndParams()
        {
            var taskDef = new TaskDefBuilder().Build(typeof(TaskWithArgDefsAndParams))[0];

            var actual = JsonSerializer.Serialize(taskDef, new JsonSerializerOptions
            {
                WriteIndented = true,
            }).Replace("\"", "'");

            Assert.AreEqual(@"{
  'Name': 'TaskWithArgDefsAndParams',
  'ArgsType': 'Tests.Tasks.TaskWithArgDefsAndParams.Args',
  'ArgsPropDefs': [
    {
      'Name': 'BoolProp',
      'Type': 'Boolean',
      'Namespace': 'System',
      'Switch': 'bp',
      'IsDefault': false
    },
    {
      'Name': 'StringProp',
      'Type': 'String',
      'Namespace': 'System',
      'Switch': 'sp',
      'IsDefault': true
    }
  ],
  'ParamDefs': [
    {
      'Name': 'args',
      'Type': 'Tests.Tasks.TaskWithArgDefsAndParams.Args',
      'IsArgs': true,
      'Namespace': 'Tests.Tasks'
    },
    {
      'Name': 'solution',
      'Type': 'Tests.CommandLineInterfaceTests.Solution',
      'IsArgs': false,
      'Namespace': 'Tests.CommandLineInterfaceTests'
    }
  ],
  'Namespace': 'Tests.Tasks'
}", actual);
        }
    }
}