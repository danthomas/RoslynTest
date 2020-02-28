using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TaskRunner.Builders
{
    public class MethodBuilder
    {
        public MethodBuilder(string name)
        {
            MethodDeclarationSyntax = SyntaxFactory.MethodDeclaration(
                SyntaxFactory.PredefinedType(
                    SyntaxFactory.Token(SyntaxKind.VoidKeyword)), name)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddBodyStatements();
        }

        public MethodBuilder WithReturnType(SyntaxKind syntaxKind)
        {
            MethodDeclarationSyntax = MethodDeclarationSyntax.WithReturnType(SyntaxFactory.PredefinedType(
                SyntaxFactory.Token(syntaxKind)));
            return this;
        }

        public MethodBuilder WithReturnType(string name)
        {
            MethodDeclarationSyntax = MethodDeclarationSyntax.WithReturnType(SyntaxFactory.IdentifierName(name));
            return this;
        }

        public MethodDeclarationSyntax MethodDeclarationSyntax { get; set; }

        public MethodBuilder WithParameter(string type, string name)
        {
            MethodDeclarationSyntax = MethodDeclarationSyntax.AddParameterListParameters(
                SyntaxFactory.Parameter(
                    SyntaxFactory.List<AttributeListSyntax>(),
                    new SyntaxTokenList(),
                    SyntaxFactory.ParseTypeName(type),
                    SyntaxFactory.Identifier(name),
                    null));

            return this;
        }

        public MethodBuilder WithIfStatement(Action<IfStatementBuilder> isb)
        {
            var ifStatementBuilder = new IfStatementBuilder();
            isb(ifStatementBuilder);
            MethodDeclarationSyntax = MethodDeclarationSyntax.AddBodyStatements(ifStatementBuilder.IfStatement);
            return this;
        }

        public MethodBuilder WithStatement(Action<StatementSyntaxBuilder> sb)
        {
            var statementSyntaxBuilder = new StatementSyntaxBuilder();
            sb(statementSyntaxBuilder);
            MethodDeclarationSyntax = MethodDeclarationSyntax.AddBodyStatements(statementSyntaxBuilder.StatementSyntax);
            return this;
        }
    }
}