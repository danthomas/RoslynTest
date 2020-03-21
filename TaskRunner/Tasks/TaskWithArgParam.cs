using CommandLineInterface;

namespace Tests.Tasks
{
    public class TaskWithArgParam : ITask
    {
        private readonly IConsole _console;

        public TaskWithArgParam(IConsole console)
        {
            _console = console;
        }

        public class Args
        {
            public bool BoolProp { get; set; }
            public string StringProp { get; set; }
        }

        public void Run(Args args)
        {
            _console.WriteInfo($"TaskWithArgParam {args.StringProp} {args.BoolProp}");
        }
    }
}