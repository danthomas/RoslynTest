using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TaskRunner.Builders
{
    public class TypeSyntaxBuilder
    {
        public TypeSyntax Build(params string[] names)
        {
            return names.Length == 1
                ? SyntaxFactory.IdentifierName(names[0])
                : (TypeSyntax)SyntaxFactory.QualifiedName(SyntaxFactory.IdentifierName(names[0]), SyntaxFactory.IdentifierName(names[1]));
        }
    }
}