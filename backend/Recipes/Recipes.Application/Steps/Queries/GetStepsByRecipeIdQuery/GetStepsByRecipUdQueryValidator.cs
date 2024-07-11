using Application.Repositories;
using Application.Validation;
using Recipes.Application.Steps.Dtos;

namespace Recipes.Application.Steps.Queries
{
    public class GetStepsByRecipeIdQueryValidator : IAsyncValidator<GetStepsByRecipeIdQuery>
    {
        private readonly IRecipeRepository _recipeRepository;

        public GetStepsByRecipeIdQueryValidator( IRecipeRepository recipeRepository )
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<ValidationResult> ValidationAsync( GetStepsByRecipeIdQuery query )
        {
            if ( query.RecipeId <= 0 )
            {
                return ValidationResult.Fail( "Id рецепта должен быть больше нуля" );
            }

            var recipe = await _recipeRepository.GetByIdAsync( query.RecipeId );
            if ( recipe == null )
            {
                return ValidationResult.Fail( "Рецепта с этим Id не существует" );
            }

            return ValidationResult.Ok();
        }
    }
}
