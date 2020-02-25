using System;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TaskRunner.Builders
{
    public class IfStatementBuilder
    {
        public IfStatementBuilder()
        {
            IfStatement = SyntaxFactory.IfStatement(SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression), SyntaxFactory.Block());
        }

        public IfStatementSyntax IfStatement { get; set; }

        public IfStatementBuilder WithBinaryExpression(Action<BinaryExpressionBuilder> beb)
        {
            var binaryExpressionBuilder = new BinaryExpressionBuilder();
            beb(binaryExpressionBuilder);
            IfStatement = IfStatement.WithCondition(binaryExpressionBuilder.BinaryExpression);
            return this;
        }

        public IfStatementBuilder WithBody(Action<BlockSyntaxBuilder> bsb)
        {
            var blockSyntaxBuilder = new BlockSyntaxBuilder();
            bsb(blockSyntaxBuilder);
            IfStatement = IfStatement.WithStatement(blockSyntaxBuilder.BlockSyntax);
            return this;
        }
    }
}