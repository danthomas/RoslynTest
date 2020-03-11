namespace CommandLineInterface
{
    public interface IRunTaskCommand
    {
        string Name { get; }

        T GetValue<T>(string @switch, string name, bool isDefault);
        bool HasSwitch(string @switch, string name, bool isDefault);
    }
}