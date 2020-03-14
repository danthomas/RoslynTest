using CommandLineInterface;

namespace TestCli.Tasks
{
    public class GitCommitWip : ITask
    {
        private readonly IConsole _console;

        public GitCommitWip(IConsole console)
        {
            _console = console;
        }

        public void Run()
        {
            _console.WriteLine("Running GitCommitWip");
        }
    }
}