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
            _console.WriteInfo("Running GitResetHardHead");
        }
    }
}