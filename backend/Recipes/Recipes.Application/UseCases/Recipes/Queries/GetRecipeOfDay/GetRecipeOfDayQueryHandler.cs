using Mapster;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Recipes.Queries.GetRecipeOfDay;

public class GetRecipeOfDayQueryHandler(
    IRecipeRepository recipeRepository,
    IAsyncValidator<GetRecipeOfDayQuery> validator )
    : QueryBaseHandler<GetRecipeOfDayDto, GetRecipeOfDayQuery>( validator )
{
    protected override async Task<Result<GetRecipeOfDayDto>> HandleImplAsync( GetRecipeOfDayQuery query )
    {
        Recipe foundRecipe = await recipeRepository.GetRecipeOfDayAsync();

        GetRecipeOfDayDto getRecipeByIdQueryDto = foundRecipe.Adapt<GetRecipeOfDayDto>();

        return Result<GetRecipeOfDayDto>.FromSuccess( getRecipeByIdQueryDto );
    }
}