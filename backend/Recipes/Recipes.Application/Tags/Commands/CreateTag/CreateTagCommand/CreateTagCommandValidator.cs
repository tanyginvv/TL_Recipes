using Application.Validation;
using Recipes.Application.Tags.Dtos;
using Recipes.Infrastructure.Entities.Tags;
using Recipes.Infrastructure.Repositories;
using System.Threading.Tasks;

namespace Recipes.Application.Tags.Commands.CreateTag
{
    public class CreateTagCommandValidator : IAsyncValidator<CreateTagDto>
    {
        private readonly ITagRepository _tagRepository;

        public CreateTagCommandValidator( ITagRepository tagRepository )
        {
            _tagRepository = tagRepository;
        }

        public async Task<ValidationResult> ValidationAsync( CreateTagDto command )
        {
            if ( string.IsNullOrWhiteSpace( command.Name ) )
            {
                return ValidationResult.Fail( "Name cannot be empty." );
            }

            return ValidationResult.Ok();
        }
    }
}
