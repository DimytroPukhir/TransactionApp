using Autofac;
using TransactionApp.Common.Mappings;
using TransactionApp.Common.Mappings.Abstractions;

namespace TransactionApp.Common.Extensions
{
    public static class CommonContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterCommonServices(this ContainerBuilder builder)
        {
            builder.RegisterType<Mapper>().As<IMapper>().SingleInstance();


            return builder;
        }
    }
}