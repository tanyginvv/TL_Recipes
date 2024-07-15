using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Steps.Dtos;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.Steps.Queries.GetStepsByRecipeIdQuery
{
    public class GetStepsByRecipeIdQueryHandler : IQueryHandler<GetStepsByRecipeIdQueryDto, GetStepsByRecipeIdQuery>
    {
        private readonly IStepRepository _stepRepository;
        private readonly IAsyncValidator<GetStepsByRecipeIdQuery> _stepQueryValidator;

        public GetStepsByRecipeIdQueryHandler(
            IStepRepository stepRepository,
            IAsyncValidator<GetStepsByRecipeIdQuery> validator )
        {
            _stepRepository = stepRepository;
            _stepQueryValidator = validator;
        }

        public async Task<QueryResult<GetStepsByRecipeIdQueryDto>> HandleAsync( GetStepsByRecipeIdQuery query )
        {
            ValidationResult validationResult = await _stepQueryValidator.ValidationAsync( query );
            if ( validationResult.IsFail )
            {
                return new QueryResult<GetStepsByRecipeIdQueryDto>( validationResult );
            }

            IReadOnlyList<Step> steps = await _stepRepository.GetByRecipeIdAsync( query.RecipeId );
            if ( steps == null )
            {
                return new QueryResult<GetStepsByRecipeIdQueryDto>( ValidationResult.Fail( "Steps not found" ) );
            }

            var dto = new GetStepsByRecipeIdQueryDto
            {
                RecipeId = query.RecipeId,
                Steps = new List<Step>( steps )
            };

            return new QueryResult<GetStepsByRecipeIdQueryDto>( dto );
        }
    }
}
