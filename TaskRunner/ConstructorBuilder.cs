using System;
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
                ));
        }

        public ConstructorDeclarationSyntax ConstructorDeclaration { get; set; }

        public ConstructorBuilder WithParameter(Action<ParameterBuilder> action)
        {
            var parameterBuilder = new ParameterBuilder();
            action(parameterBuilder);
            return this;
        }
    }

    public class ParameterBuilder
    {

    }
}