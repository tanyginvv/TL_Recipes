﻿using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Tags.Commands.GetOrCreateTag;

public class GetOrCreateTagCommandHandler(
    ITagRepository tagRepository,
    IAsyncValidator<GetOrCreateTagCommand> validator )
    : CommandBaseHandler<GetOrCreateTagCommand, Tag>( validator )
{
    protected override async Task<Result<Tag>> HandleAsyncImpl( GetOrCreateTagCommand command )
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