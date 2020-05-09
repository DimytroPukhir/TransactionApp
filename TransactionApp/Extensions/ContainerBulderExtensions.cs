using System.Linq;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using TransactionApp.Common.Mappings.Abstractions;

namespace TransactionApp.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterPresentationLayer(this ContainerBuilder builder)
        {
            builder.RegisterApiControllers(typeof(ApiController).Assembly);
            return builder
                .RegisterMappers();
        }


        private static ContainerBuilder RegisterMappers(this ContainerBuilder builder)
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