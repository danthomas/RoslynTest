using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AssemblyBuilder
{
    public class ConstructorBuilder
    {
        public ConstructorBuilder(string name)
        {
            ConstructorDeclaration = SyntaxFactory.ConstructorDeclaration(SyntaxFactory.List<AttributeListSyntax>(),
                new SyntaxTokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)),
                SyntaxFactory.Identifier(name),
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

        public ConstructorBuilder WithParameter(Action<ParameterBuilder> pb)
        {
            var parameterBuilder = new ParameterBuilder();
            pb(parameterBuilder);
            ConstructorDeclaration = ConstructorDeclaration.AddParameterListParameters(parameterBuilder.ParameterSyntax);
            
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

        public ConstructorBuilder WithParameter(SyntaxKind syntaxKind, string name)
        {
            ConstructorDeclaration = ConstructorDeclaration.AddParameterListParameters(
                SyntaxFactory.Parameter(
                    SyntaxFactory.List<AttributeListSyntax>(),
                    new SyntaxTokenList(),
                    SyntaxFactory.PredefinedType(SyntaxFactory.Token(syntaxKind)),
                    SyntaxFactory.Identifier(name),
                    null));

            return this;
        }

        public ConstructorBuilder WithAssignmentExpression(Action<AssignmentExpressionBuilder> action)
        {
            var assignmentExpressionBuilder = new AssignmentExpressionBuilder();
            action(assignmentExpressionBuilder);

            ConstructorDeclaration = ConstructorDeclaration.AddBodyStatements(SyntaxFactory.ExpressionStatement( assignmentExpressionBuilder.AssignmentExpression));

            return this;
        }
    }
}