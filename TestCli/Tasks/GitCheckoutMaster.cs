using CommandLineInterface;

namespace TestCli.Tasks
{
    public class GitCheckoutMaster : ITask
    {
        private readonly IConsole _console;

        public GitCheckoutMaster(IConsole console)
        {
            _console = console;
        }

        public void Run()
        {
            _console.WriteLine("Running GitCheckoutMaster");
        }
    }
}