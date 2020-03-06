using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AssemblyBuilder;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CommandLineInterface
{
    public class TaskRunnerBuilder
    {
        public ITaskRunner Build(IServiceProvider serviceProvider, IState state, Assembly assembly)
        {
            var taskTypes = assembly.GetTypes()
                .Where(x => x.IsClass && x.GetInterfaces().Contains(typeof(ITask)))
                .ToArray();

            var taskDefs = new TaskDefBuilder().Build(taskTypes);

            var usings = new List<string> { "System", "Microsoft.Extensions.DependencyInjection", "CommandLineInterface" };

            usings.AddRange(taskDefs.Select(x => x.Namespace));
            usings.AddRange(taskDefs.SelectMany(x => x.ArgsPropDefs).Select(x => x.Namespace));
            usings.AddRange(taskDefs.SelectMany(x => x.ParamDefs).Select(x => x.Namespace));

            usings = usings.Distinct().ToList();
            var compilationUnitBuilder = new CompilationUnitBuilder()
                .WithUsings(usings.ToArray())
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
                                if (taskDefs.Any())
                                {
                                    mb.WithIfStatement(isb =>
                                    {
                                        isb.WithBinaryExpression(beb => beb
                                                .WithOperator(SyntaxKind.EqualsExpression)
                                                .WithLeft(eb => eb.SimpleMemberAccess("runTaskCommand", "Name"))
                                                .WithRight(eb => eb.Literal(taskDefs[0].Name)))
                                            .WithBody(bsb => bsb.WithStatements(BuildStatements(taskDefs[0])));

                                        foreach (var taskDef in taskDefs.Skip(1))
                                        {
                                            isb.WithElseIfClause(beb => beb
                                                    .WithOperator(SyntaxKind.EqualsExpression)
                                                    .WithLeft(eb => eb.SimpleMemberAccess("runTaskCommand", "Name"))
                                                    .WithRight(eb => eb.Literal(taskDef.Name)),
                                                BuildStatements(taskDef));
                                        }

                                    });
                                }
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
                "C:\\Users\\dan.thomas\\.nuget\\packages\\microsoft.extensions.dependencyinjection.abstractions\\3.1.2\\lib\\netstandard2.0\\Microsoft.Extensions.DependencyInjection.Abstractions.dll",
                typeof(IServiceProvider).Assembly.Location,
                typeof(ITaskRunner).Assembly.Location,
                assembly.Location
            };

            var type = new Compiler().Compile(compilationUnitBuilder.CompilationUnitSyntax, references)
                .GetType("DynamicTaskRunner.TaskRunner");

            return (ITaskRunner)Activator.CreateInstance(type, serviceProvider, state);
        }

        private Action<StatementSyntaxBuilder>[] BuildStatements(TaskDef taskDef)
        {
            var statements = new List<Action<StatementSyntaxBuilder>>();

            if (!string.IsNullOrWhiteSpace(taskDef.ArgsType))
            {
                statements.Add(
                    ssb => ssb
                        .WithLocalDeclaration(lsdb => lsdb
                            .WithType("var")
                            .WithName("args")
                            .WithInitialiser(esb => esb.WithObjectCreation(taskDef.ArgsType.Split('.')))));

                foreach (var argPropDef in taskDef.ArgsPropDefs)
                {
                    statements.Add(ssb => ssb
                        .WithAssignment(aeb => aeb
                            .WithLeft("args", argPropDef.Name)
                            .WithRight(esb => esb
                                .WithInvocation(ieb => ieb
                                    .WithExpression(esb => esb
                                        .WithIdentifier("runTaskCommand"))
                                    .WithGenericIdentifier("GetValue", argPropDef.Type)
                                    .WithArguments(
                                        asb => asb.WithExpression(esb => esb.Literal(argPropDef.Switch ?? "")),
                                        asb => asb.WithExpression(esb => esb.Literal(argPropDef.Name)),
                                        asb => asb.WithExpression(esb => esb.Literal(argPropDef.IsDefault)))))
                        ));
                }
            }

            statements.Add(ssb => ssb
                .WithInvocation(ieb2 => ieb2
                    .WithExpression(esb => esb
                        .WithInvocation(ieb => ieb
                            .WithExpression(esb => esb
                                .WithIdentifier("_serviceProvider"))
                            .WithGenericIdentifier("GetService", taskDef.Name)))
                    .WithIdentifier("Run")
                    .WithArguments(BuildArguments(taskDef)

                        )));


            return statements.ToArray();
        }

        private Action<ArgumentSyntaxBuilder>[] BuildArguments(TaskDef taskDef)
        {
            var arguments = new List<Action<ArgumentSyntaxBuilder>>();

            foreach (var taskDefParamDef in taskDef.ParamDefs)
            {
                if (taskDefParamDef.IsArgs)
                {
                    arguments.Add(asb => asb
                        .WithExpression(esb => esb
                            .WithIdentifier("args")));
                }
                else
                {
                    arguments.Add(asb => asb
                        .WithExpression(esb => esb
                            .WithInvocation(ieb => ieb
                                .WithExpression(esb => esb
                                    .WithIdentifier("_state"))
                                .WithGenericIdentifier("GetState", taskDefParamDef.Type))));
                }
            }

            return arguments.ToArray();
        }
    }
}