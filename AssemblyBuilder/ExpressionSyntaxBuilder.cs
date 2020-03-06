using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AssemblyBuilder
{
    public class ExpressionSyntaxBuilder
    {
        public ExpressionSyntax Expression { get; set; }

        public void WithIdentifier(string name)
        {
            Expression = SyntaxFactory.IdentifierName(name);
        }

        public ExpressionSyntaxBuilder WithObjectCreation(params string[] names)
        {
            var objectCreationBuilder = new ObjectCreationBuilder(names);
            Expression = objectCreationBuilder.Expression;
            return this;
        }

        public ExpressionSyntaxBuilder WithObjectCreation(string name, Action<ObjectCreationBuilder> ocb = null)
        {
            var objectCreationBuilder = new ObjectCreationBuilder(name);
            ocb?.Invoke(objectCreationBuilder);
            Expression = objectCreationBuilder.Expression;
            return this;
        }

        public ExpressionSyntaxBuilder Literal(int i)
        {
            Expression = SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(i));
            return this;
        }

        public ExpressionSyntaxBuilder Literal(string s)
        {
            Expression = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(s));
            return this;
        }

        public ExpressionSyntaxBuilder Literal(bool b)
        {
            Expression = SyntaxFactory.LiteralExpression(b? SyntaxKind.TrueLiteralExpression: SyntaxKind.FalseLiteralExpression);
            return this;
        }

        public ExpressionSyntaxBuilder SimpleMemberAccess(string left, string right)
        {
            Expression = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                SyntaxFactory.IdentifierName(left), (SimpleNameSyntax)SyntaxFactory.ParseName(right));
            return this;
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

            Expression = SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                expressionSyntaxBuilder.Expression,
                simpleNameSyntax
            );
        }

        public ExpressionSyntaxBuilder WithInvocation(Action<InvocationExpressionBuilder> action)
        {
            var invocationExpressionBuilder = new InvocationExpressionBuilder();
            action(invocationExpressionBuilder);
            Expression = invocationExpressionBuilder.InvocationExpression;
            return this;
        }

        public ExpressionSyntaxBuilder WithStringLiteralExpression(string value)
        {
            Expression = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(value));
            return this;
        }
    }
}