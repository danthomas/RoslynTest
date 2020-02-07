using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TaskRunner
{
    class Compiler
    {
        public static Assembly Compile(CompilationUnitSyntax compilationUnitSyntax, List<string> references)
        {
            var  syntaxTree = compilationUnitSyntax.SyntaxTree;

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
            using (var ms = new MemoryStream())
            {
                var result = compilation.Emit(ms);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        Console.Error.WriteLine("{0}: {1}, {2}", diagnostic.Id, diagnostic.GetMessage(), diagnostic.Location);
                    }
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    return context.LoadFromStream(ms);
                }
            }

            return null;
        }
    }
}