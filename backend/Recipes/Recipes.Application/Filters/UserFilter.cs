using Recipes.Application.UseCases.Recipes.Queries.GetRecipes;
using Recipes.Domain.Entities;

namespace Recipes.Application.Filters;

public class UserFilter : IFilter<Recipe>
{
    public int UserId { get; set; }
    public RecipeQueryType RecipeQueryType { get; set; }

    public IQueryable<Recipe> Apply( IQueryable<Recipe> query )
    {
        if ( UserId != 0 && RecipeQueryType == RecipeQueryType.My )
        {
            query = query
                .Where( u => u.AuthorId == UserId );
        }

        if ( UserId != 0 && RecipeQueryType == RecipeQueryType.Starred )
        {
            query = query
                .Where( u => u.Favourites.Any( f => f.UserId == UserId ) );
        }

        return query;
    }
}