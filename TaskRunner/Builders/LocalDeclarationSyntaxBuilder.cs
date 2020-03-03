using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TaskRunner.Builders
{
    public class LocalDeclarationSyntaxBuilder
    {
        public LocalDeclarationSyntaxBuilder()
        {
            LocalDeclaration = SyntaxFactory.LocalDeclarationStatement(SyntaxFactory.VariableDeclaration(
                SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.IntKeyword))));
        }

        public LocalDeclarationStatementSyntax LocalDeclaration { get; set; }

        public LocalDeclarationSyntaxBuilder WithType(SyntaxKind syntaxKind)
        {
            LocalDeclaration = LocalDeclaration.WithDeclaration(LocalDeclaration.Declaration.WithType(
                SyntaxFactory.PredefinedType(SyntaxFactory.Token(syntaxKind))));
            return this;
        }

        public LocalDeclarationSyntaxBuilder WithType(string name)
        {
            LocalDeclaration = LocalDeclaration.WithDeclaration(LocalDeclaration.Declaration.WithType(
                SyntaxFactory.IdentifierName(name)));
            return this;
        }

        public LocalDeclarationSyntaxBuilder WithName(string name)
        {
            LocalDeclaration = LocalDeclaration.WithDeclaration(LocalDeclaration.Declaration.WithVariables(
                SyntaxFactory.SeparatedList(
                    new[]
                    {
                        SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(name))

                    })));
            return this;
        }

        public LocalDeclarationSyntaxBuilder WithInitialiser(Action<ExpressionSyntaxBuilder> esb)
        {
            var expressionSyntaxBuilder = new ExpressionSyntaxBuilder();
            esb(expressionSyntaxBuilder);

            LocalDeclaration = LocalDeclaration.WithDeclaration(LocalDeclaration.Declaration.WithVariables(
                SyntaxFactory.SeparatedList(
                    new[]
                    {
                        LocalDeclaration.Declaration.Variables[0].WithInitializer(SyntaxFactory.EqualsValueClause(expressionSyntaxBuilder.Expression))
                    })));
            return this;
        }
    }
}