using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TaskRunner
{
    public class ConstructorBuilder
    {
        public ConstructorBuilder()
        {
            ConstructorDeclaration = SyntaxFactory.ConstructorDeclaration(SyntaxFactory.List<AttributeListSyntax>(),
                new SyntaxTokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)),
                SyntaxFactory.Identifier("TaskRunner"),
                SyntaxFactory.ParameterList(),
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
                ));
        }

        public ConstructorDeclarationSyntax ConstructorDeclaration { get; set; }

        public ConstructorBuilder WithParameters(params Action<ParameterBuilder>[] actions)
        {
            var parameters = new List<ParameterSyntax>();

            foreach (var action in actions)
            {
                var parameterBuilder = new ParameterBuilder();
                action(parameterBuilder);
                parameters.Add(parameterBuilder.ParameterSyntax);
            }

            ConstructorDeclaration = ConstructorDeclaration.WithParameterList(
                    SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(parameters)));
            return this;
        }
    }
}