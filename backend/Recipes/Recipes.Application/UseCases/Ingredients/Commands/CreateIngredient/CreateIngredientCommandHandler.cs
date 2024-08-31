﻿using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Ingredients.Commands.CreateIngredient;

public class CreateIngredientCommandHandler(
    IIngredientRepository ingredientRepository,
    IAsyncValidator<CreateIngredientCommand> validator )
    : CommandBaseHandlerWithResult<CreateIngredientCommand, Ingredient>( validator )
{
    protected override async Task<Result<Ingredient>> HandleAsyncImpl( CreateIngredientCommand createIngredientCommand )
    {
        Ingredient ingredient = new Ingredient(
            createIngredientCommand.Title,
            createIngredientCommand.Description,
            createIngredientCommand.Recipe.Id );

        await ingredientRepository.AddAsync( ingredient );

        return Result<Ingredient>.FromSuccess( ingredient );
    }
}