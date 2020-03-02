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

        public IfStatementBuilder WithElseClause(Action<BinaryExpressionBuilder> beb, Action<StatementSyntaxBuilder> ecsb)
        {
            var binaryExpressionBuilder = new BinaryExpressionBuilder();
            beb(binaryExpressionBuilder);
            var statementSyntaxBuilder = new StatementSyntaxBuilder();
            ecsb(statementSyntaxBuilder);

            IfStatement = AddIfStatement(IfStatement, binaryExpressionBuilder.BinaryExpression, statementSyntaxBuilder.StatementSyntax);
            return this;
        }

        private IfStatementSyntax AddIfStatement(IfStatementSyntax ifStatementSyntax, BinaryExpressionSyntax binaryExpression, StatementSyntax statement)
        {
            return ifStatementSyntax.WithElse(SyntaxFactory.ElseClause(ifStatementSyntax.Else == null
                ? SyntaxFactory.IfStatement(binaryExpression, SyntaxFactory.Block( statement))
                : AddIfStatement((IfStatementSyntax)ifStatementSyntax.Else.Statement, binaryExpression, statement)));
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