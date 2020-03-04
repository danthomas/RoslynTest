using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace CommandLineInterface
{
    public interface IState
    {
        T GetState<T>() where T : class;
    }

    public interface ITaskFactory
    {
    }

    public class TaskFactory : ITaskFactory
    {
        private readonly IServiceCollection _serviceCollection;

        public TaskFactory(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public void AddTasks(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes()
                .Where(x => x.IsClass && x.GetInterfaces().Contains(typeof(ITask))))
            {
                _serviceCollection.AddTransient(type);
            }
        }
    }
}