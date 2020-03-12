namespace CommandLineInterface
{
    public interface IConsole
    {
        void WriteLine(string text);
        string ReadLine();
    }
}