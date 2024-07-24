using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Tags.Commands.CreateTag
{
    public class CreateTagCommandValidator
        : IAsyncValidator<CreateTagCommand>
    {
        public async Task<Result> ValidateAsync( CreateTagCommand command )
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
}
