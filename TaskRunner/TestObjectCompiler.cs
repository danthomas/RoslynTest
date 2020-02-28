using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TaskRunner
{
    public class TestObjectCompiler
    {
        public (object, MethodInfo) CreateInstance(CompilationUnitSyntax compilationUnitSyntax, object[] args)
        {
            var references = new List<string>
            {
                "mscorlib.dll",
                "netstandard.dll",
                "System.Private.CoreLib.dll",
                "System.Runtime.dll"
            };

            var assembly = Compiler.Compile(compilationUnitSyntax, references);

            var type = assembly.GetType("TestNamespace.TestClass");

            Activator.CreateInstance(type,
                BindingFlags.CreateInstance |
                BindingFlags.Public |
                BindingFlags.Instance |
                BindingFlags.OptionalParamBinding, null, args, CultureInfo.CurrentCulture);

            var instance = Activator.CreateInstance(type, args);

            var methodInfo = type.GetMethod("TestMethod");

            return (instance, methodInfo);
        }
    }
}