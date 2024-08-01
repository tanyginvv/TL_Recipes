using System.Collections.Generic;
using System.Linq;
using Recipes.Application.Interfaces;
using Recipes.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Recipes.Infrastructure.Filters
{
    public class SearchFilter : IFilter<Recipe>
    {
        public List<string> SearchTerm { get; set; }

        public IQueryable<Recipe> Apply( IQueryable<Recipe> query )
        {
            if ( SearchTerm is not null && SearchTerm.Any() )
            {
                var normalizedSearchTerms = SearchTerm.Select( term => term.ToLower() ).ToList();
                query = query.Include( r => r.Tags )
                             .Where( r =>
                                 normalizedSearchTerms.Any( term =>
                                     r.Tags.Any( tag => tag.Name.ToLower().Equals( term ) ) ||
                                     r.Name.ToLower().Contains( term ) ||
                                     r.Description.ToLower().Contains( term )
                                 )
                             );
            }

            return query;
        }
    }
}