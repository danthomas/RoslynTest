using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AssemblyBuilder
{
    public class ArgumentSyntaxBuilder
    {
        public ArgumentSyntax Argument { get; set; }

        public ArgumentSyntaxBuilder()
        {
            Argument = SyntaxFactory.Argument(SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression,
                SyntaxFactory.Literal(0)));
        }

        public ArgumentSyntaxBuilder WithExpression(Action<ExpressionSyntaxBuilder> esb)
        {
            var expressionSyntaxBuilder = new ExpressionSyntaxBuilder();
            esb(expressionSyntaxBuilder);
            Argument = Argument.WithExpression(expressionSyntaxBuilder.Expression);
            return this;
        }
    }
}