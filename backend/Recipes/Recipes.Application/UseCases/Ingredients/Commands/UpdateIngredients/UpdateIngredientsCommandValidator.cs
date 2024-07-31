using Recipes.Application.Results;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Ingredients.Commands
{
    public class UpdateIngredientsCommandValidator : IAsyncValidator<UpdateIngredientsCommand>
    {
        public async Task<Result> ValidateAsync( UpdateIngredientsCommand command )
        {
            if ( command.Recipe is null )
            {
                return Result.FromError( "Рецепт не может быть null." );
            }

            foreach ( IngredientDto ingredient in command.NewIngredients )
            {
                if ( string.IsNullOrEmpty( ingredient.Title ) )
                {
                    return Result.FromError( "Название ингредиента не может быть пустым." );
                }

                if ( ingredient.Title.Length > 100 )
                {
                    return Result.FromError( "Название ингредиента не может быть больше чем 100 символов." );
                }

                if ( string.IsNullOrEmpty( ingredient.Description ) )
                {
                    return Result.FromError( "Описание ингредиента не может быть пустым." );
                }

                if ( ingredient.Description.Length > 250 )
                {
                    return Result.FromError( "Описание ингредиента не может быть больше чем 250 символов." );
                }
            }

            return Result.Success;
        }
    }
}
