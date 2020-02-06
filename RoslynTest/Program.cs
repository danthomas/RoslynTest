using System;
using System.Collections.Generic;

namespace RoslynTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var definition = new Definition
            {
                References = new List<string>
                {
                    "mscorlib.dll",
                    "System.Private.CoreLib.dll",
                    "System.Console.dll",
                    "System.Runtime.dll",
                    typeof(IThing).Assembly.Location
                },
                Usings = new List<Using>
                {
                    new Using
                    {
                        Name = "System"
                    },
                    new Using
                    {
                        Name = "RoslynTest"
                    }
                },
                Namespaces = new List<Namespace>
                {
                    new Namespace
                    {
                        Name = "Tester",
                        Classes = new List<Class>
                        {
                            new Class
                            {
                                Name = "Thing",
                                Implements = "IThing",
                                Properties = new List<Property>
                                {
                                    new Property
                                    {
                                        Name = "Qty",
                                        Type = "int"
                                    },
                                    new Property
                                    {
                                        Name = "Name",
                                        Type = "string"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var compilationUnitSyntax = new Builder().Build(definition);

            var assembly = Compiler.Compile(compilationUnitSyntax, definition.References);

            var type = assembly.GetType("Tester.Thing");

            var thing = (IThing)Activator.CreateInstance(type);

            if (thing != null)
            {
                thing.Name = "ABCD";
                thing.Qty = 373737;

                Console.WriteLine($"{thing.Name} {thing.Qty}");
            }
        }
    }
}
