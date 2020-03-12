using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace CommandLineInterface
{
    public class App
    {
        public void Run(IConsole console, IState state, Assembly assembly)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(console);

            var quit = new Quit();

            serviceCollection.AddSingleton(quit);

            foreach (var type in assembly.GetTypes()
                .Where(x => x.IsClass && x.GetInterfaces().Contains(typeof(ITask))))
            {
                serviceCollection.AddTransient(type);
            }

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var taskRunner = new TaskRunnerBuilder().Build(serviceProvider, state, assembly, typeof(Quit));

            while (!quit.HasQuit)
            {
                var line = console.ReadLine();
                var runTaskCommand = new RunTaskCommand(line);

                var runResult = taskRunner.Run(runTaskCommand);

                if (!runResult.Success)
                {
                    foreach (var error in runResult.Errors)
                    {
                        console.WriteLine(error);
                    }
                }
            }
        }
    }
}