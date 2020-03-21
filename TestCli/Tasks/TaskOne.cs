using CommandLineInterface;

namespace TestCli.Tasks
{
    public class TaskOne : ITask
    {
        private readonly IConsole _console;

        public TaskOne(IConsole console)
        {
            _console = console;
        }

        public void Run()
        {
            _console.WriteInfo("Running TaskOne");
        }
    }
}