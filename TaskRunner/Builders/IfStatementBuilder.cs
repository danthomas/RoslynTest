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

        public IfStatementBuilder WithElseClause(Action<ElseClauseSyntaxBuilder> ecsb)
        {
            IfStatementSyntax WithElse(IfStatementSyntax ifStatementSyntax,
                ElseClauseSyntaxBuilder elseClauseSyntaxBuilder1)
            {
                if (ifStatementSyntax.Else != null)
                {
                    return WithElse(ifStatementSyntax.WithElse(ifStatementSyntax.Else), elseClauseSyntaxBuilder1);
                }
                return ifStatementSyntax.WithElse(elseClauseSyntaxBuilder1.ElseClauseSyntax);
            }

            var elseClauseSyntaxBuilder = new ElseClauseSyntaxBuilder();
            ecsb(elseClauseSyntaxBuilder);

            IfStatement =  WithElse(IfStatement, elseClauseSyntaxBuilder);
            return this;
        }
    }

    public class ElseClauseSyntaxBuilder
    {

        public ElseClauseSyntax ElseClauseSyntax { get; set; }
        public ElseClauseSyntaxBuilder()
        {
            ElseClauseSyntax = SyntaxFactory.ElseClause(SyntaxFactory.Block());
        }

        public ElseClauseSyntaxBuilder WithIf(Action<IfStatementBuilder> isb)
        {
            var ifStatementBuilder = new IfStatementBuilder();
            isb(ifStatementBuilder);
            ElseClauseSyntax = SyntaxFactory.ElseClause(ifStatementBuilder.IfStatement);
            return this;
        }
    }

}