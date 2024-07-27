using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Ingredients.Dtos;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.UseCases.Steps.Dtos;
using Recipes.Application.UseCases.Tags.Dtos;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Recipes.Queries.GetRecipeById
{
    public class GetRecipeByIdQueryHandler(
            IRecipeRepository recipeRepository,
            IAsyncValidator<GetRecipeByIdQuery> validator )
        : IQueryHandler<GetRecipeByIdQueryDto, GetRecipeByIdQuery>
    {
        public async Task<Result<GetRecipeByIdQueryDto>> HandleAsync( GetRecipeByIdQuery query )
        {
            Result validationResult = await validator.ValidateAsync( query );
            if ( !validationResult.IsSuccess )
            {
                return Result<GetRecipeByIdQueryDto>.FromError( validationResult );
            }

            Recipe foundRecipe = await recipeRepository.GetByIdAsync( query.Id );

            GetRecipeByIdQueryDto getRecipeByIdQueryDto = new GetRecipeByIdQueryDto
            {
                Id = foundRecipe.Id,
                Name = foundRecipe.Name,
                Description = foundRecipe.Description,
                CookTime = foundRecipe.CookTime,
                PortionCount = foundRecipe.PortionCount,
                ImageUrl = foundRecipe.ImageUrl,

                Steps = foundRecipe.Steps.Select( step => new StepDtoUseCases
                {
                    StepNumber = step.StepNumber,
                    StepDescription = step.StepDescription
                } ).ToList(),

                Ingredients = foundRecipe.Ingredients.Select( ingredient => new IngredientDtoUseCases
                {
                    Title = ingredient.Title,
                    Description = ingredient.Description
                } ).ToList(),

                Tags = foundRecipe.Tags.Select( tag => new TagDtoUseCases
                {
                    Name = tag.Name
                } ).ToList()
            };

            return Result<GetRecipeByIdQueryDto>.FromSuccess( getRecipeByIdQueryDto );
        }
    }
}
