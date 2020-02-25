using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TaskRunner.Builders
{
    public class AssignmentExpressionBuilder
    {
        public AssignmentExpressionBuilder()
        {
            ExpressionStatement = SyntaxFactory.ExpressionStatement(SyntaxFactory.AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                SyntaxFactory.IdentifierName(""),
                SyntaxFactory.IdentifierName("")
            ));
        }

        public AssignmentExpressionBuilder WithLeftExpression(string name)
        {
            var a = (AssignmentExpressionSyntax)ExpressionStatement.Expression;
            ExpressionStatement = SyntaxFactory.ExpressionStatement(SyntaxFactory.AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                SyntaxFactory.IdentifierName(name),
                a.Right
            ));
            return this;
        }

        public AssignmentExpressionBuilder WithRightExpression(string name)
        {
            var a = (AssignmentExpressionSyntax)ExpressionStatement.Expression;
            ExpressionStatement = SyntaxFactory.ExpressionStatement(SyntaxFactory.AssignmentExpression(
                SyntaxKind.SimpleAssignmentExpression,
                a.Left,
                SyntaxFactory.IdentifierName(name)
            ));
            return this;
        }

        public ExpressionStatementSyntax ExpressionStatement { get; set; }
    }

    public class ConstructorBuilder
    {
        public ConstructorBuilder()
        {
            ConstructorDeclaration = SyntaxFactory.ConstructorDeclaration(SyntaxFactory.List<AttributeListSyntax>(),
                new SyntaxTokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)),
                SyntaxFactory.Identifier("TaskRunner"),
                SyntaxFactory.ParameterList(),
                null,
                SyntaxFactory.Block());
        }

        public ConstructorDeclarationSyntax ConstructorDeclaration { get; set; }

        public ConstructorBuilder WithParameters(params Action<ParameterBuilder>[] actions)
        {
            var parameters = new List<ParameterSyntax>();

            foreach (var action in actions)
            {
                var parameterBuilder = new ParameterBuilder();
                action(parameterBuilder);
                parameters.Add(parameterBuilder.ParameterSyntax);
            }

            ConstructorDeclaration = ConstructorDeclaration.WithParameterList(SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(parameters)));
            return this;
        }

        public ConstructorBuilder WithParameter(string type, string name)
        {
            ConstructorDeclaration = ConstructorDeclaration.AddParameterListParameters(
                SyntaxFactory.Parameter(
                    SyntaxFactory.List<AttributeListSyntax>(),
                    new SyntaxTokenList(),
                    SyntaxFactory.ParseTypeName(type),
                    SyntaxFactory.Identifier(name),
                    null));

            return this;
        }

        public ConstructorBuilder WithAssignmentExpression(Action<AssignmentExpressionBuilder> action)
        {
            var assignmentExpressionBuilder = new AssignmentExpressionBuilder();
            action(assignmentExpressionBuilder);

            ConstructorDeclaration = ConstructorDeclaration.AddBodyStatements(assignmentExpressionBuilder.ExpressionStatement);

            return this;
        }
    }
}