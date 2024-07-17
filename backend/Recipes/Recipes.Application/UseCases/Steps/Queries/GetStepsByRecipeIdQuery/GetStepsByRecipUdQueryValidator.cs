using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Steps.Queries.GetStepsByRecipeIdQuery
{
    public class GetStepsByRecipeIdQueryValidator( IRecipeRepository recipeRepository )
        : IAsyncValidator<GetStepsByRecipeIdQuery>
    {
        private IRecipeRepository _recipeRepository => recipeRepository;

        public async Task<Result> ValidationAsync( GetStepsByRecipeIdQuery query )
        {
            if ( query.RecipeId <= 0 )
            {
                return Result.FromError( "Id рецепта должен быть больше нуля" );
            }

            var recipe = await _recipeRepository.GetByIdAsync( query.RecipeId );
            if ( recipe == null )
            {
                return Result.FromError( "Рецепта с этим Id не существует" );
            }

            return Result.Success;
        }
    }
}
