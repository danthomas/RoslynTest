using AssemblyBuilder;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using Tests.Utils;

namespace Tests.AssemblyBuilder
{
    public class AssignmentExpressionTests
    {
        [Test]
        public void Assignment()
        {
            var compilationUnitBuilder = new CompilationUnitBuilder()
                .WithUsings("System")
                .WithNamespace("TestNamespace", nb => nb
                    .WithClass("TestClass", cb => cb
                        .WithProperty(SyntaxKind.IntKeyword, "Prop")
                        .WithConstructor("TestClass", cb2 => cb2
                            .WithAssignmentExpression(asb => asb
                                .WithLeft("Prop")
                                .WithRight(esb2 => esb2
                                    .NumericalLiteral(123))))));

            new TestObjectCompiler(compilationUnitBuilder)
                .CreateInstance()
                .AssertPropertyValue("Prop", 123);
        }
    }
}