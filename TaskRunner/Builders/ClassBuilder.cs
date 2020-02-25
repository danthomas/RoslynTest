using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TaskRunner.Builders
{
    public class ClassBuilder
    {
        public ClassBuilder(string name, string[] bases)
        {
            ClassDeclaration = SyntaxFactory
                .ClassDeclaration(name);

            ClassDeclaration = ClassDeclaration
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            foreach (var @base in bases)
            {
                ClassDeclaration = ClassDeclaration.AddBaseListTypes(
                    SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName(@base)));
            }
        }

        public ClassDeclarationSyntax ClassDeclaration { get; set; }

        public ClassBuilder WithField(string type, string name)
        {
            ClassDeclaration = ClassDeclaration.AddMembers(
                SyntaxFactory.FieldDeclaration(
                    SyntaxFactory.VariableDeclaration(SyntaxFactory.IdentifierName(type),
                        SyntaxFactory.SeparatedList(new List<VariableDeclaratorSyntax>
                        {
                            SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(name))
                        }))
                ));

            return this;
        }

        public ClassBuilder WithConstructor(Action<ConstructorBuilder> action)
        {
            var constructorBuilder = new ConstructorBuilder();
            action(constructorBuilder);
            ClassDeclaration = ClassDeclaration.AddMembers(constructorBuilder.ConstructorDeclaration);
            return this;
        }

        public ClassBuilder WithMethod(string name, Action<MethodBuilder> action)
        {
            var methodBuilder = new MethodBuilder(name);
            action(methodBuilder);
            ClassDeclaration = ClassDeclaration.AddMembers(methodBuilder.MethodDeclarationSyntax);
            return this;
        }
    }
}