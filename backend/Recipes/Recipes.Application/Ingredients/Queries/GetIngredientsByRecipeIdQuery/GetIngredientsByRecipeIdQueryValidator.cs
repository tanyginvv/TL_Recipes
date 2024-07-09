using Application.Repositories;
using Application.Validation;
using Recipes.Application.Ingredients.Dtos;
using System.Threading.Tasks;

namespace Recipes.Application.Ingredients.Queries.GetIngredientsByRecipeId
{
    public class GetIngredientsByRecipeIdQueryValidator : IAsyncValidator<GetIngredientsByRecipeIdQueryDto>
    {
        private readonly IRecipeRepository _recipeRepository;

        public GetIngredientsByRecipeIdQueryValidator( IRecipeRepository recipeRepository )
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<ValidationResult> ValidationAsync( GetIngredientsByRecipeIdQueryDto query )
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
