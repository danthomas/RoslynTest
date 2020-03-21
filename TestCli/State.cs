using System.Collections.Generic;
using CommandLineInterface;

namespace TestCli
{
    internal class State : IState
    {
        public T GetState<T>() where T : class
        {
            object @object = default(T);

            if (typeof(T) == typeof(Thing))
            {
                @object = Thing;
            }
            else if (typeof(T) == typeof(List<Thing>))
            {
                @object = new List<Thing> { Thing };
            }

            return (T)@object;
        }

        public Thing Thing { get; set; }
    }
}