using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using TaskRunner.Builders;
using TaskRunner.Utils;

namespace TaskRunner.BuilderTests
{
    public class ClassBuilderTests
    {
        [Test]
        public void ClassBuilderTest()
        {
            var compilationUnitBuilder = new CompilationUnitBuilder()
                .WithUsings("System")
                .WithNamespace("TestNamespace", nb => nb
                    .WithClass("TestClass1")
                    .WithClass("TestClass2")
                    .WithClass("TestClass3")
                );

            var testAssemblyCompiler = new TestAssemblyCompiler()
                .Build(compilationUnitBuilder.CompilationUnitSyntax);

            Assert.IsNotNull(testAssemblyCompiler.CreateInstance("TestNamespace.TestClass1"));
            Assert.IsNotNull(testAssemblyCompiler.CreateInstance("TestNamespace.TestClass2"));
            Assert.IsNotNull(testAssemblyCompiler.CreateInstance("TestNamespace.TestClass3"));
        }
    }

    public class ExpressionSyntaxBuilderTests
    {
        [Test]
        public void ExpressionSyntaxBuilderNumericalLiteralTest()
        {
            var compilationUnitBuilder = new CompilationUnitBuilder()
                .WithUsings("System")
                .WithNamespace("TestNamespace", nb => nb
                    .WithClass("TestClass", cb => cb
                        .WithMethod("TestMethod", mb => mb
                            .WithReturnType(SyntaxKind.IntKeyword)
                            .WithStatement(sb => sb
                                .WithReturnStatement(rsb => rsb.WithExpression(esb => esb
                                    .NumericalLiteral(123))))))
                );

            new TestObjectCompiler(compilationUnitBuilder)
                .CreateInstance()
                .AssertMethod("TestMethod", 123);
        }

        [Test]
        public void ExpressionSyntaxBuilderStringLiteralTest()
        {
            var compilationUnitBuilder = new CompilationUnitBuilder()
                .WithUsings("System")
                .WithNamespace("TestNamespace", nb => nb
                    .WithClass("TestClass", cb => cb
                        .WithMethod("TestMethod", mb => mb
                            .WithReturnType(SyntaxKind.StringKeyword)
                            .WithStatement(sb => sb
                                .WithReturnStatement(rsb => rsb.WithExpression(esb => esb
                                    .StringLiteral("123"))))))
                );

            new TestObjectCompiler(compilationUnitBuilder)
                .CreateInstance()
                .AssertMethod("TestMethod", "123");
        }
    }
}