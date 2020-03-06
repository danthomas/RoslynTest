using AssemblyBuilder;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using Tests.Utils;

namespace Tests.AssemblyBuilderTests
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
                            .WithStatements(sb => sb
                                .WithReturnStatement(rsb => rsb.WithExpression(esb => esb
                                    .Literal(123))))))
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
                            .WithStatements(sb => sb
                                .WithReturnStatement(rsb => rsb.WithExpression(esb => esb
                                    .Literal("123"))))))
                );

            new TestObjectCompiler(compilationUnitBuilder)
                .CreateInstance()
                .AssertMethod("TestMethod", "123");
        }
    }
}