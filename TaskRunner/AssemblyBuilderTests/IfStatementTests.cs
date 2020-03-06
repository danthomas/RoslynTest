using AssemblyBuilder;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using Tests.CommandLineInterfaceTests;
using Tests.Utils;

namespace Tests.AssemblyBuilderTests
{
    public class IfStatementTests
    {
        [Test]
        public void Test()
        {
            var compilationUnitBuilder = new CompilationUnitBuilder()
                .WithUsings("System")
                .WithNamespace("TestNamespace", nb =>
                    nb.WithClass("TestClass", new string[0], cb =>
                        cb.WithMethod("TestMethod", mb =>
                        {
                            mb.WithReturnType(SyntaxKind.StringKeyword)
                                .WithParameter("int", "i");


                            mb.WithIfStatement(isb =>
                                {
                                    isb
                                        .WithBinaryExpression(beb => beb
                                            .WithLeft(esb => esb.WithIdentifier("i"))
                                            .WithRight(esb => esb.Literal(1)))
                                        .WithBody(bsb => bsb
                                            .WithStatements(x => x
                                                .WithReturnStatement(rsb => rsb
                                                    .WithExpression(esb2 => esb2.Literal("One")))));

                                    isb.WithElseIfClause(beb => beb
                                        .WithLeft(esb => esb.WithIdentifier("i"))
                                        .WithRight(esb => esb.Literal(2)), 
                                        ssb => ssb
                                            .WithReturnStatement(rsb => rsb
                                                .WithExpression(esb2 => esb2.Literal("Two"))));

                                    isb.WithElseIfClause(ecsb => ecsb.WithBinaryExpression(beb => beb
                                        .WithLeft(esb => esb.WithIdentifier("i"))
                                        .WithRight(esb => esb.Literal(3)))
                                        .WithBody(bsb => bsb.WithStatements(ssb => ssb
                                            .WithReturnStatement(rsb => rsb
                                                .WithExpression(esb2 => esb2.Literal("Three"))))));

                                    isb.WithElseIfClause(beb => beb
                                        .WithLeft(esb => esb.WithIdentifier("i"))
                                        .WithRight(esb => esb.Literal(4)), 
                                        ssb => ssb
                                            .WithReturnStatement(rsb => rsb
                                                .WithExpression(esb2 => esb2.Literal("Four"))));

                                })
                                .WithStatements(sb => sb.WithReturnStatement(rsb => rsb.WithExpression(esb => esb.WithStringLiteralExpression(""))));
                        })));

            var actual = compilationUnitBuilder.CompilationUnitSyntax.NormalizeWhitespace().ToFullString();

            new TestRunner(compilationUnitBuilder).AssertTests(
                new Test { Args = new object[] { 1 }, Expected = "One" },
                new Test { Args = new object[] { 2 }, Expected = "Two" },
                new Test { Args = new object[] { 3 }, Expected = "Three" },
                new Test { Args = new object[] { 4 }, Expected = "Four" },
                new Test { Args = new object[] { 5 }, Expected = "" }
                );
        }
    }
}