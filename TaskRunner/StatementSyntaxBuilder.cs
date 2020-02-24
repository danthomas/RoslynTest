using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TaskRunner
{
    public class StatementSyntaxBuilder
    {
        public StatementSyntax StatementSyntax { get; set; }

        public StatementSyntaxBuilder()
        {
            StatementSyntax = SyntaxFactory.Block();
        }
    }
}