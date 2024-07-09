using Application.CQRSInterfaces;
using Application.Result;
using Application.Validation;
using Recipes.Application.Steps.Dtos;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Entities.Steps;

namespace Recipes.Application.Steps.Queries
{
    public class GetStepsByRecipeIdQueryHandler : IQueryHandler<GetStepsByRecipeIdQueryDto, GetStepsByRecipeIdQueryDto>
    {
        private readonly IStepRepository _stepRepository;
        private readonly IAsyncValidator<GetStepsByRecipeIdQueryDto> _stepQueryValidator;

        public GetStepsByRecipeIdQueryHandler(
            IStepRepository stepRepository,
            IAsyncValidator<GetStepsByRecipeIdQueryDto> validator )
        {
            _stepRepository = stepRepository;
            _stepQueryValidator = validator;
        }

        public async Task<QueryResult<GetStepsByRecipeIdQueryDto>> HandleAsync( GetStepsByRecipeIdQueryDto query )
        {
            ValidationResult validationResult = await _stepQueryValidator.ValidationAsync( query );
            if ( validationResult.IsFail )
            {
                return new QueryResult<GetStepsByRecipeIdQueryDto>( validationResult );
            }

            IEnumerable<Step> steps = await _stepRepository.GetByRecipeIdAsync( query.RecipeId );
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
