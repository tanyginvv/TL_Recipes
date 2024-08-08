using Recipes.Application.CQRSInterfaces;
using Recipes.Domain.Entities;
using Recipes.Application.Validation;
using Recipes.Application.Results;
using Recipes.Application.Repositories;
using Mapster;

namespace Recipes.Application.UseCases.Tags.Commands.GetOrCreateTag
{
    public class GetOrCreateTagCommandHandler(
        ITagRepository tagRepository,
        IAsyncValidator<GetOrCreateTagCommand> validator )
        : ICommandHandlerWithResult<GetOrCreateTagCommand, Tag>
    {
        public async Task<Result<Tag>> HandleAsync( GetOrCreateTagCommand createTagCommand )
        {
            Result validationResult = await validator.ValidateAsync( createTagCommand );
            if ( !validationResult.IsSuccess )
            {
                return Result<Tag>.FromError( validationResult.Error );
            }

            Tag tag = await tagRepository.GetByNameAsync( createTagCommand.Name );
            if ( tag is null )
            {
                tag = createTagCommand.Adapt<Tag>();
                await tagRepository.AddAsync( tag );
            }

            return Result<Tag>.FromSuccess( tag );
        }
    }
}