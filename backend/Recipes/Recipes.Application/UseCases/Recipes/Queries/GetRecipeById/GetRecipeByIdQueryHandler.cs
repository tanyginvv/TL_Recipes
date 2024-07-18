using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Ingredients.Dtos;
using Recipes.Application.UseCases.Ingredients.Queries.GetIngredientsByRecipeIdQuery;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.UseCases.Steps.Dtos;
using Recipes.Application.UseCases.Steps.Queries.GetStepsByRecipeIdQuery;
using Recipes.Application.UseCases.Tags.Dtos;
using Recipes.Application.UseCases.Tags.Queries.GetTagsByRecipeIdQuery;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Recipes.Queries.GetRecipeById
{
    public class GetRecipeByIdQueryHandler(
            IRecipeRepository recipeRepository,
            IAsyncValidator<GetRecipeByIdQuery> validator,
            IQueryHandler<GetStepsByRecipeIdQueryDto, GetStepsByRecipeIdQuery> stepsQueryHandler,
            IQueryHandler<GetIngredientsByRecipeIdQueryDto, GetIngredientsByRecipeIdQuery> ingredientsQueryHandler,
            IQueryHandler<GetTagsByRecipeIdQueryDto, GetTagsByRecipeIdQuery> tagsQueryHandler )
        : IQueryHandler<GetRecipeByIdQueryDto, GetRecipeByIdQuery>
    {
        private IRecipeRepository _recipeRepository => recipeRepository;
        private IAsyncValidator<GetRecipeByIdQuery> _recipeQueryValidator => validator;
        private IQueryHandler<GetStepsByRecipeIdQueryDto, GetStepsByRecipeIdQuery> _stepsQueryHandler => stepsQueryHandler;
        private IQueryHandler<GetIngredientsByRecipeIdQueryDto, GetIngredientsByRecipeIdQuery> _ingredientsQueryHandler => ingredientsQueryHandler;
        private IQueryHandler<GetTagsByRecipeIdQueryDto, GetTagsByRecipeIdQuery> _tagsQueryHandler => tagsQueryHandler;

        public async Task<Result<GetRecipeByIdQueryDto>> HandleAsync( GetRecipeByIdQuery query )
        {
            Result validationResult = await _recipeQueryValidator.ValidationAsync( query );
            if ( !validationResult.IsSuccess )
            {
                return Result<GetRecipeByIdQueryDto>.FromError( validationResult );
            }

            Recipe foundRecipe = await _recipeRepository.GetByIdAsync( query.Id );
            if ( foundRecipe is null )
            {
                return Result<GetRecipeByIdQueryDto>.FromError( "Recipe not found" );
            }

            var stepsQuery = new GetStepsByRecipeIdQuery { RecipeId = foundRecipe.Id };
            var stepsResult = await _stepsQueryHandler.HandleAsync( stepsQuery );

            var ingredientsQuery = new GetIngredientsByRecipeIdQuery { RecipeId = foundRecipe.Id };
            var ingredientsResult = await _ingredientsQueryHandler.HandleAsync( ingredientsQuery );

            var tagsQuery = new GetTagsByRecipeIdQuery { RecipeId = foundRecipe.Id };
            var tagsResult = await _tagsQueryHandler.HandleAsync( tagsQuery );

            GetRecipeByIdQueryDto getRecipeByIdQueryDto = new GetRecipeByIdQueryDto
            {
                Id = foundRecipe.Id,
                Name = foundRecipe.Name,
                Description = foundRecipe.Description,
                CookTime = foundRecipe.CookTime,
                CountPortion = foundRecipe.CountPortion,
                ImageUrl = foundRecipe.ImageUrl,

                Steps = stepsResult.Value.Steps.Select( step => new StepDtoUseCases
                {
                    StepNumber = step.StepNumber,
                    StepDescription = step.StepDescription
                } ).ToList(),

                Ingredients = ingredientsResult.Value.Ingredients.Select( ingredient => new IngredientDtoUseCases
                {
                    Title = ingredient.Title,
                    Description = ingredient.Description
                } ).ToList(),

                Tags = tagsResult.Value.Tags.Select( tag => new TagDtoUseCases
                {
                    Name = tag.Name
                } ).ToList()
            };

            return Result<GetRecipeByIdQueryDto>.FromSuccess( getRecipeByIdQueryDto );
        }
    }
}
