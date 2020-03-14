using CommandLineInterface;

namespace TestCli.Tasks
{
    public class GitCommit : ITask
    {
        private readonly IConsole _console;

        public GitCommit(IConsole console)
        {
            _console = console;
        }

        public void Run()
        {
            _console.WriteLine("Running GitCommit");
        }
    }
}