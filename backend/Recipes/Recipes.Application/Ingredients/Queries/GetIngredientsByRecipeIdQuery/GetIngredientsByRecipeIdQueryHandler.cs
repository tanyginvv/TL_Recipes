using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Ingredients.Dtos;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.Ingredients.Queries.GetIngredientsByRecipeIdQuery
{
    public class GetIngredientsByRecipeIdQueryHandler : IQueryHandler<GetIngredientsByRecipeIdQueryDto, GetIngredientsByRecipeIdQuery>
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IAsyncValidator<GetIngredientsByRecipeIdQuery> _ingredientQueryValidator;

        public GetIngredientsByRecipeIdQueryHandler( IIngredientRepository ingredientRepository, IAsyncValidator<GetIngredientsByRecipeIdQuery> validator )
        {
            _ingredientRepository = ingredientRepository;
            _ingredientQueryValidator = validator;
        }

        public async Task<QueryResult<GetIngredientsByRecipeIdQueryDto>> HandleAsync( GetIngredientsByRecipeIdQuery query )
        {
            ValidationResult validationResult = await _ingredientQueryValidator.ValidationAsync( query );
            if ( validationResult.IsFail )
            {
                return new QueryResult<GetIngredientsByRecipeIdQueryDto>( validationResult );
            }

            IEnumerable<Ingredient> ingredients = await _ingredientRepository.GetByRecipeIdAsync( query.RecipeId );
            if ( ingredients == null )
            {
                return new QueryResult<GetIngredientsByRecipeIdQueryDto>( ValidationResult.Fail( "Ingredients not found" ) );
            }

            GetIngredientsByRecipeIdQueryDto dto = new GetIngredientsByRecipeIdQueryDto
            {
                RecipeId = query.RecipeId,
                Ingredients = new List<Ingredient>( ingredients )
            };

            return new QueryResult<GetIngredientsByRecipeIdQueryDto>( dto );
        }
    }
}
