using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TaskRunner
{
    public class StatementSyntaxBuilder
    {
        public StatementSyntax StatementSyntax { get; set; }

        public StatementSyntaxBuilder()
        {
            StatementSyntax = SyntaxFactory.Block();
        }

        public StatementSyntaxBuilder WithInvocation(Action<InvocationExpressionBuilder> action)
        {
            var invocationExpressionBuilder = new InvocationExpressionBuilder();
            action(invocationExpressionBuilder);
            StatementSyntax = SyntaxFactory.ExpressionStatement(invocationExpressionBuilder.StatementSyntax);
            return this;
        }
    }
}