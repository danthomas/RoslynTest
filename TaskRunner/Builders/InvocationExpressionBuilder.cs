using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TaskRunner.Builders
{
    public class InvocationExpressionBuilder
    {
        public ExpressionSyntax StatementSyntax { get; set; }

        public InvocationExpressionBuilder WithExpression(Action<ExpressionSyntaxBuilder> esb)
        {
            var expressionSyntaxBuilder = new ExpressionSyntaxBuilder();
            esb(expressionSyntaxBuilder);
            StatementSyntax = SyntaxFactory.InvocationExpression(expressionSyntaxBuilder.Expression,
                SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>()));
            return this;
        }
    }
}