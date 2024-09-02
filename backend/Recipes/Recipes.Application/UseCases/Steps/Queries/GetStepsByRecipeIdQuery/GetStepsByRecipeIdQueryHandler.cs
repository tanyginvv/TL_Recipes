using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Steps.Dtos;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Steps.Queries.GetStepsByRecipeIdQuery;

public class GetStepsByRecipeIdQueryHandler(
    IStepRepository stepRepository,
    IAsyncValidator<GetStepsByRecipeIdQuery> validator )
    : QueryBaseHandler<GetStepsByRecipeIdQueryDto, GetStepsByRecipeIdQuery>( validator )
{
    protected override async Task<Result<GetStepsByRecipeIdQueryDto>> HandleAsyncImpl( GetStepsByRecipeIdQuery query )
    {
        IReadOnlyList<Step> steps = await stepRepository.GetByRecipeIdAsync( query.RecipeId );
        if ( steps is null || !steps.Any() )
        {
            return Result<GetStepsByRecipeIdQueryDto>.FromError( "Шаги не найдены" );
        }

        GetStepsByRecipeIdQueryDto dto = new GetStepsByRecipeIdQueryDto
        {
            RecipeId = query.RecipeId,
            Steps = steps.ToList()
        };

        return Result<GetStepsByRecipeIdQueryDto>.FromSuccess( dto );
    }
}