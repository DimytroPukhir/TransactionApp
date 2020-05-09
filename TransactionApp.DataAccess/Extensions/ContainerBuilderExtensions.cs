using System.Linq;
using System.Reflection;
using Autofac;
using TransactionApp.Common.Mappings.Abstractions;
using TransactionApp.DataAccess.DAL.Context;
using TransactionApp.DataAccess.DAL.Infrastructure;
using TransactionApp.DataAccess.DAL.UnitOfWork;

namespace TransactionApp.DataAccess.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterDataAccessLayer(this ContainerBuilder builder)
        {
            builder.RegisterRepositories()
                .RegisterMappers()
                .RegisterOtherDependecies();
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

        private static void RegisterOtherDependecies(this ContainerBuilder builder)
        {
            builder.RegisterType<TransactionsContext>().As<ITransactionsContext>().InstancePerLifetimeScope();
            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();
        }
    }
}