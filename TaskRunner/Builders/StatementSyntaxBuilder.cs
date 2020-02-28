using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TaskRunner.Builders
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

        public StatementSyntaxBuilder WithReturnStatement(Action<ReturnStatementBuilder> rsb)
        {
            var returnStatementBuilder = new ReturnStatementBuilder();
            rsb(returnStatementBuilder);
            StatementSyntax = returnStatementBuilder.ReturnStatement;
            return this;
        }
    }
}