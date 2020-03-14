using CommandLineInterface;

namespace TestCli.Tasks
{
    public class TaskFour : ITask
    {
        private readonly IConsole _console;

        public TaskFour(IConsole console)
        {
            _console = console;
        }

        public void Run(Args args)
        {
            _console.WriteLine("TaskFour " + args.Name);
        }

        public class Args
        {
            public string Name { get; set; }
        }
    }
}