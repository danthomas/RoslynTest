using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TaskRunner
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

            ClassDeclaration = ClassDeclaration.AddMembers(
                SyntaxFactory.ConstructorDeclaration(SyntaxFactory.List<AttributeListSyntax>(),
                    new SyntaxTokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)),
                    SyntaxFactory.Identifier("TaskRunner"),
                    SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(new[]
                    {
                        SyntaxFactory.Parameter(SyntaxFactory.List<AttributeListSyntax>(),
                            new SyntaxTokenList(),
                            SyntaxFactory.ParseTypeName("IServiceProvider"),
                            SyntaxFactory.Identifier("serviceProvider"),
                            null),
                        SyntaxFactory.Parameter(SyntaxFactory.List<AttributeListSyntax>(),
                            new SyntaxTokenList(),
                            SyntaxFactory.ParseTypeName("IState"),
                            SyntaxFactory.Identifier("state"),
                            null)
                    })),
                    null,
                    SyntaxFactory.Block(
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                                SyntaxFactory.IdentifierName("_serviceProvider"),
                                SyntaxFactory.IdentifierName("serviceProvider")
                            )
                        ),
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                                SyntaxFactory.IdentifierName("_state"),
                                SyntaxFactory.IdentifierName("state")
                            )
                        )

                    )));
            return this;
        }
    }
}