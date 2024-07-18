using Recipes.Application.Repositories;
using Recipes.Application.Validation;
using Recipes.Application.Results;

namespace Recipes.Application.UseCases.Ingredients.Queries.GetIngredientsByRecipeIdQuery
{
    public class GetIngredientsByRecipeIdQueryValidator( IRecipeRepository recipeRepository ) : IAsyncValidator<GetIngredientsByRecipeIdQuery>
    {
        private IRecipeRepository _recipeRepository => recipeRepository;

        public async Task<Result> ValidationAsync( GetIngredientsByRecipeIdQuery query )
        {
            if ( query.RecipeId <= 0 )
            {
                return Result.FromError( "Id рецепта должен быть больше нуля" );
            }

            var recipe = await _recipeRepository.GetByIdAsync( query.RecipeId );
            if ( recipe is null )
            {
                return Result.FromError( "Рецепта с этим Id не существует" );
            }

            return Result.Success;
        }
    }
}
