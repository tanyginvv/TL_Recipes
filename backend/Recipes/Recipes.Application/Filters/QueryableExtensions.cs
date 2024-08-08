using Recipes.Application.Interfaces;

namespace Recipes.Application.Filters
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyFilters<T>( this IQueryable<T> query, IEnumerable<IFilter<T>> filters )
        {
            if ( filters is null )
            {
                return query;
            }

            foreach ( IFilter<T> filter in filters )
            {
                query = filter.Apply( query );
            }

            return query;
        }
    }
}