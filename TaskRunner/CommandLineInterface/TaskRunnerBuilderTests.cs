using System;
using CommandLineInterface;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Tests.CommandLineInterface
{
    public class TaskRunnerBuilderTests
    {
        [Test]
        public void TaskATest()
        {
            var serviceCollection = new ServiceCollection();
            var console = new Console();

            serviceCollection.AddTransient<TaskA>();
            serviceCollection.AddTransient<TaskB>();
            serviceCollection.AddSingleton<IConsole>(console);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IState state = new State
            {
                Solution = new Solution { Id = 123 }
            };

            var taskRunner = new TaskRunnerBuilder()
                .Build(serviceProvider, state, typeof(TaskA).Assembly);

            taskRunner.Run(new RunTaskCommand("TaskA"));

            taskRunner.Run(new RunTaskCommand("TaskB -BoolProp true -StringProp ABCD"));

            Assert.AreEqual(@"TaskA
TaskB 123 ABCD True", console.Text);
        }
    }
}
