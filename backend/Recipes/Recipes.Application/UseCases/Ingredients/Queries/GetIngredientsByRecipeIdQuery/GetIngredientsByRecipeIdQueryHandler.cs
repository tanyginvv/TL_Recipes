using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Ingredients.Dtos;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Ingredients.Queries.GetIngredientsByRecipeIdQuery;

public class GetIngredientsByRecipeIdQueryHandler(
    IIngredientRepository ingredientRepository,
    IAsyncValidator<GetIngredientsByRecipeIdQuery> validator )
    : IQueryHandler<GetIngredientsByRecipeIdQueryDto, GetIngredientsByRecipeIdQuery>
{
    public async Task<Result<GetIngredientsByRecipeIdQueryDto>> HandleAsync( GetIngredientsByRecipeIdQuery query )
    {
        Result validationResult = await validator.ValidateAsync( query );
        if ( !validationResult.IsSuccess )
        {
            return Result<GetIngredientsByRecipeIdQueryDto>.FromError( validationResult );
        }

        IEnumerable<Ingredient> ingredients = await ingredientRepository.GetByRecipeIdAsync( query.RecipeId );
        if ( ingredients is null )
        {
            return Result<GetIngredientsByRecipeIdQueryDto>.FromError( "Ингредиент не найден" );
        }

        GetIngredientsByRecipeIdQueryDto dto = new GetIngredientsByRecipeIdQueryDto
        {
            RecipeId = query.RecipeId,
            Ingredients = new List<Ingredient>( ingredients )
        };

        return Result<GetIngredientsByRecipeIdQueryDto>.FromSuccess( dto );
    }
}