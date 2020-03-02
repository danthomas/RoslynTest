using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TaskRunner.Builders
{
    public class InvocationExpressionBuilder
    {
        public InvocationExpressionSyntax InvocationExpression { get; set; }

        public InvocationExpressionBuilder()
        {
            InvocationExpression = SyntaxFactory.InvocationExpression(
                SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.ThisExpression(),
                    SyntaxFactory.IdentifierName("")));
        }

        public InvocationExpressionBuilder WithThisExpression()
        {
            var expressionSyntax = (MemberAccessExpressionSyntax)InvocationExpression.Expression;

            InvocationExpression = InvocationExpression
                .WithExpression(expressionSyntax.WithExpression(SyntaxFactory.ThisExpression()));
            return this;
        }

        public InvocationExpressionBuilder WithIdentifier(string name)
        {
            var expressionSyntax = (MemberAccessExpressionSyntax)InvocationExpression.Expression;

            InvocationExpression = InvocationExpression
                .WithExpression(expressionSyntax.WithName(SyntaxFactory.IdentifierName(name)));
            return this;
        }

        public InvocationExpressionBuilder WithGenericIdentifier(string name, params string[] genericArgs)
        {
            var expressionSyntax = (MemberAccessExpressionSyntax)InvocationExpression.Expression;

            var syntaxNodeOrTokenBuilder = new SyntaxNodeOrTokenBuilder();
            var syntaxNodeOrTokens = genericArgs.Select(x => syntaxNodeOrTokenBuilder.Build(x)).ToArray();

            InvocationExpression = InvocationExpression
                .WithExpression(expressionSyntax.WithName(SyntaxFactory.GenericName(name)
                    .WithTypeArgumentList(SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList<TypeSyntax>(
                        syntaxNodeOrTokens
                    )))));
            return this;
        }

        public InvocationExpressionBuilder WithExpression(Action<ExpressionSyntaxBuilder> esb)
        {
            var expressionSyntax = (MemberAccessExpressionSyntax)InvocationExpression.Expression;

            var expressionSyntaxBuilder = new ExpressionSyntaxBuilder();
            esb(expressionSyntaxBuilder);

            InvocationExpression = InvocationExpression
                .WithExpression(expressionSyntax.WithExpression(expressionSyntaxBuilder.Expression));
            return this;
        }

        public InvocationExpressionBuilder WithArguments(params Action<ArgumentSyntaxBuilder>[] asbs)
        {
            var arguments = asbs.Select(x =>
            {
                var argumentSyntaxBuilder = new ArgumentSyntaxBuilder();
                x(argumentSyntaxBuilder);
                return argumentSyntaxBuilder.Argument;
            }).ToArray();

            InvocationExpression = InvocationExpression.AddArgumentListArguments(arguments);
            return this;
        }
    }
}