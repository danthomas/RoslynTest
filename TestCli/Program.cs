using System;
using System.Linq;
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

            var assembly = typeof(Program).Assembly;

            foreach (var type in assembly.GetTypes()
                .Where(x => x.IsClass && x.GetInterfaces().Contains(typeof(ITask))))
            {
                serviceCollection.AddTransient(type);
            }

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var state = new State
            {
                Thing = new Thing
                {
                    Name = "Abcd"
                }
            };

            var taskRunner = new TaskRunnerBuilder().Build(serviceProvider, state, assembly);
            //var taskRunner = new TaskRunner(serviceProvider, state);
            string line;

            while ((line = Console.ReadLine()) != "")
            {
                var runTaskCommand = new RunTaskCommand(line);

                var runResult = taskRunner.Run(runTaskCommand);

                if (!runResult.Success)
                {
                    foreach (var error in runResult.Errors)
                    {
                        Console.WriteLine(error);
                    }
                }
            }
        }
    }
}
