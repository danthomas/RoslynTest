using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TaskRunner.Builders
{
    public class BlockSyntaxBuilder
    {
        public BlockSyntax BlockSyntax { get; set; }

        public BlockSyntaxBuilder()
        {
            BlockSyntax = SyntaxFactory.Block();
        }

        public void WithStatements(params Action<StatementSyntaxBuilder>[] actions)
        {
            var statements = actions.Select(x =>
             {
                 var statementSyntaxBuilder = new StatementSyntaxBuilder();
                 x(statementSyntaxBuilder);
                 return statementSyntaxBuilder.StatementSyntax;
             }).ToArray();

            BlockSyntax = BlockSyntax.AddStatements(statements);
        }
    }
}