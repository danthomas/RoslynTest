using System;
using CommandLineInterface;

namespace TestCli
{
    public class ProgramConsole : IConsole
    {
        public void WriteLine(string text)
        {
            Console.SetCursorPosition(0, Console.CursorTop + 1);
            Console.Write(text);
        }

        public void WriteError(string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.SetCursorPosition(0, Console.CursorTop + 1);
            Console.Write(text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public Func<string, string> Tab { get; set; }
        public Func<string, string> Enter { get; set; }
        public Func<string> Prev { get; set; }
        public Func<string> Next { get; set; }
        public Action KeyPress { get; set; }

        public void Clear()
        {
            Console.Clear();
        }

        public bool Quit { get; set; }

        public void Start()
        {
            var line = "";

            void SetCurrentLine()
            {
                var cursorTop = Console.CursorTop;
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, cursorTop);
                Console.Write(line);
            }

            while (!Quit)
            {
                var c = Console.ReadKey(true);
                switch (c.Key)
                {
                    case ConsoleKey.Tab:
                        line = Tab(line);
                        SetCurrentLine();

                        break;
                    case ConsoleKey.Backspace:
                    {
                        if (line.Length > 0)
                        {
                            line = line.Substring(0, line.Length - 1);
                            SetCurrentLine();
                            KeyPress();
                        }

                        break;
                    }
                    case ConsoleKey.Enter:
                    {
                        line = Enter(line);
                        if (line == null)
                        {
                            line = "";
                            Console.SetCursorPosition(0, Console.CursorTop + 1);
                        }
                        else
                        {
                            SetCurrentLine();
                        }

                        break;
                    }
                    case ConsoleKey.Escape:
                        line = "";
                        SetCurrentLine(); 
                        KeyPress();
                        break;
                    case ConsoleKey.UpArrow:
                    {
                        line = Prev();
                        if (line != null)
                        {
                            SetCurrentLine();
                            KeyPress();
                        }

                        break;
                    }
                    case ConsoleKey.DownArrow:
                    {
                        line = Next();
                        if (line != null)
                        {
                            SetCurrentLine();
                            KeyPress();
                        }

                        break;
                    }
                    default:
                    {
                        if (IsPrintable(c))
                        {
                            line += c.KeyChar;
                            Console.Write(c.KeyChar);
                            KeyPress();
                        }

                        break;
                    }
                }
            }
        }

        private bool IsPrintable(in ConsoleKeyInfo consoleKeyInfo)
        {
            return consoleKeyInfo.Key == ConsoleKey.Spacebar
                   || Char.IsLetterOrDigit(consoleKeyInfo.KeyChar);
        }
    }
}