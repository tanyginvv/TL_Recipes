using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Tags.Commands.CreateTag.CreateTagCommand
{
    public class CreateTagCommandValidator : IAsyncValidator<CreateTagCommand>
    {
        private readonly ITagRepository _tagRepository;

        public CreateTagCommandValidator( ITagRepository tagRepository )
        {
            _tagRepository = tagRepository;
        }

        public async Task<Result> ValidationAsync( CreateTagCommand command )
        {
            if ( string.IsNullOrWhiteSpace( command.Name ) )
            {
                return Result.FromError( "Name cannot be empty." );
            }

            return Result.FromSuccess();
        }
    }
}
