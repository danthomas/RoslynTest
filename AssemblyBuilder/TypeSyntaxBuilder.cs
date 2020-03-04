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

            return (TypeSyntax)SyntaxFactory.QualifiedName((NameSyntax)Build(names.Take(names.Length - 1).ToArray()), SyntaxFactory.IdentifierName(names.Last()));

            if (names.Length == 2)
            {
                return (TypeSyntax)SyntaxFactory.QualifiedName(SyntaxFactory.IdentifierName(names[0]), SyntaxFactory.IdentifierName(names[1]));
            }

            if (names.Length == 3)
            {
                return (TypeSyntax)SyntaxFactory.QualifiedName(SyntaxFactory.QualifiedName((NameSyntax)Build(names[0]), (SimpleNameSyntax)Build(names[1])), (SimpleNameSyntax)Build(names[2]));
            }

            if (names.Length == 4)
            {
                return (TypeSyntax)SyntaxFactory.QualifiedName(SyntaxFactory.QualifiedName(SyntaxFactory.QualifiedName((NameSyntax)Build(names[0]), (SimpleNameSyntax)Build(names[1])), (SimpleNameSyntax)Build(names[2])), (SimpleNameSyntax)Build(names[3]));
            }

            return null;
        }
    }
}