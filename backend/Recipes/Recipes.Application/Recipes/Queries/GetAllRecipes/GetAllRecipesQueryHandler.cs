using Application.CQRSInterfaces;
using Application.Repositories;
using Application.Result;
using Application.Validation;
using Recipes.Application.Ingredients.Dtos;
using Recipes.Application.Ingredients.Queries;
using Recipes.Application.Recipes.Dtos;
using Recipes.Application.Steps.Dtos;
using Recipes.Application.Steps.Queries;
using Recipes.Application.Tags.Dtos;
using Recipes.Domain.Entities;
using Recipes.Application.Tags.Queries.GetTagsByRecipeIdQuery;

namespace Recipes.Application.Recipes.Queries.GetAllRecipes
{
    public class GetAllRecipesQueryHandler : IQueryHandler<IEnumerable<RecipeDto>, GetAllRecipesQuery>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IAsyncValidator<GetAllRecipesQuery> _recipeQueryValidator;
        private readonly IQueryHandler<GetStepsByRecipeIdQueryDto, GetStepsByRecipeIdQuery> _stepsQueryHandler;
        private readonly IQueryHandler<GetIngredientsByRecipeIdQueryDto, GetIngredientsByRecipeIdQuery> _ingredientsQueryHandler;
        private readonly IQueryHandler<GetTagsByRecipeIdQueryDto, GetTagsByRecipeIdQuery> _tagsQueryHandler;

        public GetAllRecipesQueryHandler(
            IRecipeRepository recipeRepository,
            IAsyncValidator<GetAllRecipesQuery> validator,
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

        public async Task<QueryResult<IEnumerable<RecipeDto>>> HandleAsync( GetAllRecipesQuery query )
        {
            ValidationResult validationResult = await _recipeQueryValidator.ValidationAsync( query );
            if ( validationResult.IsFail )
            {
                return new QueryResult<IEnumerable<RecipeDto>>( validationResult );
            }

            IEnumerable<Recipe> recipes = await _recipeRepository.GetAllAsync();

            var recipeDtos = new List<RecipeDto>();

            foreach ( var recipe in recipes )
            {
                var stepsQuery = new GetStepsByRecipeIdQuery { RecipeId = recipe.Id };
                var stepsResult = await _stepsQueryHandler.HandleAsync( stepsQuery );

                var ingredientsQuery = new GetIngredientsByRecipeIdQuery { RecipeId = recipe.Id };
                var ingredientsResult = await _ingredientsQueryHandler.HandleAsync( ingredientsQuery );

                var tagsQuery = new GetTagsByRecipeIdQuery { RecipeId = recipe.Id };
                var tagsResult = await _tagsQueryHandler.HandleAsync( tagsQuery );

                var recipeDto = new RecipeDto
                {
                    Id = recipe.Id,
                    Name = recipe.Name,
                    Description = recipe.Description,
                    CookTime = recipe.CookTime,
                    CountPortion = recipe.CountPortion,
                    ImageUrl = recipe.ImageUrl,

                    Steps = stepsResult.ObjResult.Steps.Select( step => new StepDto
                    {
                        Id = step.Id,
                        StepNumber = step.StepNumber,
                        StepDescription = step.StepDescription
                    } ).ToList(),

                    Ingredients = ingredientsResult.ObjResult.Ingredients.Select( ingredient => new IngredientDto
                    {
                        Id = ingredient.Id,
                        Title = ingredient.Title,
                        Description = ingredient.Description
                    } ).ToList(),

                    Tags = tagsResult.ObjResult.Tags.Select( tag => new TagDto
                    {
                        Id = tag.Id,
                        Name = tag.Name
                    } ).ToList()
                };

                recipeDtos.Add( recipeDto );
            }

            return new QueryResult<IEnumerable<RecipeDto>>( recipeDtos );
        }
    }
}
