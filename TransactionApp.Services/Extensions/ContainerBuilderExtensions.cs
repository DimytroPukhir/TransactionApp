using System.Linq;
using System.Reflection;
using Autofac;
using TransactionApp.Common.Mappings.Abstractions;
using TransactionApp.Services.Services.Transactions.Abstractions;
using TransactionApp.Services.Services.Transactions.Parsers;

namespace TransactionApp.Services.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterServicesLayer(this ContainerBuilder builder)
        {
            builder
                .RegisterServices()
                .RegisterProviders()
                .RegisterMappers();
        }


        private static ContainerBuilder RegisterProviders(this ContainerBuilder builder)
        {
            var providersTypes = Assembly.GetAssembly(typeof(ContainerBuilderExtensions))
                .GetTypes()
                .Where(t => !t.IsGenericType && !t.IsAbstract && t.BaseType != null)
                .Where(t => t.Name.EndsWith("Provider"))
                .ToList();

            foreach (var type in providersTypes)
            {
                if (type.BaseType != null)
                {
                    builder.RegisterType(type)
                        .AsImplementedInterfaces()
                        .InstancePerLifetimeScope();
                }
            }

            builder.RegisterType<CsvTransactionParser>()
                .As<ITransactionsDataParser>()
                .InstancePerLifetimeScope();

            builder.RegisterType<XmlTransactionParser>()
                .As<ITransactionsDataParser>()
                .InstancePerLifetimeScope();

            return builder;
        }

        private static ContainerBuilder RegisterServices(this ContainerBuilder builder)
        {
            var servicesTypes = Assembly.GetAssembly(typeof(ContainerBuilderExtensions))
                .GetTypes()
                .Where(t => !t.IsGenericType && !t.IsAbstract && t.BaseType != null)
                .Where(t => t.Name.EndsWith("Service"))
                .ToList();

            foreach (var type in servicesTypes)
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

        private static void RegisterMappers(this ContainerBuilder builder)
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
        }
    }
}