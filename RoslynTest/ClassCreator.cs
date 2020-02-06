using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RoslynTest
{
    public class ClassCreator
    {
        public static CompilationUnitSyntax CreateClass()
        {
            var syntaxFactory = SyntaxFactory.CompilationUnit();

            syntaxFactory = syntaxFactory.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")));

            var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("CodeGenerationSample"))
                .NormalizeWhitespace();

            var classDeclaration = SyntaxFactory
                .ClassDeclaration("Order");

            classDeclaration = classDeclaration
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            // Inherit BaseEntity<T> and implement IHaveIdentity: (public class Order : BaseEntity<T>, IHaveIdentity)
            //classDeclaration = classDeclaration.AddBaseListTypes(
            //    SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName("BaseEntity<Order>")),
            //    SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName("IHaveIdentity")));

            // Create a string variable: (bool canceled;)
            //var variableDeclaration = SyntaxFactory.VariableDeclaration(SyntaxFactory.ParseTypeName("bool"))
            //    .AddVariables(SyntaxFactory.VariableDeclarator("canceled"));

            // Create a field declaration: (private bool canceled;)
            //var fieldDeclaration = SyntaxFactory.FieldDeclaration(variableDeclaration)
            //    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PrivateKeyword));

            // Create a Property: (public int Quantity { get; set; })
            var propertyDeclaration = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName("int"), "Qty")
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddAccessorListAccessors(
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                    SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));

            //var syntax = SyntaxFactory.ParseStatement("canceled = true;");

            // Create a method
            //var methodDeclaration = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("void"), "MarkAsCanceled")
            //    .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
            //    .WithBody(SyntaxFactory.Block(syntax));

            classDeclaration = classDeclaration.AddMembers(/*fieldDeclaration, methodDeclaration, */propertyDeclaration);

            @namespace = @namespace.AddMembers(classDeclaration);

            syntaxFactory = syntaxFactory.AddMembers(@namespace);

           

            return syntaxFactory;
        }
    }
}