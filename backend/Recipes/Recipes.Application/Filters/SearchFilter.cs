using Recipes.Application.Interfaces;
using Recipes.Domain.Entities;

namespace Recipes.Application.Filters
{
    public class SearchFilter : IFilter<Recipe>
    {
        public List<string> SearchTerms { get; set; }

        public IQueryable<Recipe> Apply( IQueryable<Recipe> query )
        {
            if ( SearchTerms != null && SearchTerms.Any() )
            {
                var normalizedSearchTerms = SearchTerms.Select( term => term.ToLower() ).ToList();

                query = query.Where( r =>
                    normalizedSearchTerms.Any( term =>
                        r.Name.ToLower().Contains( term ) ||
                        r.Tags.Any( tag => tag.Name.ToLower().Contains( term ) )
                    ) );
            }

            return query;
        }
    }
}
