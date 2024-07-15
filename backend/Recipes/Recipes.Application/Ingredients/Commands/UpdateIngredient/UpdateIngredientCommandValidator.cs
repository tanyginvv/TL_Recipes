using Recipes.Application.Repositories;
using Recipes.Application.Validation;

namespace Recipes.Application.Ingredients.Commands.UpdateIngredient
{
    public class UpdateIngredientCommandValidator : IAsyncValidator<UpdateIngredientCommand>
    {
        private readonly IIngredientRepository _ingredientRepository;

        public UpdateIngredientCommandValidator( IIngredientRepository ingredientRepository )
        {
            _ingredientRepository = ingredientRepository;
        }

        public async Task<ValidationResult> ValidationAsync( UpdateIngredientCommand command )
        {
            if ( command.Id <= 0 )
            {
                return ValidationResult.Fail( "ID ингредиента должен быть больше нуля" );
            }

            if ( string.IsNullOrWhiteSpace( command.Title ) )
            {
                return ValidationResult.Fail( "Название ингредиента не может быть пустым" );
            }

            if ( command.Title.Length > 100 )
            {
                return ValidationResult.Fail( "Название ингредиента не может быть больше чем 100 символов" );
            }

            if ( string.IsNullOrWhiteSpace( command.Description ) )
            {
                return ValidationResult.Fail( "Описание ингредиента не может быть пустым" );
            }

            if ( command.Description.Length > 250 )
            {
                return ValidationResult.Fail( "Описание ингредиента не может быть больше чем 250 символов" );
            }

            return ValidationResult.Ok();
        }
    }
}