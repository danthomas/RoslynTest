using System.Collections.Generic;
using CommandLineInterface;

namespace Tests.CommandLineInterfaceTests
{
    class State : IState
    {
        public T GetState<T>() where T : class
        {
            if (typeof(T) == typeof(List<Solution>))
            {
                return new List<Solution> { Solution } as T;
            }

            return default;
        }

        public Solution Solution { get; set; }
    }
}