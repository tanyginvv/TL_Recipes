using Application.CQRSInterfaces;
using Application.Result;
using Application.Validation;
using Recipes.Application.Ingredients.Dtos;
using Recipes.Application.Ingredients.Queries;
using Recipes.Application.Recipes.Dtos;
using Recipes.Application.Steps.Dtos;
using Recipes.Application.Steps.Queries;
using Recipes.Application.Tags.Dtos;
using Recipes.Application.Tags.Queries.GetTagsByRecipeIdQuery;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Repositories;

namespace Recipes.Application.Recipes.Queries.GetRecipeById
{
    public class GetRecipeByIdQueryHandler : IQueryHandler<GetRecipeByIdQueryDto, GetRecipeByIdQuery>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IAsyncValidator<GetRecipeByIdQuery> _recipeQueryValidator;
        private readonly IQueryHandler<GetStepsByRecipeIdQueryDto, GetStepsByRecipeIdQuery> _stepsQueryHandler;
        private readonly IQueryHandler<GetIngredientsByRecipeIdQueryDto, GetIngredientsByRecipeIdQuery> _ingredientsQueryHandler;
        private readonly IQueryHandler<GetTagsByRecipeIdQueryDto, GetTagsByRecipeIdQuery> _tagsQueryHandler;

        public GetRecipeByIdQueryHandler(
            IRecipeRepository recipeRepository,
            IAsyncValidator<GetRecipeByIdQuery> validator,
            IQueryHandler<GetStepsByRecipeIdQueryDto, GetStepsByRecipeIdQuery> stepsQueryHandler,
            IQueryHandler<GetIngredientsByRecipeIdQueryDto, GetIngredientsByRecipeIdQuery> ingredientsQueryHandler,
            IQueryHandler<GetTagsByRecipeIdQueryDto, GetTagsByRecipeIdQuery> tagsQueryHandler )
        {
            _recipeRepository = recipeRepository;
            _recipeQueryValidator = validator;
            _stepsQueryHandler = stepsQueryHandler;
            _ingredientsQueryHandler = ingredientsQueryHandler;
            _tagsQueryHandler = tagsQueryHandler;
        }

        public async Task<QueryResult<GetRecipeByIdQueryDto>> HandleAsync( GetRecipeByIdQuery query )
        {
            ValidationResult validationResult = await _recipeQueryValidator.ValidationAsync( query );
            if ( validationResult.IsFail )
            {
                return new QueryResult<GetRecipeByIdQueryDto>( validationResult );
            }

            Recipe foundRecipe = await _recipeRepository.GetByIdAsync( query.Id );
            if ( foundRecipe == null )
            {
                return new QueryResult<GetRecipeByIdQueryDto>( ValidationResult.Fail( "Recipe not found" ) );
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

                Steps = stepsResult.ObjResult.Steps.Select( step => new StepDto
                {
                    StepNumber = step.StepNumber,
                    StepDescription = step.StepDescription
                } ).ToList(),

                Ingredients = ingredientsResult.ObjResult.Ingredients.Select( ingredient => new IngredientDto
                {
                    Title = ingredient.Title,
                    Description = ingredient.Description
                } ).ToList(),

                Tags = tagsResult.ObjResult.Tags.Select( tag => new TagDto
                {
                    Name = tag.Name
                } ).ToList()
            };

            return new QueryResult<GetRecipeByIdQueryDto>( getRecipeByIdQueryDto );
        }
    }
}
