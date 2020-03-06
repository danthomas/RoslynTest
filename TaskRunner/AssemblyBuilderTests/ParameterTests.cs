using System;
using AssemblyBuilder;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using Tests.Utils;

namespace Tests.AssemblyBuilderTests
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
                                    .WithLeft("_i")
                                    .WithRight("i")))
                        .WithMethod("TestMethod", mb => mb
                            .WithReturnType(SyntaxKind.IntKeyword)
                            .WithStatements(sb => sb
                                .WithReturnStatement(rsb => rsb
                                    .WithExpression(esb => esb.WithIdentifier("_i")
                                        ))))));

            new TestObjectCompiler(compilationUnitBuilder)
                .CreateInstance(123)
                .AssertMethod("TestMethod", 123);
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
                                            esb.Literal(123)))
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

            new TestRunner(compilationUnitBuilder, Type.Missing).AssertTestMethod(123);
        }
    }
}