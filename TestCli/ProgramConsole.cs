using System;
using CommandLineInterface;

namespace TestCli
{
    class ProgramConsole : IConsole
    {
        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }
    }
}