using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Recipes.Commands.CreateRecipe
{
    public class CreateRecipeCommandValidator( IRecipeRepository recipeRepository )
        : IAsyncValidator<CreateRecipeCommand>
    {
        private IRecipeRepository _recipeRepository => recipeRepository;

        public async Task<Result> ValidationAsync( CreateRecipeCommand command )
        {
            if ( command.Name == null || command.Name == string.Empty )
            {
                return Result.FromError( "Название блюда не может быть пустым" );
            }

            if ( command.Name.Length > 100 )
            {
                return Result.FromError( "Название блюда не может быть больше чем 100 символов" );
            }

            if ( command.Description == null || command.Description == string.Empty )
            {
                return Result.FromError( "Описание блюда не может быть пустым" );
            }

            if ( command.Description.Length > 150 )
            {
                return Result.FromError( "Описание блюда не может быть больше чем 150 символов" );
            }

            if ( command.CountPortion == 0 || command.CountPortion < 0 )
            {
                return Result.FromError( "Количество порций должно быть больше 0" );
            }

            if ( command.CookTime == 0 || command.CookTime < 0 )
            {
                return Result.FromError( "Время приготовления должно быть больше 0" );
            }

            if ( command.ImageUrl == null || command.ImageUrl == string.Empty )
            {
                return Result.FromError( "Изображение блюда должно быть обязательно " );
            }

            return Result.Success;
        }
    }
}