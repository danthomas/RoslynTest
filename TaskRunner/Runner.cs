using System;
using Microsoft.Extensions.DependencyInjection;
using TaskRunner;

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
            if (runTaskCommand.Name == "TaskA")
            {
                _serviceProvider.GetService<TaskA>().Run();
            }
            else if (runTaskCommand.Name == "TaskB")
            {
                var args = new TaskB.Args();
                args.BoolProp = runTaskCommand.GetValue<bool>("BoolProp");
                args.StringProp = runTaskCommand.GetValue<string>("StringProp");
                _serviceProvider.GetService<TaskB>().Run(args, _state.GetState<Solution>());
            }
        }
    }
}