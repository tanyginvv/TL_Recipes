using Mapster;
using Recipes.Domain.Entities;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Results;
using Recipes.Application.Repositories;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.Filters;

namespace Recipes.Application.UseCases.Recipes.Queries.GetRecipes;

public class GetRecipesQueryHandler(
    IRecipeRepository recipeRepository,
    IAsyncValidator<GetRecipesQuery> validator )
    : QueryBaseHandler<IEnumerable<GetRecipePartDto>, GetRecipesQuery>( validator )
{
    protected override async Task<Result<IEnumerable<GetRecipePartDto>>> HandleImplAsync( GetRecipesQuery query )
    {
        IEnumerable<Recipe> recipes = await recipeRepository.GetRecipesAsync(
            new List<IFilter<Recipe>>
            {
                new SearchFilter { SearchTerms = query.SearchTerms },
                new UserFilter { UserId = query.UserId, recipeQueryType = query.RecipeQueryType },
                new PaginationFilter { PageNumber = query.PageNumber, PageSize = PaginationFilter.DefaultPageSize },
            } );

        List<GetRecipePartDto> recipeDtos = recipes.Adapt<List<GetRecipePartDto>>();

        List<int> likedRecipes = recipes.Where( r => r.Likes.Any( l => l.UserId == query.UserId ) ).Select( r => r.Id ).ToList();
        List<int> starredRecipes = recipes.Where( r => r.Favourites.Any( l => l.UserId == query.UserId ) ).Select( r => r.Id ).ToList();

        foreach ( GetRecipePartDto recipeDto in recipeDtos )
        {
            if ( likedRecipes.Contains( recipeDto.Id ) )
            {
                recipeDto.IsLiked = true;
            }

            if ( starredRecipes.Contains( recipeDto.Id ) )
            {
                recipeDto.IsFavourited = true;
            }
        }

        return Result<IEnumerable<GetRecipePartDto>>.FromSuccess( recipeDtos );
    }
}