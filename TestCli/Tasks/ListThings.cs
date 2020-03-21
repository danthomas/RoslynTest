using System;
using System.Collections.Generic;
using System.Linq;
using CommandLineInterface;

namespace TestCli.Tasks
{
    public class ListThings : ITask<ListThings.Args>
    {
        private readonly IConsole _console;

        public ListThings(IConsole console)
        {
            _console = console;
        }

        public void Run(List<Thing> things, Args args)
        {
            foreach (var solution in things.Where(x => string.IsNullOrWhiteSpace(args.Name) || x.Name.Contains(args.Name, StringComparison.CurrentCultureIgnoreCase)))
            {
                _console.WriteInfo(solution.Name);
            }
        }

        public ArgDefs<Args> ArgDefs => new ArgDefs<Args>()
            .DefaultOptional(x => x.Name);

        public class Args
        {
            public string Name { get; set; }
        }
    }
}