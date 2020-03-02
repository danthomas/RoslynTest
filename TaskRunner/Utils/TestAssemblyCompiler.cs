using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace TaskRunner.Utils
{
    public class IfTEsting
    {

        [Test]
        public void Tester()
        {
            IfStatementSyntax BuildIf(int i)
            {
                return SyntaxFactory.IfStatement(
                    SyntaxFactory.BinaryExpression(
                        SyntaxKind.EqualsExpression,
                        SyntaxFactory.LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            SyntaxFactory.Literal(1)),
                        SyntaxFactory.LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            SyntaxFactory.Literal(i))),
                    SyntaxFactory.Block());
            }

            var ifStatementSyntax = BuildIf(2);

            var code = ifStatementSyntax.NormalizeWhitespace().ToString();

            ifStatementSyntax = ifStatementSyntax.WithElse(SyntaxFactory.ElseClause(BuildIf(3)));

            var code2 = ifStatementSyntax.NormalizeWhitespace().ToString();

            ifStatementSyntax = ifStatementSyntax
                .WithElse(SyntaxFactory
                    .ElseClause(((IfStatementSyntax)ifStatementSyntax.Else.Statement)
                        .WithElse(SyntaxFactory.ElseClause(BuildIf(4)))));

            var code3 = ifStatementSyntax.NormalizeWhitespace().ToString();

            ifStatementSyntax = ifStatementSyntax
                .WithElse(SyntaxFactory
                    .ElseClause(((IfStatementSyntax)ifStatementSyntax.Else.Statement)
                        .WithElse(SyntaxFactory
                            .ElseClause(((IfStatementSyntax)((IfStatementSyntax)ifStatementSyntax.Else.Statement).Else.Statement)
                                .WithElse(SyntaxFactory.ElseClause(BuildIf(5)))))));

            var code4 = ifStatementSyntax.NormalizeWhitespace().ToString();

            ifStatementSyntax = ifStatementSyntax
                .WithElse(SyntaxFactory
                    .ElseClause(((IfStatementSyntax)ifStatementSyntax.Else.Statement)
                        .WithElse(SyntaxFactory
                            .ElseClause(((IfStatementSyntax)((IfStatementSyntax)ifStatementSyntax.Else.Statement).Else.Statement)
                                .WithElse(SyntaxFactory.ElseClause(BuildIf(5)))))));

            var code5 = ifStatementSyntax.NormalizeWhitespace().ToString();

        }



        [Test]
        public void Test()
        {
            var ifStatementSyntax = SyntaxFactory.IfStatement(
                    SyntaxFactory.BinaryExpression(
                        SyntaxKind.EqualsExpression,
                        SyntaxFactory.LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            SyntaxFactory.Literal(1)),
                        SyntaxFactory.LiteralExpression(
                            SyntaxKind.NumericLiteralExpression,
                            SyntaxFactory.Literal(2))),
                    SyntaxFactory.Block())
                .WithElse(
                    SyntaxFactory.ElseClause(
                        SyntaxFactory.IfStatement(
                                SyntaxFactory.BinaryExpression(
                                    SyntaxKind.EqualsExpression,
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.NumericLiteralExpression,
                                        SyntaxFactory.Literal(1)),
                                    SyntaxFactory.LiteralExpression(
                                        SyntaxKind.NumericLiteralExpression,
                                        SyntaxFactory.Literal(3))),
                                SyntaxFactory.Block())
                            .WithElse(
                                SyntaxFactory.ElseClause(
                                    SyntaxFactory.IfStatement(
                                            SyntaxFactory.BinaryExpression(
                                                SyntaxKind.EqualsExpression,
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.NumericLiteralExpression,
                                                    SyntaxFactory.Literal(1)),
                                                SyntaxFactory.LiteralExpression(
                                                    SyntaxKind.NumericLiteralExpression,
                                                    SyntaxFactory.Literal(4))),
                                            SyntaxFactory.Block())
                                        .WithElse(
                                            SyntaxFactory.ElseClause(
                                                SyntaxFactory.IfStatement(
                                                        SyntaxFactory.BinaryExpression(
                                                            SyntaxKind.EqualsExpression,
                                                            SyntaxFactory.LiteralExpression(
                                                                SyntaxKind.NumericLiteralExpression,
                                                                SyntaxFactory.Literal(1)),
                                                            SyntaxFactory.LiteralExpression(
                                                                SyntaxKind.NumericLiteralExpression,
                                                                SyntaxFactory.Literal(5))),
                                                        SyntaxFactory.Block())
                                                    ))))));

            var original = ifStatementSyntax;

            var code = ifStatementSyntax
                .NormalizeWhitespace()
                .ToString();

            ifStatementSyntax = Sub(ifStatementSyntax);

            IfStatementSyntax Sub(IfStatementSyntax parent)
            {
                if (parent.Else == null)
                {
                    return parent.WithElse(SyntaxFactory.ElseClause(
                        SyntaxFactory.IfStatement(
                            SyntaxFactory.BinaryExpression(
                                SyntaxKind.EqualsExpression,
                                SyntaxFactory.LiteralExpression(
                                    SyntaxKind.NumericLiteralExpression,
                                    SyntaxFactory.Literal(1)),
                                SyntaxFactory.LiteralExpression(
                                    SyntaxKind.NumericLiteralExpression,
                                    SyntaxFactory.Literal(6))),
                            SyntaxFactory.Block())));
                }
                else
                {
                    parent = ((IfStatementSyntax)parent.Else.Statement).WithElse(parent.Else);
                    return Sub(parent);
                }
            }


            code = ifStatementSyntax
                            .NormalizeWhitespace()
                            .ToString();

            var originalCode = original.NormalizeWhitespace().ToString();
        }
    }

    public class TestAssemblyCompiler
    {
        private Assembly _assembly;

        public TestAssemblyCompiler Build(CompilationUnitSyntax compilationUnitSyntax, params string[] references)
        {
            references = references.Union(
            new List<string>
            {
                "mscorlib.dll",
                "netstandard.dll",
                "System.Private.CoreLib.dll",
                "System.Runtime.dll"
            }).ToArray();

            _assembly = Compiler.Compile(compilationUnitSyntax, references);

            return this;
        }

        public object CreateInstance(string name, params object[] args)
        {
            var type = _assembly.GetType(name);

            Activator.CreateInstance(type,
                BindingFlags.CreateInstance |
                BindingFlags.Public |
                BindingFlags.Instance |
                BindingFlags.OptionalParamBinding, null, args, CultureInfo.CurrentCulture);

            var instance = Activator.CreateInstance(type, args);

            return instance;
        }
    }
}