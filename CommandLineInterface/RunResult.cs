using System.Collections.Generic;

namespace CommandLineInterface
{
    public class RunResult
    {
        public RunResult()
        {
            Errors = new List<string>();
        }
        public List<string> Errors { get; set; }
        public bool Success { get; set; }
    }
}