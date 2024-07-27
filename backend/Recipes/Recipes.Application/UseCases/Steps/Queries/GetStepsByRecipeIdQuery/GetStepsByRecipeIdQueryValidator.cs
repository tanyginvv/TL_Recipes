using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;

namespace Recipes.Application.UseCases.Steps.Queries.GetStepsByRecipeIdQuery
{
    public class GetStepsByRecipeIdQueryValidator( IRecipeRepository recipeRepository )
        : IAsyncValidator<GetStepsByRecipeIdQuery>
    {
        public async Task<Result> ValidateAsync( GetStepsByRecipeIdQuery query )
        {
            if ( query.RecipeId <= 0 )
            {
                return Result.FromError( "Id рецепта должен быть больше нуля" );
            }

            var recipe = await recipeRepository.GetByIdAsync( query.RecipeId );
            if ( recipe is null )
            {
                return Result.FromError( "Рецепта с этим Id не существует" );
            }

            return Result.Success;
        }
    }
}
