using System.Linq;
using System.Reflection;
using Autofac;
using TransactionApp.Common.Mappings.Abstractions;

namespace TransactionApp.DataAccess.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterDataAccessLayer(this ContainerBuilder builder)
        {
            return builder.RegisterRepositories()
                .RegisterMappers();
        }
        private static ContainerBuilder RegisterRepositories(this ContainerBuilder builder)
        {
            var repositoryTypes = Assembly.GetAssembly(typeof(ContainerBuilderExtensions))
                .GetTypes()
                .Where(t => !t.IsGenericType && !t.IsAbstract && t.BaseType != null)
                .Where(t => t.Name.EndsWith("Repository"))
                .ToList();

            foreach (var type in repositoryTypes)
            {
                if (type.BaseType != null)
                {
                    builder.RegisterType(type)
                        .AsImplementedInterfaces()
                        .InstancePerLifetimeScope();
                }
            }

            return builder;
        }
        private static ContainerBuilder RegisterMappers(this ContainerBuilder builder)
        {
            // mappers
            var mapperTypes = Assembly.GetAssembly(typeof(ContainerBuilderExtensions))
                .GetTypes()
                .Where(t => !t.IsGenericType && !t.IsAbstract && t.BaseType != null)
                .Where(t => t.IsClosedTypeOf(typeof(IMappingProfile<,>)))
                .ToList();

            foreach (var type in mapperTypes)
            {
                if (type.BaseType != null)
                {
                    builder.RegisterType(type)
                        .AsImplementedInterfaces()
                        .SingleInstance();
                }
            }

            return builder;
        }
    }
}