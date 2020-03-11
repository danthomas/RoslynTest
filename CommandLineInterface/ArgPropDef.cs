namespace CommandLineInterface
{
    public class ArgPropDef
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Namespace { get; set; }
        public string Switch { get; set; }
        public bool IsDefault { get; set; }
        public bool IsRequired { get; set; }
    }
}