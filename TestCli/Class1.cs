using System;
using Microsoft.Extensions.DependencyInjection;
using CommandLineInterface;
using TestCli;

namespace DynamicTaskRunner
{
    public class TaskRunner : ITaskRunner
    {
        readonly IServiceProvider _serviceProvider;
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
        }
    }
}