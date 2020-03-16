using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandLineInterface
{
    public class Help : ITask<Help.Args>
    {
        private readonly IConsole _console;

        public Help(IConsole console)
        {
            _console = console;
        }

        public void Run(Args args)
        {
            var taskDef = TaskDefs.SingleOrDefault(x => x.Name == args.Name);

            var taskDefs = TaskDefs
                .Where(x => string.IsNullOrWhiteSpace(args.Name) || x.Name.Contains(args.Name, StringComparison.CurrentCultureIgnoreCase))
                .ToList();

            if (!taskDefs.Any())
            {
                _console.WriteError("No matches.");
            }
            else if (taskDef != null)
            {
                var taskDefName = taskDef.Name;

                foreach (var argsPropDef in taskDef.ArgsPropDefs)
                {
                    taskDefName += argsPropDef.IsRequired ? " " : " [";

                    taskDefName += argsPropDef.Switch + "|" + argsPropDef.Name;

                    taskDefName += argsPropDef.IsRequired ? "" : "]";
                }

                _console.WriteLine(taskDefName);
            }
            else
            {
                foreach (var td in taskDefs
                    .OrderBy(x => x.Name))
                {
                    _console.WriteLine(td.Name);
                }
            }
        }

        public class Args
        {
            public string Name { get; set; }
        }

        public ArgDefs<Args> ArgDefs => new ArgDefs<Args>()
                .DefaultOptional(x => x.Name);

        public List<TaskDef> TaskDefs { get; set; }
    }
}