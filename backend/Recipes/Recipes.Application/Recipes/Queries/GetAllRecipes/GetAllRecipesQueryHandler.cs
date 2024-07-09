using Application.CQRSInterfaces;
using Application.Repositories;
using Application.Result;
using Application.Validation;
using Recipes.Application.Recipes.Dtos;
using Recipes.Domain.Entities;

namespace Recipes.Application.Recipes.Queries.GetAllRecipes
{
    public class GetAllRecipesQueryHandler : IQueryHandler<IEnumerable<RecipeDto>, GetAllRecipesQuery>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IAsyncValidator<GetAllRecipesQuery> _recipeQueryValidator;

        public GetAllRecipesQueryHandler( IRecipeRepository recipeRepository, IAsyncValidator<GetAllRecipesQuery> validator )
        {
            _recipeRepository = recipeRepository;
            _recipeQueryValidator = validator;
        }

        public async Task<QueryResult<IEnumerable<RecipeDto>>> HandleAsync( GetAllRecipesQuery query )
        {
            ValidationResult validationResult = await _recipeQueryValidator.ValidationAsync( query );
            if ( validationResult.IsFail )
            {
                return new QueryResult<IEnumerable<RecipeDto>>( validationResult );
            }

            IEnumerable<Recipe> recipes = await _recipeRepository.GetAllAsync();

            var recipeDtos = recipes.Select( recipe => new RecipeDto
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                CookTime = recipe.CookTime,
                CountPortion = recipe.CountPortion,
                ImageUrl = recipe.ImageUrl
            } );

            return new QueryResult<IEnumerable<RecipeDto>>( recipeDtos );
        }
    }
}
