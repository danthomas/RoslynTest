using System;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using TaskRunner.Builders;

namespace TaskRunner
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

            new Tester(compilationUnitBuilder.CompilationUnitSyntax)
                .SetProperty("Prop1", 123)
                .AssertProperty("Prop1", 123);
        }

        [Test]
        public void IdentifierPropertyTest()
        {
            var compilationUnitBuilder = new CompilationUnitBuilder()
                .WithUsings("System")
                .WithNamespace("TestNamespace", nb => nb
                    .WithClass("TestClass", new string[0], cb => cb
                        .WithProperty("DateTime", "Prop1")));

            new Tester(compilationUnitBuilder.CompilationUnitSyntax)
                .SetProperty("Prop1", new DateTime(2020, 2, 28))
                .AssertProperty("Prop1", new DateTime(2020, 2, 28));
        }
    }
}