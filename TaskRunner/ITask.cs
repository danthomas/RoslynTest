namespace TaskRunner
{
    interface ITask
    {
    }

    interface ITask<T> : ITask
    {
        ArgDefs<T> ArgDefs { get; }
    }
}