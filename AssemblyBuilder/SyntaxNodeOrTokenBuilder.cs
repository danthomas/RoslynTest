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
                case "String":
                    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.StringKeyword));
                case "bool":
                case "Boolean":
                    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword));
                case "int":
                case "Int32":
                    return SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.IntKeyword));
                default:
                    return new TypeSyntaxBuilder().Build(name.Split("."));
            }
        }
    }
}