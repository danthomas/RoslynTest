using CommandLineInterface;

namespace TestCli.Tasks
{
    public class GitCheckout : ITask<GitCheckout.Args>
    {
        private readonly IConsole _console;

        public GitCheckout(IConsole console)
        {
            _console = console;
        }

        public void Run(Args args)
        {
            _console.WriteLine("git checkout " + args.Branch);
        }

        public class Args
        {
            public string Branch { get; set; }
        }

        public ArgDefs<Args> ArgDefs => new ArgDefs<Args>()
            .DefaultRequired(x => x.Branch);
    }
}