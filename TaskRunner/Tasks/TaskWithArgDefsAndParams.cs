using CommandLineInterface;
using Tests.CommandLineInterfaceTests;

namespace Tests.Tasks
{
    public class TaskWithArgDefsAndParams : ITask<TaskWithArgDefsAndParams.Args>
    {
        private readonly IConsole _console;

        public TaskWithArgDefsAndParams(IConsole console)
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
            _console.WriteLine($"TaskWithArgDefsAndParams {solution.Id} {args.StringProp} {args.BoolProp}");
        }

        public ArgDefs<Args> ArgDefs => new ArgDefs<Args>()
            .DefaultRequired(x => x.StringProp, "sp")
            .Optional(x => x.BoolProp, "bp");
    }
}