using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TaskRunner.Builders
{
    public class BinaryExpressionBuilder
    {
        //SyntaxFactory.BinaryExpression(SyntaxKind.EqualsExpression, leftExpressionSyntaxBuilder.ExpressionSyntax, rightExpressionSyntaxBuilder.Expression)
        public BinaryExpressionBuilder()
        {
            BinaryExpression = SyntaxFactory.BinaryExpression(SyntaxKind.EqualsExpression, SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("")), SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal("")));
        }

        public BinaryExpressionSyntax BinaryExpression { get; set; }

        public BinaryExpressionBuilder WithOperator(SyntaxKind syntaxKind)
        {
            //BinaryExpression = BinaryExpression.WithOperatorToken(SyntaxToken)
            return this;
        }

        public BinaryExpressionBuilder WithLeft(Action<ExpressionSyntaxBuilder> action)
        {
            var expressionSyntaxBuilder = new ExpressionSyntaxBuilder();
            action(expressionSyntaxBuilder);
            BinaryExpression = BinaryExpression.WithLeft(expressionSyntaxBuilder.Expression);
            return this;
        }

        public BinaryExpressionBuilder WithRight(Action<ExpressionSyntaxBuilder> action)
        {
            var expressionSyntaxBuilder = new ExpressionSyntaxBuilder();
            action(expressionSyntaxBuilder);
            BinaryExpression = BinaryExpression.WithRight(expressionSyntaxBuilder.Expression);
            return this;
        }
    }
}