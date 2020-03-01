using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TaskRunner.Builders
{
    public class AssignmentExpressionBuilder
    {
        public AssignmentExpressionBuilder()
        {
            AssignmentExpression = SyntaxFactory.AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                SyntaxFactory.IdentifierName(""),
                SyntaxFactory.IdentifierName("")
            );
        }

        public AssignmentExpressionBuilder WithLeft(string name)
        {
            AssignmentExpression = AssignmentExpression.WithLeft(SyntaxFactory.IdentifierName(name));
            return this;
        }

        public AssignmentExpressionBuilder WithRight(string name)
        {
            AssignmentExpression = AssignmentExpression.WithRight(SyntaxFactory.IdentifierName(name));
            return this;
        }

        public AssignmentExpressionBuilder WithRight(Action<ExpressionSyntaxBuilder> esb)
        {
            var expressionSyntaxBuilder = new ExpressionSyntaxBuilder();
            esb(expressionSyntaxBuilder);
            AssignmentExpression = AssignmentExpression.WithRight(expressionSyntaxBuilder.Expression);
            return this;
        }

        public AssignmentExpressionSyntax AssignmentExpression { get; set; }
    }
}