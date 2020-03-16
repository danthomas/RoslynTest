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

        public RunResult Run(IRunTaskCommand runTaskCommand)
        {
            RunResult runResult = new RunResult();
            runResult.Success = true;
            if (runTaskCommand.Name == "ClearConsole")
            {
                _serviceProvider.GetService<ClearConsole>().Run();
            }
            else if (runTaskCommand.Name == "EditCode")
            {
                _serviceProvider.GetService<EditCode>().Run();
            }
            else if (runTaskCommand.Name == "EditVisualStudio")
            {
                _serviceProvider.GetService<EditVisualStudio>().Run();
            }
            else if (runTaskCommand.Name == "Explore")
            {
                _serviceProvider.GetService<Explore>().Run();
            }
            else if (runTaskCommand.Name == "GitCheckout")
            {
                var args = new TestCli.Tasks.GitCheckout.Args();
                if (runTaskCommand.HasSwitch("b", "Branch", true) == false)
                {
                    runResult.Success = false;
                    runResult.Errors.Add("b Branch required.");
                }

                if (runResult.Success == false)
                {
                    return runResult;
                }

                args.Branch = runTaskCommand.GetValue<string>("b", "Branch", true);
                _serviceProvider.GetService<GitCheckout>().Run(args);
            }
            else if (runTaskCommand.Name == "GitCheckoutMaster")
            {
                _serviceProvider.GetService<GitCheckoutMaster>().Run();
            }
            else if (runTaskCommand.Name == "GitCommit")
            {
                _serviceProvider.GetService<GitCommit>().Run();
            }
            else if (runTaskCommand.Name == "GitCommitWip")
            {
                _serviceProvider.GetService<GitCommitWip>().Run();
            }
            else if (runTaskCommand.Name == "GitResetHardHead")
            {
                _serviceProvider.GetService<GitResetHardHead>().Run();
            }
            else if (runTaskCommand.Name == "TaskFour")
            {
                var args = new TestCli.Tasks.TaskFour.Args();
                if (runResult.Success == false)
                {
                    return runResult;
                }

                args.Name = runTaskCommand.GetValue<string>("n", "Name", false);
                _serviceProvider.GetService<TaskFour>().Run(args);
            }
            else if (runTaskCommand.Name == "TaskOne")
            {
                _serviceProvider.GetService<TaskOne>().Run();
            }
            else if (runTaskCommand.Name == "TaskThree")
            {
                var args = new TestCli.Tasks.TaskThree.Args();
                if (runTaskCommand.HasSwitch("n", "Name", true) == false)
                {
                    runResult.Success = false;
                    runResult.Errors.Add("n Name required.");
                }

                if (runResult.Success == false)
                {
                    return runResult;
                }

                args.Name = runTaskCommand.GetValue<string>("n", "Name", true);
                _serviceProvider.GetService<TaskThree>().Run(args);
            }
            else if (runTaskCommand.Name == "TaskTwo")
            {
                _serviceProvider.GetService<TaskTwo>().Run(_state.GetState<TestCli.Thing>());
            }
            else if (runTaskCommand.Name == "Quit")
            {
                _serviceProvider.GetService<Quit>().Run();
            }
            else if (runTaskCommand.Name == "Help")
            {
                var args = new CommandLineInterface.Help.Args();
                if (runTaskCommand.HasSwitch("n", "Name", true) == false)
                {
                    runResult.Success = false;
                    runResult.Errors.Add("n Name required.");
                }

                if (runResult.Success == false)
                {
                    return runResult;
                }

                args.Name = runTaskCommand.GetValue<string>("n", "Name", true);
                _serviceProvider.GetService<Help>().Run(args);
            }

            return runResult;
        }
    }
}