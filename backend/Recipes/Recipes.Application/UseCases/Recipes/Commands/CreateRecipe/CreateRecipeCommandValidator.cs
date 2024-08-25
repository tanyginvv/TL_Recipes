using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Recipes.Commands.CreateRecipe;

public class CreateRecipeCommandValidator(
    IUserRepository userRepository )
    : IAsyncValidator<CreateRecipeCommand>
{
    public async Task<Result> ValidateAsync( CreateRecipeCommand command )
    {
        User author = await userRepository.GetByIdAsync( command.AuthorId );
        if ( author is null )
        {
            return Result.FromError( "Такого пользователя не существует" );
        }

        if ( string.IsNullOrEmpty( command.Name ) )
        {
            return Result.FromError( "Название блюда не может быть пустым" );
        }

        if ( command.Name.Length > 100 )
        {
            return Result.FromError( "Название блюда не может быть больше чем 100 символов" );
        }

        if ( string.IsNullOrEmpty( command.Description ) )
        {
            return Result.FromError( "Описание блюда не может быть пустым" );
        }

        if ( command.Description.Length > 150 )
        {
            return Result.FromError( "Описание блюда не может быть больше чем 150 символов" );
        }

        if ( command.PortionCount <= 0 )
        {
            return Result.FromError( "Количество порций должно быть больше 0" );
        }

        if ( command.CookTime <= 0 )
        {
            return Result.FromError( "Время приготовления должно быть больше 0" );
        }

        if ( string.IsNullOrEmpty( command.ImageUrl ) )
        {
            return Result.FromError( "Изображение блюда должно быть обязательно " );
        }

        if ( command.Tags.Count > 5 )
        {
            return Result.FromError( "Количество тегов ограничено до 5 " );
        }

        return Result.Success;
    }
}