using CommandLineInterface;

namespace Tests.CommandLineInterfaceTests
{
    public class TaskA : ITask
    {
        private readonly IConsole _console;

        public TaskA(IConsole console)
        {
            _console = console;
        }

        public void Run()
        {
            _console.WriteLine("TaskA");    
        }
    }
}