using Microsoft.Extensions.Logging;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Tags.Dtos;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Tags.Queries.GetTagsByRecipeIdQuery;

public class GetTagsByRecipeIdQueryHandler(
    ITagRepository tagRepository,
    IAsyncValidator<GetTagsByRecipeIdQuery> validator,
    ILogger<GetTagsByRecipeIdQuery> logger )
    : QueryBaseHandler<GetTagsByRecipeIdQueryDto, GetTagsByRecipeIdQuery>( validator, logger )
{
    protected override async Task<Result<GetTagsByRecipeIdQueryDto>> HandleImplAsync( GetTagsByRecipeIdQuery query )
    {
        IEnumerable<Tag> tags = await tagRepository.GetByRecipeIdAsync( query.RecipeId );
        if ( tags is null )
        {
            return Result<GetTagsByRecipeIdQueryDto>.FromError( "Теги не найдены" );
        }

        GetTagsByRecipeIdQueryDto dto = new GetTagsByRecipeIdQueryDto
        {
            RecipeId = query.RecipeId,
            Tags = new List<Tag>( tags )
        };

        return Result<GetTagsByRecipeIdQueryDto>.FromSuccess( dto );
    }
}