using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using TaskRunner.Builders;
using TaskRunner.Utils;

namespace TaskRunner.BuilderTests
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
                                            .WithRight(esb => esb.NumericalLiteral(1)))
                                        .WithBody(bsb => bsb
                                            .WithStatements(x => x
                                                .WithReturnStatement(rsb => rsb
                                                    .WithExpression(esb2 => esb2.StringLiteral("One")))));

                                    isb.WithElseClause(beb => beb
                                        .WithLeft(esb => esb.WithIdentifier("i"))
                                        .WithRight(esb => esb.NumericalLiteral(2)), 
                                        ssb => ssb
                                            .WithReturnStatement(rsb => rsb
                                                .WithExpression(esb2 => esb2.StringLiteral("Two"))));

                                    isb.WithElseClause(ecsb => ecsb.WithBinaryExpression(beb => beb
                                        .WithLeft(esb => esb.WithIdentifier("i"))
                                        .WithRight(esb => esb.NumericalLiteral(3)))
                                        .WithBody(bsb => bsb.WithStatements(ssb => ssb
                                            .WithReturnStatement(rsb => rsb
                                                .WithExpression(esb2 => esb2.StringLiteral("Three"))))));

                                    isb.WithElseClause(beb => beb
                                        .WithLeft(esb => esb.WithIdentifier("i"))
                                        .WithRight(esb => esb.NumericalLiteral(4)), 
                                        ssb => ssb
                                            .WithReturnStatement(rsb => rsb
                                                .WithExpression(esb2 => esb2.StringLiteral("Four"))));

                                })
                                .WithStatement(sb => sb
                                    .StatementSyntax = SyntaxFactory.ReturnStatement(SyntaxFactory.LiteralExpression(
                                    SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(""))));
                        })));

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