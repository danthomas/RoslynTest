using System;

namespace TaskRunner
{
    class TaskB : ITask<TaskB.Args>
    {
        internal class Args
        {
            public bool BoolProp { get; set; }
            public string StringProp { get; set; }
        }

        public void Run(Args args)
        {
            throw new NotImplementedException();
        }

        public ArgDefs<Args> ArgDefs => new ArgDefs<Args>()
            .DefaultRequired(x => x.StringProp, "sp");
    }
}