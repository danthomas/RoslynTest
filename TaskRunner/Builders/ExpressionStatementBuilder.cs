using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TaskRunner.Builders
{
    public class ExpressionStatementBuilder
    {
        public ExpressionStatementSyntax ExpressionStatementSyntax { get; set; }

        public ExpressionStatementBuilder WithAssignment(Action<AssignmentExpressionBuilder> aeb)
        {
            var assignmentExpressionBuilder = new AssignmentExpressionBuilder();
            aeb(assignmentExpressionBuilder);
            ExpressionStatementSyntax = SyntaxFactory.ExpressionStatement(assignmentExpressionBuilder.AssignmentExpression);
            return this;
        }
    }
}