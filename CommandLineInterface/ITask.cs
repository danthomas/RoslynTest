namespace CommandLineInterface
{
    public interface ITask
    {
    }

    public interface ITask<T> : ITask
    {
        ArgDefs<T> ArgDefs { get; }
    }
}