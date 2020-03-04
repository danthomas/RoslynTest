using System.Collections.Generic;

namespace CommandLineInterface
{
    public class TaskDef
    {
        public string Name { get; set; }
        public string ArgsType { get; set; }
        public List<ArgPropDef> ArgsPropDefs { get; set; }
        public List<ParamDef> ParamDefs { get; set; }
    }
}