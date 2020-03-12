using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using CommandLineInterface;

namespace TestCli
{
    class Program
    {
        static void Main(string[] args)
        {
            var programConsole = new ProgramConsole();

            var state = new State
            {
                Thing = new Thing
                {
                    Name = "Abcd"
                }
            };

            var assembly = typeof(Program).Assembly;

            new App().Run(programConsole, state, assembly);
        }
    }
}
