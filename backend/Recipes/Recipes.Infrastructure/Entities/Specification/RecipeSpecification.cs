using Microsoft.EntityFrameworkCore;
using Recipes.Application.Interfaces;
using Recipes.Application.Paginator;
using Recipes.Domain.Entities;

namespace Recipes.Infrastructure.Entities.Specification
{
    public class RecipeSpecification(
        IEnumerable<string> searchTerms,
        PaginationFilter paginationFilter )
        : ISpecification<Recipe>
    {
        public IQueryable<Recipe> Apply( IQueryable<Recipe> query )
        {
            if ( searchTerms is not null && searchTerms.Any() )
            {
                var normalizedSearchTerms = searchTerms.Select( term => term.ToLower() ).ToList();
                query = query
                    .Where( r =>
                        normalizedSearchTerms.Any( term =>
                            r.Tags.Any( tag => tag.Name.ToLower().Equals( term ) ) ||
                            r.Name.ToLower().Contains( term )
                        )
                    );
            }

            query = query
                .Include( r => r.Tags )
                .Skip( ( paginationFilter.PageNumber - 1 ) * paginationFilter.PageSize )
                .Take( paginationFilter.PageSize );

            return query;
        }
    }
}