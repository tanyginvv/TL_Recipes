using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Tags.Commands.GetOrCreateTag;

public class GetOrCreateTagCommandValidator
    : IAsyncValidator<GetOrCreateTagCommand>
{
    public async Task<Result> ValidateAsync( GetOrCreateTagCommand command )
    {
        if ( string.IsNullOrWhiteSpace( command.Name ) )
        {
            return Result.FromError( "Название тега не может быть пустым" );
        }

        if ( command.Name.Length > 50 )
        {
            return Result.FromError( "Название тега не может быть больше 50 символов" );
        }

        return Result.FromSuccess();
    }
}