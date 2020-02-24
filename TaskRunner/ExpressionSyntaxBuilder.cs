using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TaskRunner
{
    public class ExpressionSyntaxBuilder
    {
        public ExpressionSyntax ExpressionSyntax { get; set; }

        public void NumericalLiteral(int i)
        {
            ExpressionSyntax = SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(i));
        }

        public void StringLiteral(string s)
        {
            ExpressionSyntax = SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(s));
        }

        public void SimpleMemberAccess(string left, string right)
        {
            ExpressionSyntax = SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                SyntaxFactory.IdentifierName(left), (SimpleNameSyntax)SyntaxFactory.ParseName(right));
        }
    }
}