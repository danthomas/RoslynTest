using CommandLineInterface;

namespace TestCli.Tasks
{
    public class TaskThree : ITask<TaskThree.Args>
    {
        private readonly IConsole _console;

        public TaskThree(IConsole console)
        {
            _console = console;
        }

        public void Run(Args args)
        {
            _console.WriteLine("TaskThree " + args.Name);
        }

        public class Args
        {
            public string Name { get; set; }
        }

        public ArgDefs<Args> ArgDefs => new ArgDefs<Args>().DefaultRequired(x => x.Name, "-n");
    }

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