using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TaskRunner.Builders
{
    public class CompilationUnitBuilder
    {
        public CompilationUnitBuilder()
        {
            CompilationUnitSyntax = SyntaxFactory.CompilationUnit();
        }

        public CompilationUnitSyntax CompilationUnitSyntax { get; set; }

        public CompilationUnitBuilder WithNamespace(string name, Action<NamespaceBuilder> action)
        {
            var namespaceBuilder = new NamespaceBuilder(name);
            action(namespaceBuilder);
            CompilationUnitSyntax = CompilationUnitSyntax.AddMembers(namespaceBuilder.Namespace);
            return this;
        }

        public CompilationUnitBuilder WithUsing<T>()
        {
            return WithUsings(typeof(T).Namespace);
        }

        public CompilationUnitBuilder WithUsings(params Type[] types)
        {
            return WithUsings(types.Select(x => x.Namespace).ToArray());
        }

        public CompilationUnitBuilder WithUsings(params string[] usings)
        {
            CompilationUnitSyntax = CompilationUnitSyntax.AddUsings(usings
                .Select(x => SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(x))).ToArray());

            return this;
        }
    }
}