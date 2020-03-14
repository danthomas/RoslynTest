using CommandLineInterface;

namespace TestCli.Tasks
{
    public class EditVisualStudio : ITask
    {
        private readonly IConsole _console;

        public EditVisualStudio(IConsole console)
        {
            _console = console;
        }

        public void Run()
        {
            _console.WriteLine("Running EditVisualStudio");
        }
    }
}