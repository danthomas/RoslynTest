using System.Collections.Generic;

namespace RoslynTest
{
    public class Definition
    {
        public Definition()
        {
            References = new List<string>();
            Namespaces = new List<Namespace>();
            Usings = new List<Using>();
        }

        public List<string> References { get; set; }

        public List<Using> Usings { get; set; }

        public List<Namespace> Namespaces { get; set; }
    }
}