using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Extensions.DependencyInjection;

namespace CommandLineInterface
{
    public class App
    {
        public void Run(IConsole console, IState state, Assembly assembly)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(console);

            var quit = new Quit();

            serviceCollection.AddSingleton(quit);

            foreach (var type in assembly.GetTypes()
                .Where(x => x.IsClass && x.GetInterfaces().Contains(typeof(ITask))))
            {
                serviceCollection.AddTransient(type);
            }

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var taskRunnerBuilder = new TaskRunnerBuilder();

            var patternMatcher = new PatternMatcher();

            var taskRunner = taskRunnerBuilder.Build(serviceProvider, state, assembly, typeof(Quit));

            var commands = new List<string>();
            var commandIndex = -1;

            List<Match> matches = null;
            var matchIndex = -1;

            console.KeyPress = () =>
            {
                matchIndex = -1;
                matches = null;
            };

            console.Enter = line =>
            {
                if (matches == null)
                {
                    var before = line.Contains(' ') ? line.Split(' ')[0] : line;
                    var after = line.Contains(' ') ? line.Split(' ')[1] : "";

                    var exactMatch = taskRunnerBuilder.Tasks.SingleOrDefault(x =>
                        x.Equals(before, StringComparison.CurrentCultureIgnoreCase));

                    if (exactMatch != null)
                    {
                        line = exactMatch + (after == "" ? "" : " " + after);
                        matches = new List<Match> { new Match { Text = exactMatch, Type = MatchType.Full } };
                        matchIndex = 0;
                    }
                    else
                    {
                        matches = taskRunnerBuilder.Tasks.Select(x => patternMatcher.Match(x, before))
                            .Where(x => x.Type != MatchType.None)
                            .OrderBy(x => x.Type)
                            .ThenBy(x => x.Text)
                            .ToList();
                        matchIndex = matches.Any() ? 0 : -1;
                        return matchIndex == 0 ? matches[0].Text + (after == "" ? "" : " " + after) : line;
                    }
                }

                if (matchIndex > -1)
                {
                    if (commands.Contains(line))
                    {
                        commands.Remove(line);
                    }

                    commands.Insert(0, line);

                    var runResult = taskRunner.Run(new RunTaskCommand(line));
                    if (!runResult.Success)
                    {
                        foreach (var error in runResult.Errors)
                        {
                            console.WriteLine(error);
                        }
                    }
                    commandIndex = -1;
                    matches = null;
                    return null;
                }

                return line;
            };

            console.Tab = line =>
            {
                var before = line.Contains(' ') ? line.Split(' ')[0] : line;
                var after = line.Contains(' ') ? line.Split(' ')[1] : "";

                if (matches == null)
                {
                    matches = taskRunnerBuilder.Tasks.Select(x => patternMatcher.Match(x, before))
                        .Where(x => x.Type != MatchType.None)
                        .OrderBy(x => x.Type)
                        .ThenBy(x => x.Text)
                        .ToList();
                    matchIndex = matches.Any() ? 0 : -1;
                    return matchIndex == 0 ? matches[0].Text + (after == "" ? "" : " " + after) : line;
                }

                if (matches.Any())
                {
                    matchIndex++;
                    matchIndex = matchIndex % matches.Count;
                    return matches[matchIndex].Text + (after == "" ? "" : " " + after);
                }

                return line;
            };

            console.Next = () =>
            {
                if (commands.Any() && commandIndex > 0)
                {
                    commandIndex--;
                    return commands[commandIndex];
                }

                return null;
            };


            console.Prev = () =>
            {
                if (commands.Any() && commandIndex < commands.Count - 1)
                {
                    commandIndex++;
                    return commands[commandIndex];
                }

                return null;
            };

            console.Start();

            /*while (!quit.HasQuit)
            {
                var line = console.ReadLine();
                var runTaskCommand = new RunTaskCommand(line);

                var runResult = taskRunner.Run(runTaskCommand);

                if (!runResult.Success)
                {
                    foreach (var error in runResult.Errors)
                    {
                        console.WriteLine(error);
                    }
                }
            }*/
        }
    }
}