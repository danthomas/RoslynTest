using CommandLineInterface;

namespace Tests.CommandLineInterfaceTests
{
    public class TaskC : ITask
    {
        private readonly IConsole _console;

        public TaskC(IConsole console)
        {
            _console = console;
        }

        public void Run()
        {
            _console.WriteLine("TaskA");
        }
    }
}