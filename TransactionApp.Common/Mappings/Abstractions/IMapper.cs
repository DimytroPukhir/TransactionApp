using System.Collections.Generic;

namespace TransactionApp.Common.Mappings.Abstractions
{
    public interface IMapper
    {
        TDestination Map<TSource, TDestination>(TSource source)
            where TDestination : class;

        List<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source)
            where TDestination : class;
    }
}