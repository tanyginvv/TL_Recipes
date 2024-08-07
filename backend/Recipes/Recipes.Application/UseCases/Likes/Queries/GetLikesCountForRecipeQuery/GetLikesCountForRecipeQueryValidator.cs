using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Likes.Queries.GetLikesCountForRecipeQuery
{
    public class GetLikesCountForRecipeQueryValidator(
        IRecipeRepository recipeRepository
        ) : IAsyncValidator<GetLikesCountForRecipeQuery>
    {
        public async Task<Result> ValidateAsync( GetLikesCountForRecipeQuery command )
        {
            if ( await recipeRepository.GetByIdAsync( command.RecipeId ) is null )
            {
                return Result.FromError( "Рецепта с таким id не существует" );
            }

            return Result.Success;
        }
    }
}
