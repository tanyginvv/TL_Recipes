﻿using Mapster;
using Microsoft.Extensions.Logging;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Tags.Queries.GetTagsForSearch;

public class GetTagsForSearchQueryHandler( 
    ITagRepository tagRepository,
    IAsyncValidator<GetTagsForSearchQuery> validator,
    ILogger<GetTagsForSearchQuery> logger )
     : QueryBaseHandler<IReadOnlyList<TagDto>, GetTagsForSearchQuery>( validator, logger )
{
    protected override async Task<Result<IReadOnlyList<TagDto>>> HandleImplAsync( GetTagsForSearchQuery query )
    {
        IReadOnlyList<Tag> tags = await tagRepository.GetTagsForSearchAsync( query.Count );

        if ( tags is null || !tags.Any() )
        {
            return Result<IReadOnlyList<TagDto>>.FromError( "Теги не найдены" );
        }

        IReadOnlyList<TagDto> tagDtos = tags.Adapt<IReadOnlyList<TagDto>>();

        return Result<IReadOnlyList<TagDto>>.FromSuccess( tagDtos );
    }
}