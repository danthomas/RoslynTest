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
                            mb.WithReturnType(SyntaxKind.StringKeyword)
                                .WithParameter("int", "i")
                                .WithIfStatement(isb => isb
                                    .WithBinaryExpression(beb => beb
                                        .WithLeft(esb => esb.WithIdentifier("i"))
                                        .WithRight(esb => esb.NumericalLiteral(1)))
                                    .WithBody(bsb => bsb
                                        .WithStatements(x => x
                                            .WithReturnStatement(rsb => rsb
                                                .WithExpression(esb2 => esb2.StringLiteral("One")))))
                                    .WithElseClause(ecb => ecb.WithIf(isb2 => isb2
                                        .WithBinaryExpression(beb => beb
                                            .WithLeft(esb => esb.WithIdentifier("i"))
                                            .WithRight(esb => esb.NumericalLiteral(2)))
                                        .WithBody(bsb => bsb
                                            .WithStatements(x => x
                                                .WithReturnStatement(rsb => rsb
                                                    .WithExpression(esb2 => esb2.StringLiteral("Two")))))))
                                    .WithElseClause(isb2 => isb2
                                        .WithIf(z => z
                                            .WithBinaryExpression(beb => beb
                                                .WithLeft(esb => esb.WithIdentifier("i"))
                                                .WithRight(esb => esb.NumericalLiteral(3)))
                                            .WithBody(bsb => bsb
                                                .WithStatements(x => x.WithReturnStatement(rsb => rsb
                                                .WithExpression(esb2 => esb2.StringLiteral("Three"))))))))
                                .WithStatement(sb => sb
                                    .StatementSyntax = SyntaxFactory.ReturnStatement(SyntaxFactory.LiteralExpression(
                                        SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(""))))
                        )));

            new TestRunner(compilationUnitBuilder).AssertTests(
                new Test { Args = new object[] { 1 }, Expected = "One" },
                new Test { Args = new object[] { 2 }, Expected = "Two" },
                new Test { Args = new object[] { 3 }, Expected = "Three" },
                new Test { Args = new object[] { 4 }, Expected = "" }
                );
        }
    }
}