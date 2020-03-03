namespace Tests
{
    public interface ITaskRunner
    {
        void Run(IRunTaskCommand runTaskCommand);
    }
}