using System;
using CommandLineInterface;

namespace Tests.CommandLineInterfaceTests
{
    class Console : IConsole
    {
        private string _text;
        private Func<string, string> _tab;

        public Console()
        {
            _text = "";
        }

        public string Text => _text.Trim();

        public bool Quit { get; set; }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void WriteInfo(string text)
        {
            _text += text + Environment.NewLine;
        }

        public void WriteWarning(string text)
        {
            throw new NotImplementedException();
        }

        public void WriteError(string text)
        {
            throw new NotImplementedException();
        }

        Func<string, string> IConsole.Tab
        {
            get => _tab;
            set => _tab = value;
        }

        public Func<string, string> Enter { get; set; }
        public Func<string> Prev { get; set; }
        public Func<string> Next { get; set; }
        public Action KeyPress { get; set; }
        public void Clear()
        {
            throw new NotImplementedException();
        }

        public string ReadLine()
        {
            return "";
        }

        public void Tab()
        {
            throw new NotImplementedException();
        }
    }
}