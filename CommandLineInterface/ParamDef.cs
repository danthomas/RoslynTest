namespace CommandLineInterface
{
    public class ParamDef
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsArgs { get; set; }
        public string Namespace { get; set; }
        public bool IsList { get; set; }
    }
}