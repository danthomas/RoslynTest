using System.Collections.Generic;

namespace RoslynTest
{
    public class Class
    {
        public Class()
        {
            Properties = new List<Property>();
        }

        public List<Property> Properties { get; set; }

        public string Name { get; set; }
        public string Implements { get; set; }
    }
}