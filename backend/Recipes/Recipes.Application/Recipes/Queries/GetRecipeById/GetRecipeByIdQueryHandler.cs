using Application.CQRSInterfaces;
using Application.Repositories;
using Application.Result;
using Application.Validation;
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
                Steps = ( List<Step> )foundRecipe.Steps,
                Ingredients = ( List<Ingredient> )foundRecipe.Ingredients,
                Tags = ( List<Tag> )foundRecipe.Tags
            };

            return new QueryResult<GetRecipeByIdQueryDto>( getRecipeByIdQueryDto );
        }
    }
}
