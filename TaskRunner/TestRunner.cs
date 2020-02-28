using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace TaskRunner
{
    public class TestRunner
    {
        private readonly object _instance;
        private readonly MethodInfo _methodInfo;

        public TestRunner(CompilationUnitSyntax compilationUnitSyntax, params object[] args)
        {
            (_instance, _methodInfo) = new TestObjectCompiler().CreateInstance(compilationUnitSyntax, args);
        }

        public void RunTests(params Test[] tests)
        {
            foreach (var test in tests)
            {
                RunTest(test.Expected, test.Args);
            }
        }

        public void RunTest(object expected, params object[] args)
        {
            var result = _methodInfo.Invoke(_instance, args);

            Assert.AreEqual(expected, result);
        }
    }

    public class Tester
    {
        private readonly object _instance;
        private MethodInfo _methodInfo;

        public Tester(CompilationUnitSyntax compilationUnitSyntax, params object[] args)
        {
            (_instance, _methodInfo) = new TestObjectCompiler().CreateInstance(compilationUnitSyntax, args);
        }

        public Tester SetProperty(string name, object value)
        {
            _instance.GetType().GetProperty(name).SetValue(_instance, value);
            return this;
        }

        public Tester AssertProperty(string name, object expected)
        {
            var actual = _instance.GetType().GetProperty(name).GetValue(_instance);
            Assert.AreEqual(expected, actual);
            return this;
        }
    }
}