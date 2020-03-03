using System;
using System.Linq;
using AssemblyBuilder;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;

namespace Tests.Utils
{
    public class TestObjectCompiler
    {
        private readonly string[] _references;
        private object _instance;
        private readonly CompilationUnitSyntax _compilationUnitSyntax;

        public TestObjectCompiler(CompilationUnitBuilder compilationUnitBuilder, params Type[] references)
        {
            _references = references.Select(x => x.Assembly.Location).ToArray();
            _compilationUnitSyntax = compilationUnitBuilder.CompilationUnitSyntax;
        }

        public TestObjectCompiler CreateInstance(params object[] args)
        {
            _instance = new TestAssemblyCompiler()
                .Build(_compilationUnitSyntax, _references)
                .CreateInstance("TestNamespace.TestClass", args);

            return this;
        }

        public object InvokeMethod(string name, params object[] args)
        {
            return _instance
                .GetType()
                .GetMethod(name)
                .Invoke(_instance, args);
        }

        public void AssertMethod(string name, object expected, params object[] args)
        {
            Assert.AreEqual(expected, _instance
                .GetType()
                .GetMethod(name)
                .Invoke(_instance, args));
        }

        public TestObjectCompiler SetProperty(string name, object value)
        {
            _instance
                .GetType()
                .GetProperty(name)
                .SetValue(_instance, value);

            return this;
        }

        public object GetPropertyValue(string name)
        {
            return _instance
                .GetType()
                .GetProperty(name)
                .GetValue(_instance);
        }

        public TestObjectCompiler AssertPropertyValue(string name, object expected)
        {
            var actual = _instance
                .GetType()
                .GetProperty(name)
                .GetValue(_instance);

            Assert.AreEqual(expected, actual);

            return this;
        }
    }
}