using Recipes.Application.Interfaces;
using Recipes.Domain.Entities;

namespace Recipes.Application.Filters
{
    public class PaginationFilter : IFilter<Recipe>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public IQueryable<Recipe> Apply( IQueryable<Recipe> query )
        {
            int skip = ( PageNumber - 1 ) * PageSize;
            query = query
                .Skip( skip )
                .Take( PageSize );
            return query;
        }
    }

}
