using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TaskRunner.Builders
{
    public class ParameterBuilder
    {
        public ParameterBuilder()
        {
            ParameterSyntax = SyntaxFactory.Parameter(SyntaxFactory.List<AttributeListSyntax>(),
                new SyntaxTokenList(),
                SyntaxFactory.ParseTypeName(""),
                SyntaxFactory.Identifier(""),
                null);
        }

        public ParameterBuilder WithName(string name)
        {
            ParameterSyntax = ParameterSyntax.WithIdentifier(SyntaxFactory.Identifier(name));
            return this;
        }

        public ParameterBuilder WithType(string type)
        {
            ParameterSyntax = ParameterSyntax.WithType(SyntaxFactory.ParseTypeName(type));
            return this;
        }

        public ParameterBuilder WithPredefinedType(SyntaxKind syntaxKind)
        {
            ParameterSyntax = ParameterSyntax.WithType(SyntaxFactory.PredefinedType(
                SyntaxFactory.Token(syntaxKind)));
            return this;
        }

        public ParameterBuilder WithDefault(Action<ExpressionSyntaxBuilder> esb)
        {
            var expressionSyntaxBuilder = new ExpressionSyntaxBuilder();
            esb(expressionSyntaxBuilder);
            ParameterSyntax = ParameterSyntax.WithDefault(SyntaxFactory.EqualsValueClause(expressionSyntaxBuilder.Expression));
            return this;
        }

        public ParameterSyntax ParameterSyntax { get; set; }
    }
}