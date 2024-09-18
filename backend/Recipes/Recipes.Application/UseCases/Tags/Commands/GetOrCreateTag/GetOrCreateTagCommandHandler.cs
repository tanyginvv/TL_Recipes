using Microsoft.Extensions.Logging;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Tags.Commands.GetOrCreateTag;

public class GetOrCreateTagCommandHandler(
    ITagRepository tagRepository,
    IAsyncValidator<GetOrCreateTagCommand> validator,
    ILogger<GetOrCreateTagCommand> logger )
    : CommandBaseHandlerWithResult<GetOrCreateTagCommand, Tag>( validator, logger )
{
    protected override async Task<Result<Tag>> HandleImplAsync( GetOrCreateTagCommand command )
    {
        Tag tag = await tagRepository.GetByNameAsync( command.Name );
        if ( tag is null )
        {
            tag = new Tag( command.Name );
            await tagRepository.AddAsync( tag );
        }

        return Result<Tag>.FromSuccess( tag );
    }
}