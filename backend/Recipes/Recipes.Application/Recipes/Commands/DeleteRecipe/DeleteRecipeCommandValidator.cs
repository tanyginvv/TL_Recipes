using Application.Validation;
using Recipes.Infrastructure.Repositories;

namespace Recipes.Application.Recipes.Commands.DeleteRecipe
{
    public class DeleteRecipeCommandValidator : IAsyncValidator<DeleteRecipeCommand>
    {
        private readonly IRecipeRepository _recipeRepository;

        public DeleteRecipeCommandValidator( IRecipeRepository recipeRepository )
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<ValidationResult> ValidationAsync( DeleteRecipeCommand command )
        {
            if ( command.RecipeId == 0 || command.RecipeId < 0 )
            {
                return ValidationResult.Fail( "ID рецепта должно быть больше нуля" );
            }

            if ( await _recipeRepository.GetByIdAsync( command.RecipeId ) == null )
            {
                return ValidationResult.Fail( "Такого рецепта не существует" );
            }

            return ValidationResult.Ok();
        }
    }
}