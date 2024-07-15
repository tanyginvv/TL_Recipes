using Recipes.Application.Repositories;
using Recipes.Application.Validation;

namespace Recipes.Application.Tags.Commands.CreateTag.CreateTagCommand
{
    public class CreateTagCommandValidator : IAsyncValidator<CreateTagCommand>
    {
        private readonly ITagRepository _tagRepository;

        public CreateTagCommandValidator( ITagRepository tagRepository )
        {
            _tagRepository = tagRepository;
        }

        public async Task<ValidationResult> ValidationAsync( CreateTagCommand command )
        {
            if ( string.IsNullOrWhiteSpace( command.Name ) )
            {
                return ValidationResult.Fail( "Name cannot be empty." );
            }

            return ValidationResult.Ok();
        }
    }
}
