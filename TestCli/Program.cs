using System;
using Microsoft.Extensions.DependencyInjection;
using CommandLineInterface;

namespace TestCli
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<IConsole, ProgramConsole>();

            new TaskFactory(serviceCollection).AddTasks(typeof(Program).Assembly);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            IState state = null;
            
            var taskRunner = new TaskRunnerBuilder().Build(serviceProvider, state, typeof(Program).Assembly);
            //var taskRunner = new DynamicTaskRunner.TaskRunner(serviceProvider, state);
            string line;

            while ((line = Console.ReadLine()) != "")
            {
                taskRunner.Run(new RunTaskCommand(line));
            }
        }
    }



    class ProgramConsole : IConsole
    {
        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }
    }

    public class TaskOne : ITask
    {
        private readonly IConsole _console;

        public TaskOne(IConsole console)
        {
            _console = console;
        }

        public void Run()
        {
            _console.WriteLine("TaskOne");
        }
    }

}
