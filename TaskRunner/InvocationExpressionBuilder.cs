using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TaskRunner
{
    public class InvocationExpressionBuilder
    {
        public StatementSyntax StatementSyntax { get; set; }

        public InvocationExpressionBuilder WithExpression(Action<ExpressionSyntaxBuilder> esb)
        {
            var expressionSyntaxBuilder = new ExpressionSyntaxBuilder();
            esb(expressionSyntaxBuilder);
            StatementSyntax = SyntaxFactory.ExpressionStatement(SyntaxFactory.InvocationExpression(expressionSyntaxBuilder.ExpressionSyntax,
                SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>())));
            return this;
        }
    }
}