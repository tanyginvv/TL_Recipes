using Recipes.Application.Interfaces;
using Recipes.Domain.Entities;

namespace Recipes.Infrastructure
{
    public class PaginationFilter : IFilter<Recipe>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public IQueryable<Recipe> Apply( IQueryable<Recipe> query )
        {
            var skip = ( PageNumber - 1 ) * PageSize;
            query = query.Skip( skip ).Take( PageSize );
            return query;
        }
    }

}
