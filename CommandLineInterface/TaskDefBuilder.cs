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

            var switches = new List<SwitchDef>();

            var parameter = runMethodInfo
                .GetParameters()
                .SingleOrDefault(x => x.ParameterType.IsNested && x.ParameterType.DeclaringType == type);


            if (type.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ITask<>)))
            {
                var count = type.GetConstructors()[0].GetParameters().Length;

                var args = Enumerable.Repeat<object>(null, count).ToArray();

                var instance = Activator.CreateInstance(type, args);

                var argDefs = instance.GetType().GetProperty("ArgDefs").GetValue(instance);
                switches = (List<SwitchDef>)argDefs.GetType().GetProperty("Switches").GetValue(argDefs);
            }
            else if (parameter != null)
            {
                switches = parameter.ParameterType.GetProperties()
                    .Select(x => new SwitchDef
                    {
                        Name = x.Name,
                        Switch = new string(x.Name.ToCharArray().Where(Char.IsUpper).ToArray()).ToLower()
                    }).ToList();
            }

            return new TaskDef
            {
                Name = type.Name,
                Namespace = type.Namespace,
                ArgsType = parameter == null ? "" : $"{type.FullName}.{parameter.ParameterType.Name}",
                ParamDefs = runMethodInfo.GetParameters().Select(x => BuildParamDef(x, type)).ToList(),
                ArgsPropDefs = GetArgPropDefs(parameter, switches)
            };
        }

        private static List<ArgPropDef> GetArgPropDefs(ParameterInfo parameter, List<SwitchDef> switches)
        {
            if (parameter == null)
            {
                return new List<ArgPropDef>();
            }

            return parameter
                .ParameterType
                .GetProperties()
                .Select(x =>
                {
                    var @switch = switches.SingleOrDefault(y => x.Name == y.Name);

                    return new ArgPropDef
                    {
                        Name = x.Name,
                        Switch = @switch?.Switch,
                        IsDefault = @switch?.IsDefault ?? false,
                        IsRequired = @switch?.IsRequired ?? false,
                        Namespace = x.PropertyType.Namespace,
                        Type = x.PropertyType.Name
                    };
                }).ToList();
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