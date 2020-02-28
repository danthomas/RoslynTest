using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using TaskRunner.Builders;

namespace TaskRunner
{
    public class ConstructorTests
    {
        [Test]
        public void Test()
        {
            var compilationUnitBuilder = new CompilationUnitBuilder()
                .WithUsings("System")
                .WithNamespace("TestNamespace", nb =>
                    nb.WithClass("TestClass", new string[0], cb =>
                        cb.WithField(SyntaxKind.IntKeyword, "_i")
                            .WithConstructor("TestClass", cb => cb.WithParameter(pb =>
                                    pb.WithName("i")
                                        .WithPredefinedType(SyntaxKind.IntKeyword))
                                .WithAssignmentExpression(aeb =>
                                    aeb.WithLeftExpression("_i")
                                        .WithRightExpression("i")))
                            .WithMethod("TestMethod", mb =>
                                mb.WithReturnType(SyntaxKind.IntKeyword)
                                    .WithStatement(sb =>
                                        sb.WithReturnStatement(rsb =>
                                            rsb.WithExpression(esb =>
                                                esb.Identifier("_i")
                                            )
                                        )
                                    )
                            )
                    )
                );

            new TestRunner(compilationUnitBuilder.CompilationUnitSyntax).RunTest(123, 123);
        }
    }
}