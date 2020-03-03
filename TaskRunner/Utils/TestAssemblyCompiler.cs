using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using AssemblyBuilder;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Tests.Utils
{
    public class TestAssemblyCompiler
    {
        private Assembly _assembly;

        public TestAssemblyCompiler Build(CompilationUnitSyntax compilationUnitSyntax, params string[] references)
        {
            references = references.Union(
            new List<string>
            {
                "mscorlib.dll",
                "netstandard.dll",
                "System.Private.CoreLib.dll",
                "System.Runtime.dll"
            }).ToArray();

            _assembly = new Compiler().Compile(compilationUnitSyntax, references);

            return this;
        }

        public object CreateInstance(string name, params object[] args)
        {
            var type = _assembly.GetType(name);

            Activator.CreateInstance(type,
                BindingFlags.CreateInstance |
                BindingFlags.Public |
                BindingFlags.Instance |
                BindingFlags.OptionalParamBinding, null, args, CultureInfo.CurrentCulture);

            var instance = Activator.CreateInstance(type, args);

            return instance;
        }
    }
}