namespace CommandLineInterface
{
    public class Quit : ITask
    {
        public void Run()
        {
            HasQuit = true;
        }

        public bool HasQuit { get; set; }
    }
}