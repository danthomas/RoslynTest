namespace CommandLineInterface
{
    public interface ITaskRunner
    {
        RunResult Run(IRunTaskCommand runTaskCommand);
    }
}