using CommandLineInterface;

namespace TestCli.Tasks
{
    public class Explore : ITask
    {
        private readonly IConsole _console;

        public Explore(IConsole console)
        {
            _console = console;
        }

        public void Run()
        {
            _console.WriteInfo("Running Explore");
        }
    }
}