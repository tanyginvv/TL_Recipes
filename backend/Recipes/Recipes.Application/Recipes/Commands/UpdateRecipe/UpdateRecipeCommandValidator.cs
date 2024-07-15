using Application.Validation;
using Recipes.Infrastructure.Repositories;

namespace Recipes.Application.Recipes.Commands.UpdateRecipe
{
    public class UpdateRecipeCommandValidator : IAsyncValidator<UpdateRecipeCommand>
    {
        private readonly IRecipeRepository _recipeRepository;

        public UpdateRecipeCommandValidator( IRecipeRepository recipeRepository )
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<ValidationResult> ValidationAsync( UpdateRecipeCommand command )
        {
            if ( command.Name == null || command.Name == String.Empty )
            {
                return ValidationResult.Fail( "Название блюда не может быть пустым" );
            }

            if ( command.Name.Length > 100 )
            {
                return ValidationResult.Fail( "Название блюда не может быть больше чем 100 символов" );
            }

            if ( command.Description == null || command.Description == String.Empty )
            {
                return ValidationResult.Fail( "Описание блюда не может быть пустым" );
            }

            if ( command.Description.Length > 500 )
            {
                return ValidationResult.Fail( "Описание блюда не может быть больше чем 500 символов" );
            }

            if ( command.CountPortion == 0 || command.CountPortion < 0 )
            {
                return ValidationResult.Fail( "Количество порций должно быть больше 0" );
            }

            if ( command.CookTime == 0 || command.CookTime < 0 )
            {
                return ValidationResult.Fail( "Время приготовления должно быть больше 0" );
            }

            if ( command.ImageUrl == null || command.ImageUrl == String.Empty )
            {
                return ValidationResult.Fail( "Изображение блюда должно быть обязательно " );
            }

            return ValidationResult.Ok();
        }
    }
}
