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

            if (true)
            {

            }

            MethodDeclarationSyntax = SyntaxFactory.MethodDeclaration(
                    SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)), name)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddBodyStatements();
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
    }
}