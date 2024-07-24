using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Interfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Ingredients.Commands.CreateIngredient;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.UseCases.Steps.Commands;
using Recipes.Application.UseCases.Tags.Commands.CreateTag;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Recipes.Commands.CreateRecipe
{
    public class CreateRecipeCommandHandler(
            IRecipeRepository recipeRepository,
            ITagRepository tagRepository,
            IAsyncValidator<CreateRecipeCommand> validator,
            ICommandHandlerWithResult<CreateTagCommand, Tag> createTagCommandHandler,
            ICommandHandlerWithResult<CreateIngredientCommand, Ingredient> createIngredientCommandHandler,
            ICommandHandlerWithResult<CreateStepCommand, Step> createStepCommandHandler,
            IUnitOfWork unitOfWork )
        : ICommandHandlerWithResult<CreateRecipeCommand, int>
    {
        public async Task<Result<int>> HandleAsync( CreateRecipeCommand createRecipeCommand )
        {
            Result validationResult = await validator.ValidateAsync( createRecipeCommand );
            if ( !validationResult.IsSuccess )
            {
                return Result<int>.FromError( validationResult.Error );
            }

            Recipe recipe = new Recipe(
                createRecipeCommand.Name,
                createRecipeCommand.Description,
                createRecipeCommand.CookTime,
                createRecipeCommand.PortionCount,
                createRecipeCommand.ImageUrl );

            await recipeRepository.AddAsync( recipe );

            foreach ( TagDto tagDto in createRecipeCommand.Tags )
            {
                CreateTagCommand createTagCommand = new() { Name = tagDto.Name };
                Result<Tag> tagResult = await createTagCommandHandler.HandleAsync( createTagCommand );

                if ( !tagResult.IsSuccess )
                {
                    return Result<int>.FromError( tagResult.Error );
                }

                recipe.Tags.Add( tagResult.Value );
            }

            foreach ( IngredientDto ingredientDto in createRecipeCommand.Ingredients )
            {
                CreateIngredientCommand createIngredientCommand = new()
                {
                    Title = ingredientDto.Title,
                    Description = ingredientDto.Description,
                    RecipeId = recipe.Id
                };
                Result<Ingredient> ingredientResult = await createIngredientCommandHandler.HandleAsync( createIngredientCommand );

                if ( !ingredientResult.IsSuccess )
                {
                    return Result<int>.FromError( ingredientResult.Error );
                }

                recipe.Ingredients.Add( ingredientResult.Value );
            }

            foreach ( StepDto stepDto in createRecipeCommand.Steps )
            {
                CreateStepCommand createStepCommand = new()
                {
                    StepNumber = stepDto.StepNumber,
                    StepDescription = stepDto.StepDescription,
                    RecipeId = recipe.Id
                };
                Result<Step> stepResult = await createStepCommandHandler.HandleAsync( createStepCommand );

                if ( !stepResult.IsSuccess )
                {
                    return Result<int>.FromError( stepResult.Error );
                }

                recipe.Steps.Add( stepResult.Value );
            }

            await unitOfWork.CommitAsync();

            return Result<int>.FromSuccess( recipe.Id );
        }
    }
}