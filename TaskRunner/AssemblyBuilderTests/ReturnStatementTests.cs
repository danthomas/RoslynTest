using AssemblyBuilder;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using Tests.Utils;

namespace Tests.AssemblyBuilderTests
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
                            .WithStatements(sb => sb
                                .WithReturnStatement(rsb => rsb
                                    .WithExpression(esb => esb
                                        .Literal("Fnord")))))));

            new TestRunner(compilationUnitBuilder)
                .AssertTestMethod("Fnord");
        }
    }
}