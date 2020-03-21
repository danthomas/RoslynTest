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
            _console.WriteInfo("TaskThree " + args.Name);
        }

        public class Args
        {
            public string Name { get; set; }
        }

        public ArgDefs<Args> ArgDefs => new ArgDefs<Args>().DefaultRequired(x => x.Name, "n");
    }
}