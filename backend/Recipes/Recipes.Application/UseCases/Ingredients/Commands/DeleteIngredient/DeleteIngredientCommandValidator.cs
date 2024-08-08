using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Ingredients.Commands.DeleteIngredient
{
    public class DeleteIngredientCommandValidator(
        IIngredientRepository ingredientRepository )
        : IAsyncValidator<DeleteIngredientCommand>
    {
        public async Task<Result> ValidateAsync( DeleteIngredientCommand command )
        {
            if ( command.Id <= 0 )
            {
                return Result.FromError( "Неверный ID ингредиента." );
            }

            Ingredient ingredient = await ingredientRepository.GetByIdAsync( command.Id );
            if ( ingredient is null )
            {
                return Result.FromError( "Ингредиент не найден." );
            }

            return Result.Success;
        }
    }
}
