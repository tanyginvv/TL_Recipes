using System.ComponentModel.DataAnnotations;
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

namespace Recipes.Application.UseCases.Recipes.Queries.SearchRecipes
{
    public class SearchRecipesQueryHandler(
            IRecipeRepository recipeRepository,
            IQueryHandler<GetStepsByRecipeIdQueryDto, GetStepsByRecipeIdQuery> stepsQueryHandler,
            IQueryHandler<GetIngredientsByRecipeIdQueryDto, GetIngredientsByRecipeIdQuery> ingredientsQueryHandler,
            IQueryHandler<GetTagsByRecipeIdQueryDto, GetTagsByRecipeIdQuery> tagsQueryHandler,
            IAsyncValidator<SearchRecipesQuery> validator ) : IQueryHandler<IEnumerable<RecipeDto>, SearchRecipesQuery>
    {
        private IRecipeRepository _recipeRepository => recipeRepository;
        private IQueryHandler<GetStepsByRecipeIdQueryDto, GetStepsByRecipeIdQuery> _stepsQueryHandler => stepsQueryHandler;
        private IQueryHandler<GetIngredientsByRecipeIdQueryDto, GetIngredientsByRecipeIdQuery> _ingredientsQueryHandler => ingredientsQueryHandler;
        private IQueryHandler<GetTagsByRecipeIdQueryDto, GetTagsByRecipeIdQuery> _tagsQueryHandler => tagsQueryHandler;
        private IAsyncValidator<SearchRecipesQuery> _validator => validator;

        public async Task<Result<IEnumerable<RecipeDto>>> HandleAsync( SearchRecipesQuery query )
        {
            var validationResult = await _validator.ValidationAsync( query );
            if ( !validationResult.IsSuccess )
            {
                return Result<IEnumerable<RecipeDto>>.FromError( validationResult.Error );
            }
            var recipes = await _recipeRepository.GetFilteredRecipesAsync( query.SearchTerms );

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
                    Steps = stepsResult.IsSuccess ? stepsResult.Value.Steps.Select( step => new StepDtoUseCases
                    {
                        StepNumber = step.StepNumber,
                        StepDescription = step.StepDescription
                    } ).ToList() : new List<StepDtoUseCases>(),
                    Ingredients = ingredientsResult.IsSuccess ? ingredientsResult.Value.Ingredients.Select( ingredient => new IngredientDtoUseCases
                    {
                        Title = ingredient.Title,
                        Description = ingredient.Description
                    } ).ToList() : new List<IngredientDtoUseCases>(),
                    Tags = tagsResult.IsSuccess ? tagsResult.Value.Tags.Select( tag => new TagDtoUseCases
                    {
                        Name = tag.Name
                    } ).ToList() : new List<TagDtoUseCases>()
                };

                recipeDtos.Add( recipeDto );
            }

            return Result<IEnumerable<RecipeDto>>.FromSuccess( recipeDtos );
        }
    }
}