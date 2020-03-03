using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AssemblyBuilder
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

        public MethodBuilder WithParameter(SyntaxKind syntaxKind, string name)
        {
            MethodDeclarationSyntax = MethodDeclarationSyntax.AddParameterListParameters(
                SyntaxFactory.Parameter(
                    SyntaxFactory.List<AttributeListSyntax>(),
                    new SyntaxTokenList(),
                    SyntaxFactory.PredefinedType(SyntaxFactory.Token(syntaxKind)),
                    SyntaxFactory.Identifier(name),
                    null));

            return this;
        }

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

        public MethodBuilder WithStatements(params Action<StatementSyntaxBuilder>[] sbs)
        {
            var statementSyntaxes = sbs.Select(x =>
            {
                var statementSyntaxBuilder = new StatementSyntaxBuilder();
                x(statementSyntaxBuilder);
                return statementSyntaxBuilder.StatementSyntax;
            }).ToArray();
            
            MethodDeclarationSyntax = MethodDeclarationSyntax.AddBodyStatements(statementSyntaxes);
            return this;
        }

        public MethodBuilder WithExpression(Action<ExpressionStatementBuilder> esb)
        {
            var expressionStatementBuilder = new ExpressionStatementBuilder();
            esb(expressionStatementBuilder);
            MethodDeclarationSyntax = MethodDeclarationSyntax.AddBodyStatements(expressionStatementBuilder.ExpressionStatementSyntax);
            return this;
        }
    }
}