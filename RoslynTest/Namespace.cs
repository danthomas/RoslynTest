using System.Collections.Generic;

namespace RoslynTest
{
    public class Namespace
    {
        public Namespace()
        {
            Classes = new List<Class>();
        }
        public string Name { get; set; }
        public List<Class> Classes { get; set; }
    }
}