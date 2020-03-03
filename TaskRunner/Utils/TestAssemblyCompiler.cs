using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace Tests.Utils
{
    public class IfTEsting
    {

        [Test]
        public void Tester()
        {
           

            var ifStatementSyntax = (IfStatementSyntax)BuildIf(2);

            ifStatementSyntax = AddIfStatement(3, ifStatementSyntax);
            ifStatementSyntax = AddIfStatement(4, ifStatementSyntax);
            ifStatementSyntax = AddIfStatement(5, ifStatementSyntax);
            ifStatementSyntax = AddIfStatement(6, ifStatementSyntax);
            ifStatementSyntax = AddIfStatement(7, ifStatementSyntax);
            ifStatementSyntax = AddIfStatement(0, ifStatementSyntax);

            var code = ifStatementSyntax.NormalizeWhitespace().ToString();
        }

        private StatementSyntax BuildIf(int i)
        {
            if (i == 0)
            {
                return SyntaxFactory.Block();
            }

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

        private IfStatementSyntax AddIfStatement(int i, IfStatementSyntax ifStatementSyntax)
        {
            return ifStatementSyntax.WithElse(SyntaxFactory.ElseClause(ifStatementSyntax.Else == null
                ? BuildIf(i)
                : AddIfStatement(i, (IfStatementSyntax)ifStatementSyntax.Else.Statement)));
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