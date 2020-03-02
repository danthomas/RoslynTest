using NUnit.Framework;
using TaskRunner.Builders;
using TaskRunner.Utils;

namespace TaskRunner.BuilderTests
{
    public class ObjectCreationTests
    {
        [Test]
        public void Assignment()
        {
            var compilationUnitBuilder = new CompilationUnitBuilder()
                .WithUsings("System")
                .WithUsing<Thing>()
                .WithNamespace("TestNamespace", nb => nb
                    .WithClass("TestClass", cb => cb
                        .WithProperty("Thing", "Prop")
                        .WithConstructor("TestClass", cb2 => cb2
                            .WithAssignmentExpression(asb => asb
                                .WithLeft("Prop")
                                .WithRight(esb2 => esb2
                                    .WithObjectCreation("Thing"))))));

            var thing = new TestObjectCompiler(compilationUnitBuilder, typeof(Thing))
                .CreateInstance()
                .GetPropertyValue("Prop");

            Assert.IsNotNull(thing);
        }
    }
}