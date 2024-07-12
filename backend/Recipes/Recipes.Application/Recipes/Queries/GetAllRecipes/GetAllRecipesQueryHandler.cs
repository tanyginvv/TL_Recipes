using Application.CQRSInterfaces;
using Application.Repositories;
using Application.Result;
using Application.Validation;
using Recipes.Application.Ingredients.Dtos;
using Recipes.Application.Recipes.Dtos;
using Recipes.Application.Steps.Dtos;
using Recipes.Application.Tags.Dtos;
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
                ImageUrl = recipe.ImageUrl,

                Steps = recipe.Steps.Select( step => new StepDto
                {
                    StepNumber = step.StepNumber,
                    StepDescription = step.StepDescription
                } ).ToList(),

                Ingredients = recipe.Ingredients.Select( ingredient => new IngredientDto
                {
                    Title = ingredient.Title,
                    Description = ingredient.Description
                } ).ToList(),

                Tags = recipe.Tags.Select( tag => new TagDto
                {
                    Name = tag.Name
                } ).ToList()
            } );

            return new QueryResult<IEnumerable<RecipeDto>>( recipeDtos );
        }
    }
}
