using System;
using AssemblyBuilder;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using Tests.Utils;

namespace Tests.AssemblyBuilderTests
{
    public class PropertyTests
    {
        [Test]
        public void PredefinedTypePropertyTest()
        {
            var compilationUnitBuilder = new CompilationUnitBuilder()
                .WithUsings("System")
                .WithNamespace("TestNamespace", nb => nb
                    .WithClass("TestClass", new string[0], cb => cb
                        .WithProperty(SyntaxKind.IntKeyword, "Prop1")));

            new TestObjectCompiler(compilationUnitBuilder)
                .CreateInstance()
                .SetProperty("Prop1", 123)
                .AssertPropertyValue("Prop1", 123);
        }

        [Test]
        public void IdentifierPropertyTest()
        {
            var compilationUnitBuilder = new CompilationUnitBuilder()
                .WithUsings("System")
                .WithNamespace("TestNamespace", nb => nb
                    .WithClass("TestClass", new string[0], cb => cb
                        .WithProperty("DateTime", "Prop1")));

            new TestObjectCompiler(compilationUnitBuilder)
                .CreateInstance()
                .SetProperty("Prop1", new DateTime(2020, 2, 28))
                .AssertPropertyValue("Prop1", new DateTime(2020, 2, 28));
        }
    }
}