using System;

namespace CommandLineInterface
{
    public interface IConsole
    {
        void Start();
        void WriteLine(string text);
        Func<string, string> Tab { get; set; }
        Func<string, string> Enter { get; set; }
        Func<string> Prev { get; set; }
        Func<string> Next { get; set; }
        Action KeyPress { get; set; }
        void Clear();
    }
}