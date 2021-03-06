﻿using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AssemblyBuilder
{
    public class Compiler
    {
        public Assembly Compile(CompilationUnitSyntax compilationUnitSyntax, string[] references)
        {
            var syntaxTree = compilationUnitSyntax.SyntaxTree;

            //creating options that tell the compiler to output a console application
            var options = new CSharpCompilationOptions(
                OutputKind.DynamicallyLinkedLibrary,
                optimizationLevel: OptimizationLevel.Debug,
                allowUnsafe: true);

            //creating the compilation
            var compilation = CSharpCompilation.Create(Path.GetRandomFileName(), options: options);

            //adding the syntax tree
            compilation = compilation.AddSyntaxTrees(syntaxTree);

            //getting the local path of the assemblies
            var assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);


            compilation = compilation.AddReferences(references
                .Select(x => MetadataReference.CreateFromFile(Path.IsPathRooted(x)
                    ? x :
                    Path.Combine(assemblyPath, x))));

            var context = AssemblyLoadContext.Default;

            using var ms = new MemoryStream();

            var result = compilation.Emit(ms);

            var code = compilationUnitSyntax.NormalizeWhitespace().ToString();

            if (!result.Success)
            {
                var failures = result.Diagnostics.Where(diagnostic =>
                    diagnostic.IsWarningAsError ||
                    diagnostic.Severity == DiagnosticSeverity.Error);

                var stringBuilder = new StringBuilder();


                foreach (var diagnostic in failures)
                {
                    stringBuilder.AppendLine($"{diagnostic.Id}: {diagnostic.GetMessage()}, {diagnostic.Location}");
                }


                throw new CompilationException(stringBuilder.ToString(), code);
            }

            ms.Seek(0, SeekOrigin.Begin);
            return context.LoadFromStream(ms);
        }
    }
}