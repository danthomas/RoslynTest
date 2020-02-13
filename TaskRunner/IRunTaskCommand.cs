namespace TaskRunner
{
    public interface IRunTaskCommand
    {
        string CommandLine { get; set; }

        string Name { get; }

        T GetValue<T>(string name);
    }
}