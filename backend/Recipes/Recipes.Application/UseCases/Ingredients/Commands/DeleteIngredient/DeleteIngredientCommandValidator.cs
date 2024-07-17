using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Ingredients.Commands.DeleteIngredient
{
    public class DeleteIngredientCommandValidator( IIngredientRepository ingredientRepository ) : IAsyncValidator<DeleteIngredientCommand>
    {
        private IIngredientRepository _ingredientRepository => ingredientRepository;
        public async Task<Result> ValidationAsync( DeleteIngredientCommand command )
        {
            if ( command.Id <= 0 )
            {
                return Result.FromError( "Invalid ingredient ID." );
            }

            var ingredient = await _ingredientRepository.GetByIdAsync( command.Id );
            if ( ingredient is null )
            {
                return Result.FromError( "Ingredient not found." );
            }

            return Result.Success;
        }
    }
}
