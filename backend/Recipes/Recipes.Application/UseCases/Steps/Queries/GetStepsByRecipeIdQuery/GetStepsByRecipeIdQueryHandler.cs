using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Steps.Dtos;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Steps.Queries.GetStepsByRecipeIdQuery
{
    public class GetStepsByRecipeIdQueryHandler(
            IStepRepository stepRepository,
            IAsyncValidator<GetStepsByRecipeIdQuery> validator )
        : IQueryHandler<GetStepsByRecipeIdQueryDto, GetStepsByRecipeIdQuery>
    {
        public async Task<Result<GetStepsByRecipeIdQueryDto>> HandleAsync( GetStepsByRecipeIdQuery query )
        {
            Result validationResult = await validator.ValidateAsync( query );
            if ( !validationResult.IsSuccess )
            {
                return Result<GetStepsByRecipeIdQueryDto>.FromError( validationResult );
            }

            IReadOnlyList<Step> steps = await stepRepository.GetByRecipeIdAsync( query.RecipeId );
            if ( steps is null )
            {
                return Result<GetStepsByRecipeIdQueryDto>.FromError( "Шаг не найден" );
            }

            GetStepsByRecipeIdQueryDto dto = new GetStepsByRecipeIdQueryDto
            {
                RecipeId = query.RecipeId,
                Steps = new List<Step>( steps )
            };

            return Result<GetStepsByRecipeIdQueryDto>.FromSuccess( dto );
        }
    }
}
