using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using TaskRunner.Builders;

namespace TaskRunner
{
    public class ReturnStatementTests
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
                                .WithStatement(sb =>
                                    sb.WithReturnStatement(rsb =>
                                        rsb.WithExpression(esb =>
                                            esb.StringLiteral("Fnord")
                                        )
                                    )
                                )
                        )
                    )
                );

            new TestRunner(compilationUnitBuilder.CompilationUnitSyntax).RunTest("Fnord");
        }
    }
}