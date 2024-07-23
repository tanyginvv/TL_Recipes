using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Ingredients.Commands.CreateIngredient;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.UseCases.Steps.Commands.CreateStepCommand;
using Recipes.Application.UseCases.Tags.Commands.CreateTag;
using Recipes.Application.Validation;
using Recipes.Domain.Entities;

namespace Recipes.Application.UseCases.Recipes.Commands.CreateRecipe
{
    public class CreateRecipeCommandHandler(
            IRecipeRepository recipeRepository,
            ITagRepository tagRepository,
            IAsyncValidator<CreateRecipeCommand> validator,
            ICommandHandler<CreateTagCommand> createTagCommandHandler,
            ICommandHandler<CreateIngredientCommand> createIngredientCommandHandler,
            ICommandHandler<CreateStepCommand> createStepCommandHandler,
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
                Tag existingTag = await tagRepository.GetByNameAsync( tagDto.Name );
                if ( existingTag is null )
                {
                    CreateTagCommand createTagCommand = new() { Name = tagDto.Name };
                    await createTagCommandHandler.HandleAsync( createTagCommand );
                    existingTag = await tagRepository.GetByNameAsync( tagDto.Name );
                }
                recipe.Tags.Add( existingTag );
            }

            foreach ( IngredientDto ingredientDto in createRecipeCommand.Ingredients )
            {
                CreateIngredientCommand createIngredientCommand = new()
                {
                    Title = ingredientDto.Title,
                    Description = ingredientDto.Description,
                    RecipeId = recipe.Id
                };
                await createIngredientCommandHandler.HandleAsync( createIngredientCommand );
            }

            foreach ( StepDto stepDto in createRecipeCommand.Steps )
            {
                CreateStepCommand createStepCommand = new()
                {
                    StepNumber = stepDto.StepNumber,
                    StepDescription = stepDto.StepDescription,
                    RecipeId = recipe.Id
                };
                await createStepCommandHandler.HandleAsync( createStepCommand );
            }

            await unitOfWork.CommitAsync();

            return Result<int>.FromSuccess( recipe.Id );
        }
    }
}