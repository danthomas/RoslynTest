using System;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using TaskRunner.Builders;

namespace TaskRunner
{
    public class ParameterTests
    {
        [Test]
        public void ParametersTest()
        {
            var compilationUnitBuilder = new CompilationUnitBuilder()
                .WithUsings("System")
                .WithNamespace("TestNamespace", nb => nb
                    .WithClass("TestClass", new string[0], cb => cb
                        .WithField(SyntaxKind.IntKeyword, "_i")
                        .WithField(SyntaxKind.StringKeyword, "_s")
                        .WithField(SyntaxKind.DecimalKeyword, "_d")
                        .WithConstructor("TestClass", cb => cb
                            .WithParameter(pb => pb
                                .WithName("i")
                                .WithPredefinedType(SyntaxKind.IntKeyword))
                                .WithAssignmentExpression(aeb => aeb
                                    .WithLeftExpression("_i")
                                    .WithRightExpression("i")))
                        .WithMethod("TestMethod", mb => mb
                            .WithReturnType(SyntaxKind.IntKeyword)
                            .WithStatement(sb => sb
                                .WithReturnStatement(rsb => rsb
                                    .WithExpression(esb => esb.Identifier("_i")
                                        ))))));

            new TestRunner(compilationUnitBuilder.CompilationUnitSyntax, Type.Missing).RunTest(123);
        }

        [Test]
        public void ParameterWithDefaultTest()
        {
            var compilationUnitBuilder = new CompilationUnitBuilder()
                .WithUsings("System")
                .WithNamespace("TestNamespace", nb =>
                    nb.WithClass("TestClass", new string[0], cb =>
                        cb.WithField(SyntaxKind.IntKeyword, "_i")
                            .WithConstructor("TestClass", cb => cb.WithParameter(pb =>
                                    pb.WithName("i")
                                        .WithPredefinedType(SyntaxKind.IntKeyword)
                                        .WithDefault(esb =>
                                            esb.NumericalLiteral(123)))
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

            new TestRunner(compilationUnitBuilder.CompilationUnitSyntax, Type.Missing).RunTest(123);
        }
    }
}