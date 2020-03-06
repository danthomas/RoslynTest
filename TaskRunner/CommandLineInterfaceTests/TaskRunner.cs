using System;
using Microsoft.Extensions.DependencyInjection;
using CommandLineInterface;
using Tests.Tasks;
using Tests.CommandLineInterfaceTests;

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
            if (runTaskCommand.Name == "TaskWithArgDefsAndParams")
            {
                var args = new Tests.Tasks.TaskWithArgDefsAndParams.Args();
                args.BoolProp = runTaskCommand.GetValue<bool>("bp", "BoolProp", false);
                args.StringProp = runTaskCommand.GetValue<string>("sp", "StringProp", true);
                _serviceProvider.GetService<TaskWithArgDefsAndParams>().Run(args, _state.GetState<Tests.CommandLineInterfaceTests.Solution>());
            }
            else if (runTaskCommand.Name == "TaskWithArgParam")
            {
                var args = new Tests.Tasks.TaskWithArgParam.Args();
                args.BoolProp = runTaskCommand.GetValue<bool>("bp", "BoolProp", false);
                args.StringProp = runTaskCommand.GetValue<string>("sp", "StringProp", false);
                _serviceProvider.GetService<TaskWithArgParam>().Run(args);
            }
            else if (runTaskCommand.Name == "TaskWithNoArgs")
            {
                _serviceProvider.GetService<TaskWithNoArgs>().Run();
            }
        }
    }
}