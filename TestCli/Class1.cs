using System;
using Microsoft.Extensions.DependencyInjection;
using CommandLineInterface;
using TestCli.Tasks;
using TestCli;

namespace DynamicTaskRunner
{
    public class TaskRunner : ITaskRunner
    {
        IServiceProvider _serviceProvider;
        IState _state;
        public TaskRunner(IServiceProvider serviceProvider, IState state)
        {
            _serviceProvider = serviceProvider;
            _state = state;
        }

        public void Run(IRunTaskCommand runTaskCommand)
        {
            if (runTaskCommand.Name == "TaskOne")
            {
                _serviceProvider.GetService<TaskOne>().Run();
            }
            else if (runTaskCommand.Name == "TaskThree")
            {
                var args = new TestCli.Tasks.TaskThree.Args();
                args.Name = runTaskCommand.GetValue<string>("n", "Name", true);
                _serviceProvider.GetService<TaskThree>().Run(args);
            }
            else if (runTaskCommand.Name == "TaskTwo")
            {
                _serviceProvider.GetService<TaskTwo>().Run(_state.GetState<TestCli.Thing>());
            }
        }
    }
}