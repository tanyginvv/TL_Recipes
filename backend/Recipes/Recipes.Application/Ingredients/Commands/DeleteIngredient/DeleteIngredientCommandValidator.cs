using Recipes.Application.Repositories;
using Recipes.Application.Validation;

namespace Recipes.Application.Ingredients.Commands.DeleteIngredient
{
    public class DeleteIngredientCommandValidator : IAsyncValidator<DeleteIngredientCommand>
    {
        private readonly IIngredientRepository _ingredientRepository;

        public DeleteIngredientCommandValidator( IIngredientRepository ingredientRepository )
        {
            _ingredientRepository = ingredientRepository;
        }

        public async Task<ValidationResult> ValidationAsync( DeleteIngredientCommand command )
        {
            if ( command.Id <= 0 )
            {
                return ValidationResult.Fail( "Invalid ingredient ID." );
            }

            // Check if the ingredient exists in the repository
            var ingredient = await _ingredientRepository.GetByIdAsync( command.Id );
            if ( ingredient == null )
            {
                return ValidationResult.Fail( "Ingredient not found." );
            }

            // If all validations pass
            return ValidationResult.Ok();
        }
    }
}
