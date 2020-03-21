using CommandLineInterface;

namespace TestCli.Tasks
{
    public class EditCode : ITask
    {
        private readonly IConsole _console;

        public EditCode(IConsole console)
        {
            _console = console;
        }

        public void Run()
        {
            _console.WriteInfo("Running EditCode");
        }
    }
}