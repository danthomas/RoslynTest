using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AssemblyBuilder
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

        public AssignmentExpressionBuilder WithLeft(params string[] names)
        {
            var typeSyntax = new TypeSyntaxBuilder().Build(names);

            AssignmentExpression = AssignmentExpression.WithLeft(typeSyntax);
            return this;
        }

        public AssignmentExpressionBuilder WithRight(params string[] names)
        {
            var typeSyntax = new TypeSyntaxBuilder().Build(names);

            AssignmentExpression = AssignmentExpression.WithRight(typeSyntax);
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