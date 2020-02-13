namespace TaskRunner
{
    public interface ITaskRunner
    {
        void Run(IRunTaskCommand runTaskCommand);
    }
}