using System;
using CommandLineInterface;

namespace Tests.CommandLineInterfaceTests
{
    class Console : IConsole
    {
        private string _text;

        public Console()
        {
            _text = "";
        }

        public string Text => _text.Trim();

        public void WriteLine(string text)
        {
            _text += text + Environment.NewLine;
        }
    }
}