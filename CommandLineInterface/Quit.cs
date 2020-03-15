namespace CommandLineInterface
{
    public class Quit : ITask
    {
        private readonly IConsole _console;

        public Quit(IConsole console)
        {
            _console = console;
        }
            
        public void Run()
        {
            _console.Quit = true;
        }
    }
}