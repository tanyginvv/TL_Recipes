﻿using Microsoft.Extensions.Logging;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Ingredients.Commands.UpdateIngredient;

public class UpdateIngredientCommandHandler(
    IIngredientRepository ingredientRepository,
    IAsyncValidator<UpdateIngredientCommand> validator,
    ILogger<UpdateIngredientCommand> logger)
    : CommandBaseHandler<UpdateIngredientCommand>(validator, logger)
{
    protected override async Task<Result> HandleImplAsync( UpdateIngredientCommand command)
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