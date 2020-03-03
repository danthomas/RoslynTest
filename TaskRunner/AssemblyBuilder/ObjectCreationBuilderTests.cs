using AssemblyBuilder;
using Microsoft.CodeAnalysis;
using NUnit.Framework;

namespace Tests.AssemblyBuilder
{
    public class ObjectCreationBuilderTests
    {
        [Test]
        public void ObjectCreationBuilderTest()
        {
            var compilationUnitBuilder = new CompilationUnitBuilder()
                .WithUsings("System")
                .WithUsing<Thing>()
                .WithNamespace("TestNamespace", nb => nb
                    .WithClass("TestClass", new string[0], cb => cb
                        .WithMethod("TestMethod", mb => mb
                            .WithStatements(sb => sb
                                    .WithLocalDeclaration(ldb => ldb
                                        .WithType("var")
                                        .WithName("thing")
                                        .WithInitialiser(esb => esb
                                            .WithObjectCreation("Thing", "Args")))))));

            var actual = compilationUnitBuilder.CompilationUnitSyntax.NormalizeWhitespace().ToFullString();

            Assert.AreEqual(@"using System;
using Tests.AssemblyBuilder;

namespace TestNamespace
{
    public class TestClass
    {
        public void TestMethod()
        {
            var thing = new Thing.Args();
        }
    }
}", actual);
        }
    }
}