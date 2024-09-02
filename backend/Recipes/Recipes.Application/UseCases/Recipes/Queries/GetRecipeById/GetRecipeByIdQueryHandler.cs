using Mapster;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Recipes.Queries.GetRecipeById;

public class GetRecipeByIdQueryHandler(
    IRecipeRepository recipeRepository,
    IAsyncValidator<GetRecipeByIdQuery> validator )
    : QueryBaseHandler<GetRecipeQueryDto, GetRecipeByIdQuery>( validator )
{
    protected override async Task<Result<GetRecipeQueryDto>> HandleAsyncImpl( GetRecipeByIdQuery query )
    {
        Recipe foundRecipe = await recipeRepository.GetByIdAsync( query.Id );
        if ( foundRecipe is null )
        {
            return Result<GetRecipeQueryDto>.FromError( "Recipe not found" );
        }

        GetRecipeQueryDto getRecipeByIdQueryDto = foundRecipe.Adapt<GetRecipeQueryDto>();

        return Result<GetRecipeQueryDto>.FromSuccess( getRecipeByIdQueryDto );
    }
}