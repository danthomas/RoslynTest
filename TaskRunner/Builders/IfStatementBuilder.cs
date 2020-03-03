using System;
using System.Linq;
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

        public IfStatementBuilder WithElseIfClause(Action<ElseClauseSyntaxBuilder> ecsb)
        {
            var elseClauseSyntaxBuilder = new ElseClauseSyntaxBuilder();
            ecsb(elseClauseSyntaxBuilder);

            IfStatement = AddIfStatement(IfStatement, elseClauseSyntaxBuilder.IfStatement);

            return this;
        }

        public IfStatementBuilder WithElseIfClause(Action<BinaryExpressionBuilder> beb, params Action<StatementSyntaxBuilder>[] ssbs)
        {
            var binaryExpressionBuilder = new BinaryExpressionBuilder();
            beb(binaryExpressionBuilder);

            var statementSyntaxes = ssbs.Select(x =>
            {
                var statementSyntaxBuilder = new StatementSyntaxBuilder();
                x(statementSyntaxBuilder);
                return statementSyntaxBuilder.StatementSyntax;
            }).ToArray();
            
            IfStatement = AddIfStatement(IfStatement, binaryExpressionBuilder.BinaryExpression, statementSyntaxes);
            return this;
        }

        private IfStatementSyntax AddIfStatement(IfStatementSyntax parent, BinaryExpressionSyntax binaryExpression, StatementSyntax[] statements)
        {
            return parent.WithElse(SyntaxFactory.ElseClause(parent.Else == null
                ? SyntaxFactory.IfStatement(binaryExpression, SyntaxFactory.Block(statements))
                : AddIfStatement((IfStatementSyntax)parent.Else.Statement, binaryExpression, statements)));
        }

        private IfStatementSyntax AddIfStatement(IfStatementSyntax parent, IfStatementSyntax child)
        {
            return parent.WithElse(SyntaxFactory.ElseClause(parent.Else == null
                ? child
                : AddIfStatement((IfStatementSyntax)parent.Else.Statement, child)));
        }
    }

    public class ElseClauseSyntaxBuilder
    {
        public IfStatementSyntax IfStatement { get; set; }

        public ElseClauseSyntaxBuilder()
        {
            IfStatement = SyntaxFactory.IfStatement(SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression), SyntaxFactory.Block());
        }

        public ElseClauseSyntaxBuilder WithBinaryExpression(Action<BinaryExpressionBuilder> beb)
        {
            var binaryExpressionBuilder = new BinaryExpressionBuilder();
            beb(binaryExpressionBuilder);
            IfStatement = IfStatement.WithCondition(binaryExpressionBuilder.BinaryExpression);
            return this;
        }

        public ElseClauseSyntaxBuilder WithBody(Action<BlockSyntaxBuilder> bsb)
        {
            var blockSyntaxBuilder = new BlockSyntaxBuilder();
            bsb(blockSyntaxBuilder);
            IfStatement = IfStatement.WithStatement(blockSyntaxBuilder.BlockSyntax);
            return this;
        }
    }

}