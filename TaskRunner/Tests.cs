using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
            new TaskA().Run();


            var syntaxFactory = SyntaxFactory.CompilationUnit();

            syntaxFactory = syntaxFactory.AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")));

            var @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("DynamicTaskRunner"))
                .NormalizeWhitespace();

            var classDeclaration = SyntaxFactory
                .ClassDeclaration("Runner");

            classDeclaration = classDeclaration
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            var commandLineIdentifier = SyntaxFactory.Identifier("commandLine");

            var syntaxToken = SyntaxFactory.Identifier("args");

            var methodDeclaration = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("void"), "Run")
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddParameterListParameters(SyntaxFactory.Parameter(new SyntaxList<AttributeListSyntax>(),
                    new SyntaxTokenList(),
                    SyntaxFactory.ParseTypeName("string"),
                    commandLineIdentifier,
                    null))
                .WithBody(SyntaxFactory.Block(SyntaxFactory.IfStatement(
                    EqualsExpression(SyntaxFactory.IdentifierName(commandLineIdentifier), LiteralExpression("TaskA")),
                        SyntaxFactory.Block(
                            SimpleMemberAccessExpression(ObjectCreationExpression("TaskA"), "Run")),
                        SyntaxFactory.ElseClause(
                            SyntaxFactory.IfStatement(
                            EqualsExpression(SyntaxFactory.IdentifierName(commandLineIdentifier), LiteralExpression("TaskB")), SyntaxFactory.Block(

                                SyntaxFactory.LocalDeclarationStatement(SyntaxFactory.VariableDeclaration(
                                    SyntaxFactory.ParseTypeName("int"),
                                    SyntaxFactory.SeparatedList(new[]
                                    {
                                        SyntaxFactory.VariableDeclarator(syntaxToken, null, SyntaxFactory.EqualsValueClause(ObjectCreationExpression(new []{ "TaskB", "Args"})))
                                    })
                                    )),
                                SimpleMemberAccessExpression(ObjectCreationExpression("TaskB"), "Run", "args")
                            ))))));

            classDeclaration = classDeclaration.AddMembers(methodDeclaration);

            @namespace = @namespace.AddMembers(classDeclaration);

            syntaxFactory = syntaxFactory.AddMembers(@namespace);

            var code = syntaxFactory.NormalizeWhitespace().ToString();
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
            var separatedSyntaxList = SyntaxFactory.SeparatedList(args
                .Select(s => SyntaxFactory.Argument(SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(s)))));

            return SyntaxFactory.ExpressionStatement(
                SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                        @object, SyntaxFactory.IdentifierName(SyntaxFactory.Identifier(name))), 
                    SyntaxFactory.ArgumentList(separatedSyntaxList)));
        }

        private static BinaryExpressionSyntax EqualsExpression(ExpressionSyntax left, ExpressionSyntax right)
        {
            return SyntaxFactory.BinaryExpression(SyntaxKind.EqualsExpression, left, right);
        }

        private static LiteralExpressionSyntax LiteralExpression(string text)
        {
            return SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(text));
        }

        [Test]
        public void TaskBTest()
        {
            var args = new TaskB.Args();

            new TaskB().Run(args);
        }
    }

    class TaskRunner
    {
        public void Run(string commandLine)
        {
            if (commandLine == "TaskA")
            {
                new TaskA().Run();
            }
            else if (commandLine == "TaskB")
            {
                var args = new TaskB.Args();

                new TaskB().Run(args);
            }
        }
    }
}