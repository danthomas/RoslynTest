using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TaskRunner
{
    public class MethodBuilder
    {
        public MethodBuilder(string name)
        {
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

        public void WithIfStatement(Action<BinaryExpressionBuilder> condition, Action<BlockSyntaxBuilder> then)
        {
            var binaryExpressionBuilder = new BinaryExpressionBuilder();
            condition(binaryExpressionBuilder);
            var blockSyntaxBuilder = new BlockSyntaxBuilder();
            then(blockSyntaxBuilder);
            MethodDeclarationSyntax = MethodDeclarationSyntax.AddBodyStatements(
                SyntaxFactory.IfStatement(binaryExpressionBuilder.BinaryExpression, blockSyntaxBuilder.BlockSyntax));
        }
    }
}