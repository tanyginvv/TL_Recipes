using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Domain.Entities;
using Recipes.Application.CQRSInterfaces;

namespace Recipes.Application.UseCases.Ingredients.Queries.GetIngredientsByRecipeIdQuery;

public class GetIngredientsByRecipeIdQueryValidator( IRecipeRepository recipeRepository )
    : IAsyncValidator<GetIngredientsByRecipeIdQuery>
{
    public async Task<Result> ValidateAsync( GetIngredientsByRecipeIdQuery query )
    {
        if ( query.RecipeId <= 0 )
        {
            return Result.FromError( "Id рецепта должен быть больше нуля" );
        }

        Recipe recipe = await recipeRepository.GetByIdAsync( query.RecipeId );
        if ( recipe is null )
        {
            return Result.FromError( "Рецепта с этим Id не существует" );
        }

        return Result.Success;
    }
}