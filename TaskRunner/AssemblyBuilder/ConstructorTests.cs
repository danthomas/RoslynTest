using AssemblyBuilder;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using Tests.Utils;

namespace Tests.AssemblyBuilder
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
                                    aeb.WithLeft("_i")
                                        .WithRight("i")))
                            .WithMethod("TestMethod", mb =>
                                mb.WithReturnType(SyntaxKind.IntKeyword)
                                    .WithStatements(sb =>
                                        sb.WithReturnStatement(rsb =>
                                            rsb.WithExpression(esb =>
                                                esb.WithIdentifier("_i")
                                            )
                                        )
                                    )
                            )
                    )
                );

            new TestRunner(compilationUnitBuilder, 123).AssertTestMethod(123);
        }
    }
}