using System;
using CommandLineInterface;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.CommandLineInterfaceTests
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
            if (runTaskCommand.Name == "TaskA")
            {
                _serviceProvider.GetService<TaskA>().Run();
            }
            else if (runTaskCommand.Name == "TaskB")
            {
                var args = new TaskB.Args();
                args.BoolProp = runTaskCommand.GetValue<Boolean>("bp", "BoolProp", false);
                args.StringProp = runTaskCommand.GetValue<String>("sp", "StringProp", false);
                _serviceProvider.GetService<TaskB>().Run(args, _state.GetState<Solution>());
            }
            else if (runTaskCommand.Name == "TaskC")
            {
                _serviceProvider.GetService<TaskC>().Run();
            }
        }
    }
}