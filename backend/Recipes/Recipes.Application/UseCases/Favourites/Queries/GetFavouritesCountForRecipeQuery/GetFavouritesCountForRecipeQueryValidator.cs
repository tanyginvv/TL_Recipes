using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Likes.Queries.GetLikesCountForRecipeQuery
{
    public class GetFavouritesCountForRecipeQueryValidator(
        IRecipeRepository recipeRepository
        ) : IAsyncValidator<GetFavouritesCountForRecipeQuery>
    {
        public async Task<Result> ValidateAsync( GetFavouritesCountForRecipeQuery command )
        {
            if ( await recipeRepository.GetByIdAsync( command.RecipeId ) is null )
            {
                return Result.FromError( "Рецепта с таким id не существует" );
            }

            return Result.Success;
        }
    }
}
