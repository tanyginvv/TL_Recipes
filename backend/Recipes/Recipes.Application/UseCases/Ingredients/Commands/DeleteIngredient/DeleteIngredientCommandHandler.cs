using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Ingredients.Commands.DeleteIngredient
{
    public class DeleteIngredientCommandHandler(
        IIngredientRepository ingredientRepository,
        IAsyncValidator<DeleteIngredientCommand> validator )
        : ICommandHandler<DeleteIngredientCommand>
    {
        public async Task<Result> HandleAsync( DeleteIngredientCommand command )
        {
            Result validationResult = await validator.ValidateAsync( command );
            if ( !validationResult.IsSuccess )
            {
                return Result.FromError( validationResult.Error );
            }

            Ingredient ingredient = await ingredientRepository.GetByIdAsync( command.Id );
            if ( ingredient is null )
            {
                return Result.FromError( "Ингредиент не найден" );
            }

            await ingredientRepository.Delete( ingredient );

            return Result.Success;
        }
    }
}