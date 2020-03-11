using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AssemblyBuilder
{
    public class TypeSyntaxBuilder
    {
        public TypeSyntax Build(params string[] names)
        {
            if (names.Length == 1)
            {
                return SyntaxFactory.IdentifierName(names[0]);
            }

            return SyntaxFactory.QualifiedName((NameSyntax)Build(names.Take(names.Length - 1).ToArray()), SyntaxFactory.IdentifierName(names.Last()));
        }
    }
}