
using Recipes.Domain.Entities;

namespace Recipes.Application.Filters;

public class UserFilter : IFilter<Recipe>
{
    public int UserId { get; set; }
    public bool IsFavourite { get; set; }
    public bool IsAuth { get; set; }

    public IQueryable<Recipe> Apply( IQueryable<Recipe> query )
    {
        if ( UserId != 0 && !IsFavourite && IsAuth )
        {
            query = query
                .Where( u => u.AuthorId == UserId );
        }

        if ( IsFavourite && UserId > 0 )
        {
            query = query
                .Where( u => u.Favourites
                .Any( f => f.UserId == UserId ) );
        }

        return query;
    }
}