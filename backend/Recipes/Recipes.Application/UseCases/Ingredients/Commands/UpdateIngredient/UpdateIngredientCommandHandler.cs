using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Ingredients.Commands.UpdateIngredient;

public class UpdateIngredientCommandHandler(
    IIngredientRepository ingredientRepository,
    IAsyncValidator<UpdateIngredientCommand> validator)
    : CommandBaseHandler<UpdateIngredientCommand>(validator)
{
    protected override async Task<Result> HandleAsyncImpl(UpdateIngredientCommand command)
    {
        Ingredient ingredient = await ingredientRepository.GetByIdAsync(command.Id);
        if (ingredient is null)
        {
            return Result.FromError("Такого id ингредиента не существует");
        }

        ingredient.Title = command.Title;
        ingredient.Description = command.Description;

        return Result.Success;
    }
}