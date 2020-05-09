using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using TransactionApp.Common.Extensions;
using TransactionApp.Common.Mappings.Abstractions;
using TransactionApp.DataAccess.Extensions;
using TransactionApp.Services.Extensions;

namespace TransactionApp.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static void CreateContainerBuilder(Assembly mvcAssembly)
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(mvcAssembly);
            // Register web abstractions like HttpContextBase.
            builder.RegisterModule<AutofacWebTypesModule>();

            builder.RegisterFilterProvider();
            builder.RegisterServicesLayer();
            builder.RegisterDataAccessLayer();
            builder.RegisterCommonServices();
            builder.RegisterMappers();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }

        private static void RegisterMappers(this ContainerBuilder builder)
        {
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

        private static ContainerBuilder Register(this ContainerBuilder builder)
        {
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