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
            StatementSyntax = SyntaxFactory.ExpressionStatement(invocationExpressionBuilder.InvocationExpression);
            return this;
        }

        public StatementSyntaxBuilder WithReturnStatement(Action<ReturnStatementBuilder> rsb)
        {
            var returnStatementBuilder = new ReturnStatementBuilder();
            rsb(returnStatementBuilder);
            StatementSyntax = returnStatementBuilder.ReturnStatement;
            return this;
        }

        public StatementSyntaxBuilder WithAssignment(Action<AssignmentExpressionBuilder> aeb)
        {
            var assignmentExpressionBuilder = new AssignmentExpressionBuilder();
            aeb(assignmentExpressionBuilder);
            StatementSyntax = SyntaxFactory.ExpressionStatement(assignmentExpressionBuilder.AssignmentExpression);
            return this;
        }

        public void WithLocalDeclaration(Action<LocalDeclarationSyntaxBuilder> ldsb)
        {
            var localDeclarationSyntaxBuilder = new LocalDeclarationSyntaxBuilder();
            ldsb(localDeclarationSyntaxBuilder);
            StatementSyntax = localDeclarationSyntaxBuilder.LocalDeclaration;
        }
    }
}