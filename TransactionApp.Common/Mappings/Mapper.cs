using System.Collections.Generic;
using System.Linq;
using Autofac;
using TransactionApp.Common.Mappings.Abstractions;

namespace TransactionApp.Common.Mappings
{
    public class Mapper : IMapper
    {
        private readonly IComponentContext _context;

        public Mapper(IComponentContext context)
        {
            _context = context;
        }

        public TDestination Map<TSource, TDestination>(TSource source)
            where TDestination : class
        {
            if (source == null)
            {
                return null;
            }

            var profile = _context.Resolve<IMappingProfile<TSource, TDestination>>();
            var result = profile.Map(source);

            return result;
        }

        public List<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> sourceItems)
            where TDestination : class
        {
            if (sourceItems == null)
            {
                return null;
            }

            var profile = _context.Resolve<IMappingProfile<TSource, TDestination>>();

            var result = sourceItems.Select(i => profile.Map(i)).ToList();

            return result;
        }
    }
}