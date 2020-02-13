namespace TaskRunner
{
    class RunTaskCommand : IRunTaskCommand
    {
        public string CommandLine { get; set; }

        public string Name => CommandLine;

        public T GetValue<T>(string name)
        {
            return default;
        }
    }
}