using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Ingredients.Commands.UpdateIngredient;

public class UpdateIngredientCommandHandler(
    IIngredientRepository ingredientRepository,
    IAsyncValidator<UpdateIngredientCommand> validator )
    : ICommandHandler<UpdateIngredientCommand>
{
    public async Task<Result> HandleAsync( UpdateIngredientCommand updateIngredientCommand )
    {
        Result validationResult = await validator.ValidateAsync( updateIngredientCommand );

        if ( !validationResult.IsSuccess )
        {
            return Result.FromError( validationResult.Error );
        }

        Ingredient ingredient = await ingredientRepository.GetByIdAsync( updateIngredientCommand.Id );
        if ( ingredient is not null )
        {
            ingredient.Title = updateIngredientCommand.Title;
            ingredient.Description = updateIngredientCommand.Description;
        }
        else
        {
            return Result.FromError( "Такого id ингредиента не существует" );
        }

        return Result.FromSuccess();
    }
}