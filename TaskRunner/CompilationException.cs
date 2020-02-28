using System;

namespace TaskRunner
{
    public class CompilationException : Exception
    {
        public string Errors { get; }
        public string Code { get; }

        public CompilationException(string errors, string code)
            : base("Compilation Failed.")
        {
            Errors = errors;
            Code = code;
        }
    }
}