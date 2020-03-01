using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TaskRunner.Builders
{
    public class ObjectCreationBuilder
    {
        public ExpressionSyntax Expression { get; set; }

        public ObjectCreationBuilder(string name)
        {
            Expression = SyntaxFactory.ObjectCreationExpression(SyntaxFactory.IdentifierName(name))
                .WithArgumentList(
                SyntaxFactory.ArgumentList());
        }

        public void WithIdentifier(string name)
        {
            Expression = SyntaxFactory.ObjectCreationExpression(SyntaxFactory.IdentifierName(name)).WithArgumentList(
                SyntaxFactory.ArgumentList());
        }

        public void WithArguments(params Action<ArgumentSyntaxBuilder>[] asbs)
        {
            Expression = ((ObjectCreationExpressionSyntax) Expression).AddArgumentListArguments(asbs.Select(x =>
            {
                var argumentSyntaxBuilder = new ArgumentSyntaxBuilder();
                x(argumentSyntaxBuilder);
                return argumentSyntaxBuilder.Argument;
            }).ToArray());
        }
    }

    public class ArgumentSyntaxBuilder
    {
        public ArgumentSyntax Argument { get; set; }
    }
}