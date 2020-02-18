using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TaskRunner
{
    public class NamespaceBuilder
    {
        public NamespaceBuilder(string name)
        {
            Namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(name))
                .NormalizeWhitespace();
        }

        public NamespaceDeclarationSyntax Namespace { get; set; }

        public NamespaceBuilder WithClass(string name, string[] bases, Action<ClassBuilder> action)
        {
            var classBuilder = new ClassBuilder(name, bases);
            action(classBuilder);
            Namespace = Namespace.AddMembers(classBuilder.ClassDeclaration);
            return this;
        }
    }
}