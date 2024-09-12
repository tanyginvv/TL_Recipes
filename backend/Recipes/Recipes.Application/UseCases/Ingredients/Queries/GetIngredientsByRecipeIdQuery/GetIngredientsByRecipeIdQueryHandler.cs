using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Ingredients.Dtos;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Ingredients.Queries.GetIngredientsByRecipeIdQuery;

public class GetIngredientsByRecipeIdQueryHandler(
    IIngredientRepository ingredientRepository,
    IAsyncValidator<GetIngredientsByRecipeIdQuery> validator )
    : QueryBaseHandler<GetIngredientsByRecipeIdQueryDto, GetIngredientsByRecipeIdQuery>( validator )
{
    protected override async Task<Result<GetIngredientsByRecipeIdQueryDto>> HandleImplAsync( GetIngredientsByRecipeIdQuery query )
    {
        IEnumerable<Ingredient> ingredients = await ingredientRepository.GetByRecipeIdAsync( query.RecipeId );
        if ( ingredients is null || !ingredients.Any() )
        {
            return Result<GetIngredientsByRecipeIdQueryDto>.FromError( "Ингредиенты не найдены" );
        }

        GetIngredientsByRecipeIdQueryDto dto = new GetIngredientsByRecipeIdQueryDto
        {
            RecipeId = query.RecipeId,
            Ingredients = new List<Ingredient>( ingredients )
        };

        return Result<GetIngredientsByRecipeIdQueryDto>.FromSuccess( dto );
    }
}