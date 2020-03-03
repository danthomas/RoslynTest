namespace Tests
{
    public interface IRunTaskCommand
    {
        string Name { get; }

        T GetValue<T>(string name);
    }
}