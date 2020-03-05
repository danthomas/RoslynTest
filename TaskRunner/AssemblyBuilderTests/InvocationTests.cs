using AssemblyBuilder;
using Microsoft.CodeAnalysis.CSharp;
using NUnit.Framework;
using Tests.Utils;

namespace Tests.AssemblyBuilderTests
{
    public class InvocationTests
    {
        [Test]
        public void InvocationTestLocalMethodNoArgs()
        {
            //this.SetValue();

            var compilationUnitBuilder = new CompilationUnitBuilder()
                .WithUsings("System")
                .WithNamespace("TestNamespace", nb => nb
                    .WithClass("TestClass", new string[0], cb => cb
                        .WithField(SyntaxKind.IntKeyword, "_i")
                        .WithMethod("SetValue", mb => mb.WithStatements(sb => sb
                            .WithAssignment(x => x
                                .WithLeft("_i")
                                .WithRight(esb => esb   
                                    .NumericalLiteral(123)))))
                        .WithMethod("TestMethod", mb => mb
                            .WithReturnType(SyntaxKind.IntKeyword)
                            .WithStatements(sb => sb.WithInvocation(ieb => ieb
                                .WithThisExpression()
                                .WithIdentifier("SetValue")))
                            .WithStatements(sb => sb
                                .WithReturnStatement(rsb => rsb
                                    .WithExpression(esb => esb.WithIdentifier("_i")
                                    ))))));

            new TestObjectCompiler(compilationUnitBuilder)
                .CreateInstance()
                .AssertMethod("TestMethod", 123);
        }

        [Test]
        public void InvocationTestLocalMethodArgs()
        {
            //this.SetValue(100, 1);

            var compilationUnitBuilder = new CompilationUnitBuilder()
                .WithUsings("System")
                .WithNamespace("TestNamespace", nb => nb
                    .WithClass("TestClass", new string[0], cb => cb
                        .WithField(SyntaxKind.IntKeyword, "_i")
                        .WithMethod("SetValue", mb => mb
                            .WithParameter(SyntaxKind.IntKeyword, "i")
                            .WithParameter(SyntaxKind.IntKeyword, "j")
                            .WithStatements(sb => sb
                            .WithAssignment(x => x
                                .WithLeft("_i")
                                .WithRight(esb => esb .WithIdentifier("i")))))
                        .WithMethod("TestMethod", mb => mb
                            .WithReturnType(SyntaxKind.IntKeyword)
                            .WithStatements(sb => sb.WithInvocation(ieb => ieb
                                .WithThisExpression()
                                .WithIdentifier("SetValue")
                                .WithArguments(asb => asb.WithExpression(esb => esb.NumericalLiteral(123)),
                                    asb => asb.WithExpression(esb => esb.NumericalLiteral(1)))))
                            .WithStatements(sb => sb
                                .WithReturnStatement(rsb => rsb
                                    .WithExpression(esb => esb.WithIdentifier("_i")
                                    ))))));

            new TestObjectCompiler(compilationUnitBuilder)
                .CreateInstance()
                .AssertMethod("TestMethod", 123);
        }

        [Test]
        public void InvocationTestMethodNoArgs()
        {
            //thing.SetValue(123);

            var compilationUnitBuilder = new CompilationUnitBuilder()
                .WithUsings("System")
                .WithUsing<Thing>()
                .WithNamespace("TestNamespace", nb => nb
                    .WithClass("TestClass", new string[0], cb => cb
                        .WithMethod("TestMethod", mb => mb
                            .WithParameter("Thing", "thing")
                            .WithStatements(sb => sb.WithInvocation(ieb => ieb
                                .WithExpression(esb => esb.WithIdentifier("thing"))
                                .WithIdentifier("SetIntProp")
                                .WithArguments(asb => asb.WithExpression(esb => esb.NumericalLiteral(123))))))));

            var thing = new Thing();
            new TestObjectCompiler(compilationUnitBuilder, typeof(Thing))
                .CreateInstance()
                .InvokeMethod("TestMethod", thing);

            Assert.AreEqual(123, thing.IntProp);
        }

        [Test]
        public void InvocationTestMethodGenericNoArgs()
        {
            //thing.SetStringProp<string>();

            var compilationUnitBuilder = new CompilationUnitBuilder()
                .WithUsings("System")
                .WithUsing<Thing>()
                .WithNamespace("TestNamespace", nb => nb
                    .WithClass("TestClass", new string[0], cb => cb
                        .WithMethod("TestMethod", mb => mb
                            .WithParameter("Thing", "thing")
                            .WithStatements(sb => sb.WithInvocation(ieb => ieb
                                .WithExpression(esb => esb.WithIdentifier("thing"))
                                .WithGenericIdentifier("SetStringProp", "string"))))));

            var thing = new Thing();
            new TestObjectCompiler(compilationUnitBuilder, typeof(Thing))
                .CreateInstance()
                .InvokeMethod("TestMethod", thing);

            Assert.AreEqual("String", thing.StringProp);
        }

        [Test]
        public void InvocationTestMethodGenericArgs()
        {
            //thing.SetStringProp<string>("Abcd");

            var compilationUnitBuilder = new CompilationUnitBuilder()
                .WithUsings("System")
                .WithUsing<Thing>()
                .WithNamespace("TestNamespace", nb => nb
                    .WithClass("TestClass", new string[0], cb => cb
                        .WithMethod("TestMethod", mb => mb
                            .WithParameter("Thing", "thing")
                            .WithStatements(sb => sb.WithInvocation(ieb => ieb
                                .WithExpression(esb => esb.WithIdentifier("thing"))
                                .WithGenericIdentifier("SetStringProp", "string")
                                .WithArguments(asb => asb.WithExpression(esb => esb.StringLiteral("Abcd"))))))));

            var thing = new Thing();
            new TestObjectCompiler(compilationUnitBuilder, typeof(Thing))
                .CreateInstance()
                .InvokeMethod("TestMethod", thing);

            Assert.AreEqual("String Abcd", thing.StringProp);
        }


        [Test]
        public void Test()
        {
            /*
using System;
using TaskRunner.BuilderTests;

namespace TestNamespace
{
    public class TestClass
    {
        public string TestMethod(Thing thing, string s)
        {
            return thing.GetService<string>().Run(s);
        }
    }
}
*/

            var compilationUnitBuilder = new CompilationUnitBuilder()
                .WithUsings("System")
                .WithUsing<Thing>()
                .WithNamespace("TestNamespace", nb => nb
                    .WithClass("TestClass", new string[0], cb => cb
                        .WithMethod("TestMethod", mb => mb
                            .WithParameter("Thing", "thing")
                            .WithParameter(SyntaxKind.StringKeyword, "s")
                            .WithReturnType(SyntaxKind.StringKeyword)
                            .WithStatements(sb => sb
                                .WithReturnStatement(rsb => rsb.WithExpression(esb3 => esb3
                                    .WithInvocation(ieb2 => ieb2
                                        .WithExpression(esb => esb
                                            .WithInvocation(ieb => ieb
                                                .WithExpression(esb2 => esb2
                                                    .WithIdentifier("thing"))
                                                .WithGenericIdentifier("GetService", "string")))
                                        .WithIdentifier("Run")
                                        .WithArguments(asb => asb.WithExpression(esb => esb.WithIdentifier("s"))))))))));

            var thing = new Thing();
            new TestObjectCompiler(compilationUnitBuilder, typeof(Thing))
                .CreateInstance()
                .AssertMethod("TestMethod", "String Abcd", thing, "Abcd");
        }
    }
}


