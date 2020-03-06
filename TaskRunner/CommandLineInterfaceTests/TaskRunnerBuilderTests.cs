using System;
using CommandLineInterface;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Tests.Tasks;

namespace Tests.CommandLineInterfaceTests
{
    public class TaskRunnerBuilderTests
    {
        [Test]
        public void TaskATest()
        {
            var serviceCollection = new ServiceCollection();
            var console = new Console();

            serviceCollection.AddTransient<TaskWithNoArgs>();
            serviceCollection.AddTransient<TaskWithArgParam>();
            serviceCollection.AddTransient<TaskWithArgDefsAndParams>();
            serviceCollection.AddSingleton<IConsole>(console);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IState state = new State
            {
                Solution = new Solution { Id = 123 }
            };

            var taskRunner = new TaskRunnerBuilder()
                .Build(serviceProvider, state, typeof(TaskWithArgParam).Assembly);

            //taskRunner = new DynamicTaskRunner.TaskRunner(serviceProvider, state);

            taskRunner.Run(new RunTaskCommand("TaskWithNoArgs"));

            taskRunner.Run(new RunTaskCommand("TaskWithArgParam -BoolProp true -StringProp ABCD"));

            taskRunner.Run(new RunTaskCommand("TaskWithArgDefsAndParams -BoolProp true -StringProp ABCD"));

            Assert.AreEqual(@"TaskWithNoArgs
TaskWithArgParam ABCD True
TaskWithArgDefsAndParams 123 ABCD True", console.Text);
        }
    }
}
