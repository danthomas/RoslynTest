using System;
using AssemblyBuilder;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CommandLineInterface
{
    public class TaskRunnerBuilder
    {
        public ITaskRunner Build(IServiceProvider serviceProvider, IState state, string reference, string @namespace)
        {
            var compilationUnitBuilder = new CompilationUnitBuilder()
                .WithUsings("System", "Microsoft.Extensions.DependencyInjection", "CommandLineInterface", "Tests", @namespace)
                .WithNamespace("DynamicTaskRunner", nb =>
                {
                    nb.WithClass("TaskRunner", new[] { "ITaskRunner" }, cb =>
                    {
                        cb
                            .WithField("IServiceProvider", "_serviceProvider")
                            .WithField("IState", "_state")
                            .WithConstructor("TaskRunner", cob =>
                            {
                                cob
                                    .WithParameter("IServiceProvider", "serviceProvider")
                                    .WithParameter("IState", "state")
                                    .WithAssignmentExpression(aeb => aeb.WithLeft("_serviceProvider").WithRight("serviceProvider"))
                                    .WithAssignmentExpression(aeb => aeb.WithLeft("_state").WithRight("state"));
                            })
                            .WithMethod("Run", mb =>
                            {
                                mb.WithParameter("IRunTaskCommand", "runTaskCommand");
                                mb.WithIfStatement(isb =>
                                {
                                    isb.WithBinaryExpression(beb => beb
                                            .WithOperator(SyntaxKind.EqualsExpression)
                                            .WithLeft(eb => eb.SimpleMemberAccess("runTaskCommand", "Name"))
                                            .WithRight(eb => eb.StringLiteral("TaskA")))
                                        .WithBody(bsb => bsb
                                            .WithStatements(ssb => ssb
                                                .WithInvocation(ieb2 => ieb2
                                                    .WithExpression(esb => esb
                                                        .WithInvocation(ieb => ieb
                                                            .WithExpression(esb => esb
                                                                .WithIdentifier("_serviceProvider"))
                                                            .WithGenericIdentifier("GetService", "TaskA")))
                                                    .WithIdentifier("Run"))));
                                    /**/

                                    isb.WithElseIfClause(beb => beb
                                            .WithOperator(SyntaxKind.EqualsExpression)
                                            .WithLeft(eb => eb.SimpleMemberAccess("runTaskCommand", "Name"))
                                            .WithRight(eb => eb.StringLiteral("TaskB")), ssb => ssb
                                            .WithLocalDeclaration(lsdb => lsdb
                                                .WithType("var")
                                                .WithName("args")
                                                .WithInitialiser(esb => esb.WithObjectCreation("TaskB", "Args"))), ssb => ssb
                                            .WithAssignment(aeb => aeb
                                                .WithLeft("args", "BoolProp")
                                                .WithRight(esb => esb
                                                    .WithInvocation(ieb => ieb
                                                        .WithExpression(esb => esb
                                                            .WithIdentifier("runTaskCommand"))
                                                        .WithGenericIdentifier("GetValue", "bool")
                                                        .WithArguments(asb => asb.WithExpression(esb => esb.StringLiteral("BoolProp")))))
                                            ), ssb => ssb
                                            .WithAssignment(aeb => aeb
                                                .WithLeft("args", "StringProp")
                                                .WithRight(esb => esb
                                                    .WithInvocation(ieb => ieb
                                                        .WithExpression(esb => esb
                                                            .WithIdentifier("runTaskCommand"))
                                                        .WithGenericIdentifier("GetValue", "string")
                                                        .WithArguments(asb => asb.WithExpression(esb => esb.StringLiteral("StringProp")))))
                                            ),
                                        ssb => ssb
                                            .WithInvocation(ieb2 => ieb2
                                                .WithExpression(esb => esb
                                                    .WithInvocation(ieb => ieb
                                                        .WithExpression(esb => esb
                                                            .WithIdentifier("_serviceProvider"))
                                                        .WithGenericIdentifier("GetService", "TaskB")))
                                                .WithIdentifier("Run")
                                                .WithArguments(asb => asb
                                                        .WithExpression(esb => esb
                                                            .WithIdentifier("args")),
                                                    asb => asb
                                                        .WithExpression(esb => esb
                                                            .WithInvocation(ieb => ieb
                                                                .WithExpression(esb => esb
                                                                    .WithIdentifier("_state"))
                                                                .WithGenericIdentifier("GetState", "Solution"))))));

                                });
                            });
                    });
                });

            var code = compilationUnitBuilder.CompilationUnitSyntax.NormalizeWhitespace().ToString();

            var references = new[]
            {
                "mscorlib.dll",
                "netstandard.dll",
                "System.Private.CoreLib.dll",
                "System.Console.dll",
                "System.Runtime.dll",
                "C:\\Users\\dan.thomas\\.nuget\\packages\\microsoft.extensions.dependencyinjection\\3.1.0\\lib\\netcoreapp3.1\\Microsoft.Extensions.DependencyInjection.dll",
                "C:\\Users\\dan.thomas\\.nuget\\packages\\microsoft.extensions.dependencyinjection.abstractions\\3.1.0\\lib\\netstandard2.0\\Microsoft.Extensions.DependencyInjection.Abstractions.dll",
                typeof(IServiceProvider).Assembly.Location,
                typeof(ITaskRunner).Assembly.Location,
                reference
            };

            var assembly = new Compiler().Compile(compilationUnitBuilder.CompilationUnitSyntax, references);

            var type = assembly.GetType("DynamicTaskRunner.TaskRunner");

            return (ITaskRunner)Activator.CreateInstance(type, serviceProvider, state);
        }
    }
}