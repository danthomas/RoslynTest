using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace TaskRunner.Builders
{
    public class SyntaxNodeOrTokenBuilder
    {
        public SyntaxNodeOrToken Build(string name)
        {
            if (name == "string")
            {
                return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword));
            }

            return SyntaxFactory.IdentifierName(name);
        }
    }
}