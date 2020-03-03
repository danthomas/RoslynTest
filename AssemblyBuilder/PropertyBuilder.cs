using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AssemblyBuilder
{
    public class PropertyBuilder
    {
        public PropertyBuilder(string name)
        {
            PropertyDeclarationSyntax = SyntaxFactory.PropertyDeclaration(
                    SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword)),
                    SyntaxFactory.Identifier(name))
                .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                .WithAccessorList(SyntaxFactory.AccessorList(SyntaxFactory.List(new[]
                        {
                            SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)),
                            SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
                        }
                    )
                ));
        }

        public PropertyBuilder WithType(SyntaxKind syntaxKind)
        {
            PropertyDeclarationSyntax = PropertyDeclarationSyntax
                .WithType(SyntaxFactory.PredefinedType(SyntaxFactory.Token(syntaxKind)));
            return this;
        }

        public PropertyBuilder WithType(string name)
        {
            PropertyDeclarationSyntax = PropertyDeclarationSyntax
                .WithType(SyntaxFactory.IdentifierName(name));
            return this;
        }

        public PropertyDeclarationSyntax PropertyDeclarationSyntax { get; set; }
    }
}