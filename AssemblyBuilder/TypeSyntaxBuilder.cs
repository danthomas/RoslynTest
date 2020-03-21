using System;
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

    public class TypeSyntaxBuilder2
    {
        public TypeSyntax TypeSyntax { get; set; }

        public TypeSyntaxBuilder2()
        {

        }

        public void WithName(string name)
        {
            TypeSyntax = new TypeSyntaxBuilder().Build(name.Split('.').ToArray());
        }

        public void WithGenericName(string name, Action<TypeSyntaxBuilder2> tsb)
        {
            var typeSyntaxBuilder2 = new TypeSyntaxBuilder2();
            tsb(typeSyntaxBuilder2);

            TypeSyntax = SyntaxFactory.GenericName(SyntaxFactory.Identifier(name),
                SyntaxFactory.TypeArgumentList(
                    SyntaxFactory.SeparatedList(new[] { typeSyntaxBuilder2.TypeSyntax })));
        }
    }
}