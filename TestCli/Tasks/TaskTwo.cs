using CommandLineInterface;

namespace TestCli.Tasks
{
    public class TaskTwo : ITask
    {
        private readonly IConsole _console;

        public TaskTwo(IConsole console)
        {
            _console = console;
        }

        public void Run(Thing thing)
        {
            _console.WriteInfo("TaskTwo " + thing.Name);
        }
    }
}