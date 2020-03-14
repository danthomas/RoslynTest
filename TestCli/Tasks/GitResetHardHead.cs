using CommandLineInterface;

namespace TestCli.Tasks
{
    public class GitResetHardHead : ITask
    {
        private readonly IConsole _console;

        public GitResetHardHead(IConsole console)
        {
            _console = console;
        }

        public void Run()
        {
            _console.WriteLine("Running GitResetHardHead");
        }
    }
}