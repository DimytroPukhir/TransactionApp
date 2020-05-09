using TransactionApp.Common.Mappings.Abstractions;

namespace TransactionApp.Common.Mappings
{
    public static class MapperExtensions
    {
        public static TDestination MapOrNull<TSource, TDestination>(this IMappingProfile<TSource, TDestination> mapper, TSource item)
            where TDestination: class
        {
            var result = item == null ? null : mapper.Map(item);

            return result;
        }
    }
}