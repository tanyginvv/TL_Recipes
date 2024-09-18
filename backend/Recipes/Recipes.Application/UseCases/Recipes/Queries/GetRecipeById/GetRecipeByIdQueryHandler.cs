using Mapster;
using Microsoft.Extensions.Logging;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Recipes.Queries.GetRecipeById;

public class GetRecipeByIdQueryHandler(
    IRecipeRepository recipeRepository,
    ILikeRepository likeRepository,
    IFavouriteRepository favouriteRepository,
    IAsyncValidator<GetRecipeByIdQuery> validator,
    ILogger<GetRecipeByIdQuery> logger )
    : QueryBaseHandler<GetRecipeQueryDto, GetRecipeByIdQuery>( validator, logger )
{
    protected override async Task<Result<GetRecipeQueryDto>> HandleImplAsync( GetRecipeByIdQuery query )
    {
        Recipe foundRecipe = await recipeRepository.GetByIdAsync( query.Id );

        GetRecipeQueryDto getRecipeByIdQueryDto = foundRecipe.Adapt<GetRecipeQueryDto>();

        if ( query.UserId != 0 )
        {
            getRecipeByIdQueryDto.IsLiked = await likeRepository.GetLikeByAttributes( getRecipeByIdQueryDto.Id, query.UserId ) is not null;
            getRecipeByIdQueryDto.IsFavourited = await favouriteRepository.GetFavouriteByAttributes( getRecipeByIdQueryDto.Id, query.UserId ) is not null;
        }

        return Result<GetRecipeQueryDto>.FromSuccess( getRecipeByIdQueryDto );
    }
}