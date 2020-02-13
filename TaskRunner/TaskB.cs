namespace TaskRunner
{
    public class TaskB : ITask<TaskB.Args>
    {
        private readonly IConsole _console;

        public TaskB(IConsole console)
        {
            _console = console;
        }

        public class Args
        {
            public bool BoolProp { get; set; }
            public string StringProp { get; set; }
        }

        public void Run(Args args, Solution solution)
        {
            _console.WriteLine($"TaskB {solution.Id} {args.StringProp} {args.BoolProp}");
        }

        public ArgDefs<Args> ArgDefs => new ArgDefs<Args>()
            .DefaultRequired(x => x.StringProp, "sp")
            .Optional(x => x.BoolProp, "sp");
    }
}