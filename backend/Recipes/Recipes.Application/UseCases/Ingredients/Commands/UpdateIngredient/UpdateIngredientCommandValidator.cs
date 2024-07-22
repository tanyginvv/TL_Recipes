﻿using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Ingredients.Commands.UpdateIngredient
{
    public class UpdateIngredientCommandValidator()
        : IAsyncValidator<UpdateIngredientCommand>
    {
        public async Task<Result> ValidateAsync( UpdateIngredientCommand command )
        {
            if ( command.Id <= 0 )
            {
                return Result.FromError( "ID ингредиента должен быть больше нуля" );
            }

            if ( string.IsNullOrWhiteSpace( command.Title ) )
            {
                return Result.FromError( "Название ингредиента не может быть пустым" );
            }

            if ( command.Title.Length > 100 )
            {
                return Result.FromError( "Название ингредиента не может быть больше чем 100 символов" );
            }

            if ( string.IsNullOrWhiteSpace( command.Description ) )
            {
                return Result.FromError( "Описание ингредиента не может быть пустым" );
            }

            if ( command.Description.Length > 250 )
            {
                return Result.FromError( "Описание ингредиента не может быть больше чем 250 символов" );
            }

            return Result.Success;
        }
    }
}