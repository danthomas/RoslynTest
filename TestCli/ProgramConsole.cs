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

            while (true)
            {
                var c = Console.ReadKey(true);
                if (c.Key == ConsoleKey.Tab)
                {
                    line = Tab(line);
                    SetCurrentLine();
                }
                else if (c.Key == ConsoleKey.Backspace)
                {
                    if (line.Length > 0)
                    {
                        line = line.Substring(0, line.Length - 1);
                        SetCurrentLine();
                        KeyPress();
                    }
                }
                else if (c.Key == ConsoleKey.Enter)
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
                }
                else if (c.Key == ConsoleKey.Escape)
                {
                    line = "";
                    SetCurrentLine(); 
                    KeyPress();
                }
                else if (c.Key == ConsoleKey.UpArrow)
                {
                    line = Prev();
                    if (line != null)
                    {
                        SetCurrentLine();
                        KeyPress();
                    }
                }   
                else if (c.Key == ConsoleKey.DownArrow)
                {
                    line = Next();
                    if (line != null)
                    {
                        SetCurrentLine();
                        KeyPress();
                    }
                }
                else if (IsPrintable(c))
                {
                    line += c.KeyChar;
                    Console.Write(c.KeyChar);
                    KeyPress();
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