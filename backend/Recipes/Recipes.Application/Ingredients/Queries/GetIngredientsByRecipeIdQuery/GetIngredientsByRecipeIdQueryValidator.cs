using Recipes.Application.Repositories;
using Recipes.Application.Validation;

namespace Recipes.Application.Ingredients.Queries.GetIngredientsByRecipeIdQuery
{
    public class GetIngredientsByRecipeIdQueryValidator : IAsyncValidator<GetIngredientsByRecipeIdQuery>
    {
        private readonly IRecipeRepository _recipeRepository;

        public GetIngredientsByRecipeIdQueryValidator( IRecipeRepository recipeRepository )
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<ValidationResult> ValidationAsync( GetIngredientsByRecipeIdQuery query )
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
