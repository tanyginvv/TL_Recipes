using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Recipes.Commands.DeleteRecipe
{
    public class DeleteRecipeCommandValidator( IRecipeRepository recipeRepository )
        : IAsyncValidator<DeleteRecipeCommand>
    {
        private IRecipeRepository _recipeRepository => recipeRepository;

        public async Task<Result> ValidationAsync( DeleteRecipeCommand command )
        {
            if ( command.RecipeId == 0 || command.RecipeId < 0 )
            {
                return Result.FromError( "ID рецепта должно быть больше нуля" );
            }

            if ( await _recipeRepository.GetByIdAsync( command.RecipeId ) == null )
            {
                return Result.FromError( "Такого рецепта не существует" );
            }

            return Result.Success;
        }
    }
}