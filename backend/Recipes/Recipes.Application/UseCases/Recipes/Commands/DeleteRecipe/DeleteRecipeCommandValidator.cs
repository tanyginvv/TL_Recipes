using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Recipes.Commands.DeleteRecipe
{
    public class DeleteRecipeCommandValidator(
        IRecipeRepository recipeRepository )
        : IAsyncValidator<DeleteRecipeCommand>
    {
        public async Task<Result> ValidateAsync( DeleteRecipeCommand command )
        {
            if ( command.RecipeId <= 0 )
            {
                return Result.FromError( "ID рецепта должно быть больше нуля" );
            }

            if ( await recipeRepository.GetByIdAsync( command.RecipeId ) is null )
            {
                return Result.FromError( "Такого рецепта не существует" );
            }

            return Result.Success;
        }
    }
}