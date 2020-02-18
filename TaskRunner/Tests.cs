using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace TaskRunner
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TaskATest()
        {
            var compilationUnitSyntax = SyntaxFactory.CompilationUnit();

            compilationUnitSyntax = compilationUnitSyntax.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")));
            compilationUnitSyntax = compilationUnitSyntax.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("Microsoft.Extensions.DependencyInjection")));
            compilationUnitSyntax = compilationUnitSyntax.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("TaskRunner")));

            var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("DynamicTaskRunner"))
                .NormalizeWhitespace();

            var classDeclaration = SyntaxFactory
                .ClassDeclaration("TaskRunner");

            classDeclaration = classDeclaration
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            classDeclaration = classDeclaration.AddBaseListTypes(
                SyntaxFactory.SimpleBaseType(SyntaxFactory.ParseTypeName("ITaskRunner")));


            classDeclaration = classDeclaration.AddMembers(
                SyntaxFactory.FieldDeclaration(
                    SyntaxFactory.VariableDeclaration(SyntaxFactory.IdentifierName("IServiceProvider"),
                        SyntaxFactory.SeparatedList(new List<VariableDeclaratorSyntax>
                        {
                            SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier("_serviceProvider"))
                        }))
                    ),
                SyntaxFactory.FieldDeclaration(
                    SyntaxFactory.VariableDeclaration(SyntaxFactory.IdentifierName("IState"),
                        SyntaxFactory.SeparatedList(new List<VariableDeclaratorSyntax>
                        {
                            SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier("_state"))
                        }))
                    )
                );

            classDeclaration = classDeclaration.AddMembers(
                SyntaxFactory.ConstructorDeclaration(SyntaxFactory.List<AttributeListSyntax>(),
                    new SyntaxTokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)),
                    SyntaxFactory.Identifier("TaskRunner"),
                    SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList(new[]
                    {
                        SyntaxFactory.Parameter(SyntaxFactory.List<AttributeListSyntax>(),
                            new SyntaxTokenList(),
                            SyntaxFactory.ParseTypeName("IServiceProvider"),
                            SyntaxFactory.Identifier("serviceProvider"),
                            null),
                        SyntaxFactory.Parameter(SyntaxFactory.List<AttributeListSyntax>(),
                            new SyntaxTokenList(),
                            SyntaxFactory.ParseTypeName("IState"),
                            SyntaxFactory.Identifier("state"),
                            null)
                    })),
                    null,
                    SyntaxFactory.Block(
                        SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                            SyntaxFactory.IdentifierName("_serviceProvider"),
                            SyntaxFactory.IdentifierName("serviceProvider")
                        )
                        ),
                        SyntaxFactory.ExpressionStatement(
                            SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                                SyntaxFactory.IdentifierName("_state"),
                                SyntaxFactory.IdentifierName("state")
                            )
                        )

                        )));

            var methodDeclaration = SyntaxFactory.MethodDeclaration(
                    SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)), "Run")
                        .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                        .AddParameterListParameters(SyntaxFactory.Parameter(new SyntaxList<AttributeListSyntax>(),
                    new SyntaxTokenList(),
                    SyntaxFactory.ParseTypeName("IRunTaskCommand"),
                    SyntaxFactory.Identifier("runTaskCommand"),
                    null))
                .WithBody(SyntaxFactory.Block(
                    SyntaxFactory.IfStatement(EqualsExpression(SimpleMemberAccessExpression(SyntaxFactory.Identifier("runTaskCommand"), "Name"), LiteralExpression("TaskA")),
                        SyntaxFactory.Block(
                            SimpleMemberAccessExpression(SyntaxFactory.InvocationExpression(
                                SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                    SyntaxFactory.IdentifierName("_serviceProvider"),
                                    SyntaxFactory.GenericName(SyntaxFactory.Identifier("GetService"),
                                        SyntaxFactory.TypeArgumentList(
                                            SyntaxFactory.SeparatedList(new TypeSyntax[]
                                            {
                                                SyntaxFactory.IdentifierName(SyntaxFactory.Identifier("TaskA"))
                                            })
                                        ))
                                ),
                                SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>()
                                )
                            ), "Run")),
                        SyntaxFactory.ElseClause(
                            SyntaxFactory.IfStatement(
                            EqualsExpression(SimpleMemberAccessExpression(SyntaxFactory.Identifier("runTaskCommand"), "Name"), LiteralExpression("TaskB")),
                            SyntaxFactory.Block(
                                SyntaxFactory.LocalDeclarationStatement(SyntaxFactory.VariableDeclaration(
                                    SyntaxFactory.ParseTypeName("var"),
                                    SyntaxFactory.SeparatedList(new[]
                                    {
                                        SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier("args"),
                                            null,
                                            SyntaxFactory.EqualsValueClause(ObjectCreationExpression(new []{ "TaskB", "Args"})))
                                    })
                                    )),
                                AssignArgsPropValue("BoolProp", SyntaxKind.BoolKeyword),
                                AssignArgsPropValue("StringProp", SyntaxKind.StringKeyword),
                                SyntaxFactory.ExpressionStatement(
                                    SyntaxFactory.InvocationExpression(
                                        SyntaxFactory.MemberAccessExpression(
                                            SyntaxKind.SimpleMemberAccessExpression,
                                            SyntaxFactory.InvocationExpression(
                                                SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                                    SyntaxFactory.IdentifierName("_serviceProvider"),
                                                    SyntaxFactory.GenericName(SyntaxFactory.Identifier("GetService"),
                                                        SyntaxFactory.TypeArgumentList(
                                                            SyntaxFactory.SeparatedList(new TypeSyntax[]
                                                            {
                                                                SyntaxFactory.IdentifierName(SyntaxFactory.Identifier("TaskB"))
                                                            })
                                                        ))
                                                ),
                                                SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>()
                                                )
                                            ),
                                            SyntaxFactory.IdentifierName(
                                                SyntaxFactory.Identifier("Run")
                                            )
                                        ),
                                        SyntaxFactory.ArgumentList(
                                            SyntaxFactory.SeparatedList(new[] {
                                                SyntaxFactory.Argument(
                                                    SyntaxFactory.IdentifierName(
                                                        SyntaxFactory.Identifier("args")
                                                    )
                                                ),
                                                SyntaxFactory.Argument(SyntaxFactory.InvocationExpression(
                                                    SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                                                        SyntaxFactory.IdentifierName("_state"),
                                                        SyntaxFactory.GenericName(SyntaxFactory.Identifier("GetState"),
                                                            SyntaxFactory.TypeArgumentList(
                                                                SyntaxFactory.SeparatedList(new TypeSyntax[]
                                                                {
                                                                    SyntaxFactory.IdentifierName(SyntaxFactory.Identifier("Solution"))
                                                                })
                                                            ))
                                                    ),
                                                    SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList<ArgumentSyntax>()
                                                    )
                                                ))
                                            }
                                            )
                                        )
                                    )
                                )
                                )
                            )))));

            classDeclaration = classDeclaration.AddMembers(methodDeclaration);

            @namespace = @namespace.AddMembers(classDeclaration);

            compilationUnitSyntax = compilationUnitSyntax.AddMembers(@namespace);

            var code = compilationUnitSyntax.NormalizeWhitespace().ToString();

            var references = new List<string>
            {
                "mscorlib.dll",
                "netstandard.dll",
                "System.Private.CoreLib.dll",
                "System.Console.dll",
                "System.Runtime.dll",
                "C:\\Users\\dan.thomas\\.nuget\\packages\\microsoft.extensions.dependencyinjection\\3.1.0\\lib\\netcoreapp3.1\\Microsoft.Extensions.DependencyInjection.dll",
                "C:\\Users\\dan.thomas\\.nuget\\packages\\microsoft.extensions.dependencyinjection.abstractions\\3.1.0\\lib\\netstandard2.0\\Microsoft.Extensions.DependencyInjection.Abstractions.dll",
                typeof(IServiceProvider).Assembly.Location,
                typeof(ITaskRunner).Assembly.Location
            };



            var serviceCollection = new ServiceCollection();
            var console = new Console();

            serviceCollection.AddTransient<TaskA>();
            serviceCollection.AddTransient<TaskB>();
            serviceCollection.AddSingleton<IConsole>(console);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IState state = new State
            {
                Solution = new Solution { Id = 123 }
            };

            var assembly = Compiler.Compile(compilationUnitSyntax, references);

            var type = assembly.GetType("DynamicTaskRunner.TaskRunner");

            var taskRunner = (ITaskRunner)Activator.CreateInstance(type, serviceProvider, state);

            //  taskRunner = new DynamicTaskRunner.TaskRunner(serviceProvider, state);

            taskRunner.Run(new RunTaskCommand("TaskA"));

            taskRunner.Run(new RunTaskCommand("TaskB"));

            Assert.AreEqual(@"TaskA
TaskB 123  False", console.Text);
        }

        private static ExpressionStatementSyntax AssignArgsPropValue(string propName, SyntaxKind propType)
        {
            return SyntaxFactory.ExpressionStatement(
                SyntaxFactory.AssignmentExpression(SyntaxKind.SimpleAssignmentExpression,
                    SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName("args"),
                        SyntaxFactory.IdentifierName(propName)
                    ),
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("runTaskCommand"),
                            SyntaxFactory.GenericName(SyntaxFactory.Identifier("GetValue"), SyntaxFactory.TypeArgumentList(
                                SyntaxFactory.SeparatedList(
                                    new[]
                                    {
                                        (TypeSyntax)SyntaxFactory.PredefinedType(SyntaxFactory.Token(propType))
                                    }
                                )
                            ))
                        ),
                        SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(
                                new[] { SyntaxFactory.Argument(LiteralExpression(propName)) }
                            )
                        )
                    )
                )
            );
        }

        private static MemberAccessExpressionSyntax SimpleMemberAccessExpression(SyntaxToken runTaskCommandIdentifier, string name)
        {
            return SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                SyntaxFactory.IdentifierName(runTaskCommandIdentifier), (SimpleNameSyntax)SyntaxFactory.ParseName(name));
        }

        private static ObjectCreationExpressionSyntax ObjectCreationExpression(string name, params string[] args)
        {
            return ObjectCreationExpression(new[] { name }, args);
        }

        private static ObjectCreationExpressionSyntax ObjectCreationExpression(string[] names, params string[] args)
        {
            var identifierNameSyntax = names.Length == 1
                ? (TypeSyntax)SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(names[0]))
                : SyntaxFactory.QualifiedName(SyntaxFactory.ParseName(names[0]), (SimpleNameSyntax)SyntaxFactory.ParseName(names[1]));

            var separatedSyntaxList = SyntaxFactory.SeparatedList(args
                .Select(s => SyntaxFactory.Argument(SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(s)))));

            return SyntaxFactory.ObjectCreationExpression(identifierNameSyntax,
                SyntaxFactory.ArgumentList(separatedSyntaxList), null);
        }

        private static ExpressionStatementSyntax SimpleMemberAccessExpression(ExpressionSyntax @object, string name, params string[] args)
        {
            return SyntaxFactory.ExpressionStatement(
                SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        @object,
                        SyntaxFactory.IdentifierName(
                            SyntaxFactory.Identifier(name)
                            )
                        ),
                    SyntaxFactory.ArgumentList(
                        SyntaxFactory.SeparatedList(args.Select(s => SyntaxFactory.Argument(
                            SyntaxFactory.IdentifierName(
                                SyntaxFactory.Identifier(s)
                                )
                            )
                        )
                    )
                    )
                    )
                );
        }

        private static BinaryExpressionSyntax EqualsExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return SyntaxFactory.BinaryExpression(SyntaxKind.EqualsExpression, left, right);
        }

        private static LiteralExpressionSyntax LiteralExpression(string text)
        {
            return SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(text));
        }
    }
}

