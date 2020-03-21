using CommandLineInterface;

namespace TestCli.Tasks
{
    public class GitCommit : ITask<GitCommit.Args>
    {
        private readonly IConsole _console;

        public GitCommit(IConsole console)
        {
            _console = console;
        }

        public void Run(Args args)
        {
            _console.WriteInfo($"git commit -m \"{args.Message}\"");
        }

        public class Args
        {
            public string Message { get; set; }
        }

        public ArgDefs<Args> ArgDefs => new ArgDefs<Args>()
            .DefaultRequired(x => x.Message);
    }
}