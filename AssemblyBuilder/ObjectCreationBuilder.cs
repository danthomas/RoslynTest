using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AssemblyBuilder
{
    public class ObjectCreationBuilder
    {
        public ExpressionSyntax Expression { get; set; }

        public ObjectCreationBuilder(params string[] names)
        {
            var typeSyntax = new TypeSyntaxBuilder().Build(names);

            Expression = SyntaxFactory.ObjectCreationExpression(typeSyntax)
                .WithArgumentList(
                SyntaxFactory.ArgumentList());
        }

        public ObjectCreationBuilder WithIdentifier(params string[] names)
        {
            var typeSyntax = new TypeSyntaxBuilder().Build(names);

            Expression = SyntaxFactory.ObjectCreationExpression(typeSyntax).WithArgumentList(
                SyntaxFactory.ArgumentList());
            return this;
        }

        public ObjectCreationBuilder WithArguments(params Action<ArgumentSyntaxBuilder>[] asbs)
        {
            Expression = ((ObjectCreationExpressionSyntax)Expression).AddArgumentListArguments(asbs.Select(x =>
            {
                var argumentSyntaxBuilder = new ArgumentSyntaxBuilder();
                x(argumentSyntaxBuilder);
                return argumentSyntaxBuilder.Argument;
            }).ToArray());
            return this;
        }
    }
}