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
    ILikeRepository likeRepository,
    IFavouriteRepository favouriteRepository,
    IAsyncValidator<GetRecipesQuery> validator )
    : QueryBaseHandler<IEnumerable<GetRecipePartDto>, GetRecipesQuery>( validator )
{
    protected override async Task<Result<IEnumerable<GetRecipePartDto>>> HandleImplAsync( GetRecipesQuery query )
    {
        IEnumerable<Recipe> recipes = await recipeRepository.GetRecipesAsync(
            new List<IFilter<Recipe>>
            {
                new SearchFilter { SearchTerms = query.SearchTerms },
                new UserFilter { UserId = query.UserId, IsFavourite = query.isFavourite, IsAuth = query.IsAuth },
                new PaginationFilter { PageNumber = query.PageNumber, PageSize = 4 },
            } );

        List<GetRecipePartDto> recipeDtos = recipes.Adapt<List<GetRecipePartDto>>();

        if ( query.UserId != 0 )
        {
            foreach ( GetRecipePartDto recipeDto in recipeDtos )
            {
                recipeDto.IsLike = await likeRepository.ContainsAsync( like =>
                    like.UserId == query.UserId && like.RecipeId == recipeDto.Id );

                recipeDto.IsFavourite = await favouriteRepository.ContainsAsync( fav =>
                    fav.UserId == query.UserId && fav.RecipeId == recipeDto.Id );
            }
        }

        return Result<IEnumerable<GetRecipePartDto>>.FromSuccess( recipeDtos );
    }
}