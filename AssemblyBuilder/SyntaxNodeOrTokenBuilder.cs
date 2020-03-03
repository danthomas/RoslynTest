using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace AssemblyBuilder
{
    public class SyntaxNodeOrTokenBuilder
    {
        public 
            SyntaxNodeOrToken Build(string name)
        {
            switch (name)
            {
                case "string":
                    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword));
                case "bool":
                    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword));
                case "int":
                    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.IntKeyword));
                default:
                    return SyntaxFactory.IdentifierName(name);
            }
        }
    }
}