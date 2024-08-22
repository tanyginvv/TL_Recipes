using Recipes.Application.Interfaces;
using Recipes.Domain.Entities;

namespace Recipes.Application.Filters;

public class UserFilter : IFilter<Recipe>
{
    public int UserId { get; set; }

    public IQueryable<Recipe> Apply( IQueryable<Recipe> query )
    {
        if ( UserId != 0 )
        {
            query = query
                .Where( u => u.AuthorId == UserId );
        }

        return query;
    }
}