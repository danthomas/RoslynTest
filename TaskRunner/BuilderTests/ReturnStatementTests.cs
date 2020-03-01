using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using TaskRunner.Builders;
using TaskRunner.Utils;

namespace TaskRunner.BuilderTests
{
    public class ReturnStatementTests
    {
        [Test]
        public void ReturnStatementTest()
        {
            var compilationUnitBuilder = new CompilationUnitBuilder()
                .WithUsings("System")
                .WithNamespace("TestNamespace", nb => nb
                    .WithClass("TestClass", new string[0], cb => cb
                        .WithMethod("TestMethod", mb => mb
                            .WithReturnType(SyntaxKind.StringKeyword)
                            .WithStatement(sb => sb
                                .WithReturnStatement(rsb => rsb
                                    .WithExpression(esb => esb
                                        .StringLiteral("Fnord")))))));

            new TestRunner(compilationUnitBuilder)
                .AssertTestMethod("Fnord");
        }
    }
}