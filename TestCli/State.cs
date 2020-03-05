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

            return (T)@object;
        }

        public Thing Thing { get; set; }
    }
}