using AssemblyBuilder;
using NUnit.Framework;
using Tests.CommandLineInterface;

namespace Tests.Utils
{
    public class TestRunner
    {
        private readonly TestObjectCompiler _testObjectCompiler;

        public TestRunner(CompilationUnitBuilder compilationUnitBuilder, params object[] constructorArgs)
        {
            _testObjectCompiler = new TestObjectCompiler(compilationUnitBuilder).CreateInstance(constructorArgs);
        }

        public void AssertTests(params Test[] tests)
        {
            foreach (var test in tests)
            {
                AssertTestMethod(test.Expected, test.Args);
            }
        }

        public void AssertTestMethod(object expected, params object[] args)
        {
            var result = _testObjectCompiler.InvokeMethod("TestMethod", args);

            Assert.AreEqual(expected, result);
        }
    }
}