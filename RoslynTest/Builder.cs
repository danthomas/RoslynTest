using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RoslynTest
{
    class Builder
    {
        public CompilationUnitSyntax Build(Definition definition)
        {
            var compilationUnitSyntax = SyntaxFactory.CompilationUnit();

            foreach (var @using in definition.Usings)
            {
                compilationUnitSyntax = compilationUnitSyntax.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(@using.Name)));
            }

            foreach (var ns in definition.Namespaces)
            {
                var namespaceDeclaration = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(ns.Name))
                    .NormalizeWhitespace();

                foreach (var c in ns.Classes)
                {
                    var classDeclaration = SyntaxFactory
                        .ClassDeclaration(c.Name);

                    classDeclaration = classDeclaration
                        .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

                    if (!string.IsNullOrWhiteSpace(c.Implements))
                    {
                        classDeclaration = classDeclaration.AddBaseListTypes(
                            SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(c.Implements)));
                    }

                    foreach (var property in c.Properties)
                    {
                        var propertyDeclaration = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(property.Type), property.Name)
                            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                            .AddAccessorListAccessors(
                                SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                                SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));

                        classDeclaration = classDeclaration.AddMembers(propertyDeclaration);
                    }

                    namespaceDeclaration = namespaceDeclaration.AddMembers(classDeclaration);
                }

                compilationUnitSyntax = compilationUnitSyntax.AddMembers(namespaceDeclaration);
            }


            return compilationUnitSyntax;
        }
    }
}