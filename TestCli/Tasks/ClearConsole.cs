using CommandLineInterface;

namespace TestCli.Tasks
{
    public class ClearConsole : ITask
    {
        private readonly IConsole _console;

        public ClearConsole(IConsole console)
        {
            _console = console;
        }

        public void Run()
        {
            _console.Clear();
        }
    }
}