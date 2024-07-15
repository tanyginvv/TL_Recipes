using Recipes.Application.Repositories;
using Recipes.Application.Validation;

namespace Recipes.Application.Ingredients.Commands.CreateIngredient
{
    public class CreateIngredientCommandValidator : IAsyncValidator<CreateIngredientCommand>
    {
        private readonly IIngredientRepository _ingredientRepository;

        public CreateIngredientCommandValidator( IIngredientRepository ingredientRepository )
        {
            _ingredientRepository = ingredientRepository;
        }

        public async Task<ValidationResult> ValidationAsync( CreateIngredientCommand command )
        {
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