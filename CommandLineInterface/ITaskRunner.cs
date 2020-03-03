namespace CommandLineInterface
{
    public interface ITaskRunner
    {
        void Run(IRunTaskCommand runTaskCommand);
    }
}