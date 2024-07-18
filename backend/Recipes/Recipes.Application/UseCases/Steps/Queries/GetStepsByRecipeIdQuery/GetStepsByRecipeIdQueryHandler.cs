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
        private IStepRepository _stepRepository => stepRepository;
        private IAsyncValidator<GetStepsByRecipeIdQuery> _stepQueryValidator => validator;


        public async Task<Result<GetStepsByRecipeIdQueryDto>> HandleAsync( GetStepsByRecipeIdQuery query )
        {
            Result validationResult = await _stepQueryValidator.ValidationAsync( query );
            if ( !validationResult.IsSuccess )
            {
                return Result<GetStepsByRecipeIdQueryDto>.FromError( validationResult );
            }

            IReadOnlyList<Step> steps = await _stepRepository.GetByRecipeIdAsync( query.RecipeId );
            if ( steps is null )
            {
                return Result<GetStepsByRecipeIdQueryDto>.FromError( "Steps not found" );
            }

            var dto = new GetStepsByRecipeIdQueryDto
            {
                RecipeId = query.RecipeId,
                Steps = new List<Step>( steps )
            };

            return Result<GetStepsByRecipeIdQueryDto>.FromSuccess( dto );
        }
    }
}
