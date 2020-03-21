using CommandLineInterface;

namespace Tests.Tasks
{
    public class TaskWithNoArgs : ITask
    {
        private readonly IConsole _console;

        public TaskWithNoArgs(IConsole console)
        {
            _console = console;
        }

        public void Run()
        {
            _console.WriteInfo("TaskWithNoArgs");
        }
    }
}