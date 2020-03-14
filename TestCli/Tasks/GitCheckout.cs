using CommandLineInterface;

namespace TestCli.Tasks
{
    public class GitCheckout : ITask
    {
        private readonly IConsole _console;

        public GitCheckout(IConsole console)
        {
            _console = console;
        }

        public void Run()
        {
            _console.WriteLine("Running GitCheckout");
        }
    }
}