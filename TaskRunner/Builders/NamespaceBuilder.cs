using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TaskRunner.Builders
{
    public class NamespaceBuilder
    {
        public NamespaceBuilder(string name)
        {
            Namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(name))
                .NormalizeWhitespace();
        }

        public NamespaceDeclarationSyntax Namespace { get; set; }

        public NamespaceBuilder WithClass(string name, Action<ClassBuilder> action = null)
        {
            return WithClass(name, new string[0], action);
        }

        public NamespaceBuilder WithClass(string name, string[] bases, Action<ClassBuilder> action = null)
        {
            var classBuilder = new ClassBuilder(name, bases);
            action?.Invoke(classBuilder);
            Namespace = Namespace.AddMembers(classBuilder.ClassDeclaration);
            return this;
        }
    }
}