namespace TransactionApp.Common.Mappings.Abstractions
{
    public interface IMappingProfile<in TSource, out TDestination>
        where TDestination: class
    {
        TDestination Map(TSource source);
    }
}