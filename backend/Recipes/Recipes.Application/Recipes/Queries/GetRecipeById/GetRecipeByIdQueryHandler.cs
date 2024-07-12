using Application.CQRSInterfaces;
using Application.Repositories;
using Application.Result;
using Application.Validation;
using Recipes.Application.Ingredients.Dtos;
using Recipes.Application.Recipes.Dtos;
using Recipes.Application.Steps.Dtos;
using Recipes.Application.Tags.Dtos;
using Recipes.Domain.Entities;

namespace Recipes.Application.Recipes.Queries.GetRecipeById
{
    public class GetRecipeByIdQueryHandler : IQueryHandler<GetRecipeByIdQueryDto, GetRecipeByIdQuery>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IAsyncValidator<GetRecipeByIdQuery> _recipeQueryValidator;

        public GetRecipeByIdQueryHandler( IRecipeRepository recipeRepository, IAsyncValidator<GetRecipeByIdQuery> validator )
        {
            _recipeRepository = recipeRepository;
            _recipeQueryValidator = validator;
        }

        public async Task<QueryResult<GetRecipeByIdQueryDto>> HandleAsync( GetRecipeByIdQuery getRecipeByIdQuery )
        {
            ValidationResult validationResult = await _recipeQueryValidator.ValidationAsync( getRecipeByIdQuery );
            if ( validationResult.IsFail )
            {
                return new QueryResult<GetRecipeByIdQueryDto>( validationResult );
            }

            Recipe foundRecipe = await _recipeRepository.GetByIdAsync( getRecipeByIdQuery.Id );
            if ( foundRecipe == null )
            {
                return new QueryResult<GetRecipeByIdQueryDto>( validationResult );
            }

            GetRecipeByIdQueryDto getRecipeByIdQueryDto = new GetRecipeByIdQueryDto
            {
                Id = foundRecipe.Id,
                Name = foundRecipe.Name,
                Description = foundRecipe.Description,
                CookTime = foundRecipe.CookTime,
                CountPortion = foundRecipe.CountPortion,
                ImageUrl = foundRecipe.ImageUrl,
                Steps = foundRecipe.Steps.Select( step => new StepDto
                {
                    StepNumber = step.StepNumber,
                    StepDescription = step.StepDescription
                } ).ToList(),
                Ingredients = foundRecipe.Ingredients.Select( ingredient => new IngredientDto
                {
                    Title = ingredient.Title,
                    Description = ingredient.Description
                } ).ToList(),
                Tags = foundRecipe.Tags.Select( tag => new TagDto
                {
                    Name = tag.Name
                } ).ToList()
            };

            return new QueryResult<GetRecipeByIdQueryDto>( getRecipeByIdQueryDto );
        }
    }
}
