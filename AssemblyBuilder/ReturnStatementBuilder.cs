using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AssemblyBuilder
{
    public class ReturnStatementBuilder
    {
        public ReturnStatementBuilder()
        {
            ReturnStatement = SyntaxFactory.ReturnStatement();
        }

        public ReturnStatementBuilder WithExpression(Action<ExpressionSyntaxBuilder> esb)
        {
            var expressionSyntaxBuilder = new ExpressionSyntaxBuilder();
            esb(expressionSyntaxBuilder);
            ReturnStatement = ReturnStatement.WithExpression(expressionSyntaxBuilder.Expression);
            return this;
        }

        public ReturnStatementSyntax ReturnStatement { get; set; }
    }
}