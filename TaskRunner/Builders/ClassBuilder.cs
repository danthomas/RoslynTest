using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
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

        public ClassBuilder WithField(SyntaxKind syntaxKind, string name)
        {
            ClassDeclaration = ClassDeclaration.AddMembers(
                SyntaxFactory.FieldDeclaration(
                    SyntaxFactory.VariableDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(syntaxKind)),
                        SyntaxFactory.SeparatedList(new List<VariableDeclaratorSyntax>
                        {
                            SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(name))
                        }))
                ));

            return this;
        }
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

        public ClassBuilder WithConstructor(string name, Action<ConstructorBuilder> action)
        {
            var constructorBuilder = new ConstructorBuilder(name);
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

        public ClassBuilder WithProperty(SyntaxKind syntaxKind, string name, Action<PropertyBuilder> pb = null)
        {
            var propertyBuilder = new PropertyBuilder(name).WithType(syntaxKind);
            pb?.Invoke(propertyBuilder);
            ClassDeclaration = ClassDeclaration.AddMembers(propertyBuilder.PropertyDeclarationSyntax);
            return this;
        }

        public ClassBuilder WithProperty(string type, string name, Action<PropertyBuilder> pb = null)
        {
            var propertyBuilder = new PropertyBuilder(name).WithType(type);
            pb?.Invoke(propertyBuilder);
            ClassDeclaration = ClassDeclaration.AddMembers(propertyBuilder.PropertyDeclarationSyntax);
            return this;
        }
    }
}