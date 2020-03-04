using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CommandLineInterface
{
    public class TaskDefBuilder
    {
        public List<TaskDef> Build(params Type[] taskTypes)
        {
            return taskTypes
                .Select(BuildTaskDef)
                .ToList();
        }

        private static TaskDef BuildTaskDef(Type type)
        {
            var runMethodInfo = type.GetMethod("Run");

            var parameter = runMethodInfo
                .GetParameters()
                .SingleOrDefault(x => x.ParameterType.IsNested && x.ParameterType.DeclaringType == type);

            return new TaskDef
            {
                Name = type.Name,
                Namespace = type.Namespace,
                ArgsType = parameter == null ? "" : $"{type.FullName}.{parameter.ParameterType.Name}",
                ParamDefs = runMethodInfo.GetParameters().Select(x => BuildParamDef(x, type)).ToList(),
                ArgsPropDefs = GetArgPropDefs(parameter)
            };
        }

        private static List<ArgPropDef> GetArgPropDefs(ParameterInfo parameter)
        {
            return parameter?
                .ParameterType
                .GetProperties()
                .Select(x => new ArgPropDef
                {
                    Name = x.Name,
                    Namespace = x.PropertyType.Namespace,
                    Type = x.PropertyType.Name
                }).ToList()
                   ?? new List<ArgPropDef>();
        }

        private static ParamDef BuildParamDef(ParameterInfo x, Type type)
        {
            var typeNames = x.ParameterType.FullName;

            var isArgs = false;

            if (x.ParameterType.IsNested && x.ParameterType.DeclaringType == type)
            {
                typeNames = $"{type.FullName}.{x.ParameterType.Name}";
                isArgs = true;
            }

            return new ParamDef
            {
                Name = x.Name,
                Namespace = x.ParameterType.Namespace,
                Type = typeNames,
                IsArgs = isArgs
            };
        }
    }
}