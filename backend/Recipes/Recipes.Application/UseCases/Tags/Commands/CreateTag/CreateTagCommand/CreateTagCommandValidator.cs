using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Tags.Commands.CreateTag.CreateTagCommand
{
    public class CreateTagCommandValidator
        : IAsyncValidator<CreateTagCommand>
    {
        public async Task<Result> ValidateAsync( CreateTagCommand command )
        {
            if ( string.IsNullOrWhiteSpace( command.Name ) )
            {
                return Result.FromError( "Name cannot be empty." );
            }

            return Result.FromSuccess();
        }
    }
}
