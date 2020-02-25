using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TaskRunner.Builders
{
    public class ExpressionSyntaxBuilder
    {
        public ExpressionSyntax ExpressionSyntax { get; set; }

        public void NumericalLiteral(int i)
        {
            ExpressionSyntax = SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(i));
        }

        public void StringLiteral(string s)
        {
            ExpressionSyntax = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(s));
        }

        public void SimpleMemberAccess(string left, string right)
        {
            ExpressionSyntax = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                SyntaxFactory.IdentifierName(left), (SimpleNameSyntax)SyntaxFactory.ParseName(right));
        }

        public void SimpleMemberAccess(Action<ExpressionSyntaxBuilder> left, string name, params string[] genericArgs)
        {
            var expressionSyntaxBuilder = new ExpressionSyntaxBuilder();

            left(expressionSyntaxBuilder);

            var simpleNameSyntax = genericArgs.Any()
                ?
                (SimpleNameSyntax)SyntaxFactory.GenericName(SyntaxFactory.Identifier(name),
                    SyntaxFactory.TypeArgumentList(SyntaxFactory.SeparatedList(genericArgs.Select(x => (TypeSyntax)SyntaxFactory.IdentifierName(
                            SyntaxFactory.Identifier(x))).ToArray())
                    ))
                : SyntaxFactory.IdentifierName(
                    SyntaxFactory.Identifier(name)
                );

            ExpressionSyntax = SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                expressionSyntaxBuilder.ExpressionSyntax,
                simpleNameSyntax
            );
        }

        public void Identifier(string name)
        {
            ExpressionSyntax = SyntaxFactory.IdentifierName(name);
        }

        public ExpressionSyntaxBuilder WithInvocation(Action<InvocationExpressionBuilder> action)
        {
            var invocationExpressionBuilder = new InvocationExpressionBuilder();
            action(invocationExpressionBuilder);
            ExpressionSyntax = invocationExpressionBuilder.StatementSyntax;
            return this;
        }
    }
}