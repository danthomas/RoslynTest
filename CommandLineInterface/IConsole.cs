using System;

namespace CommandLineInterface
{
    public interface IConsole
    {
        bool Quit { get; set; }
        void Start();
        void WriteInfo(string text);
        void WriteWarning(string text);
        void WriteError(string text);
        Func<string, string> Tab { get; set; }
        Func<string, string> Enter { get; set; }
        Func<string> Prev { get; set; }
        Func<string> Next { get; set; }
        Action KeyPress { get; set; }
        void Clear();
    }
}