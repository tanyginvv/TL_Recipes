using Recipes.Domain.Entities;
using Recipes.Application.Validation;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Results;
using Recipes.Application.Repositories;
using Recipes.Application.UseCases.Tags.Dtos;
using Recipes.Application.UseCases.Steps.Dtos;
using Recipes.Application.UseCases.Tags.Queries.GetTagsByRecipeIdQuery;
using Recipes.Application.UseCases.Steps.Queries.GetStepsByRecipeIdQuery;
using Recipes.Application.UseCases.Ingredients.Dtos;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.UseCases.Ingredients.Queries.GetIngredientsByRecipeIdQuery;

namespace Recipes.Application.UseCases.Recipes.Queries.GetAllRecipes
{
    public class GetAllRecipesQueryHandler(
            IRecipeRepository recipeRepository,
            IAsyncValidator<GetAllRecipesQuery> validator,
            IQueryHandler<GetStepsByRecipeIdQueryDto, GetStepsByRecipeIdQuery> stepsQueryHandler,
            IQueryHandler<GetIngredientsByRecipeIdQueryDto, GetIngredientsByRecipeIdQuery> ingredientsQueryHandler,
            IQueryHandler<GetTagsByRecipeIdQueryDto, GetTagsByRecipeIdQuery> tagsQueryHandler )
        : IQueryHandler<IEnumerable<RecipeDto>, GetAllRecipesQuery>
    {
        private IRecipeRepository _recipeRepository => recipeRepository;
        private IAsyncValidator<GetAllRecipesQuery> _recipeQueryValidator => validator;
        private IQueryHandler<GetStepsByRecipeIdQueryDto, GetStepsByRecipeIdQuery> _stepsQueryHandler => stepsQueryHandler;
        private IQueryHandler<GetIngredientsByRecipeIdQueryDto, GetIngredientsByRecipeIdQuery> _ingredientsQueryHandler => ingredientsQueryHandler;
        private IQueryHandler<GetTagsByRecipeIdQueryDto, GetTagsByRecipeIdQuery> _tagsQueryHandler => tagsQueryHandler;

        public async Task<Result<IEnumerable<RecipeDto>>> HandleAsync( GetAllRecipesQuery query )
        {
            Result validationResult = await _recipeQueryValidator.ValidationAsync( query );
            if ( !validationResult.IsSuccess )
            {
                return Result<IEnumerable<RecipeDto>>.FromError( validationResult );
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

                recipeDtos.Add( recipeDto );
            }

            return Result<IEnumerable<RecipeDto>>.FromSuccess( recipeDtos );
        }
    }
}
