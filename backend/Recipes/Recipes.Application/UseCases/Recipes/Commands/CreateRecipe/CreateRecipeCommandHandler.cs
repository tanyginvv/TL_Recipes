using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Ingredients.Commands.CreateIngredient;
using Recipes.Application.UseCases.Steps.Commands.CreateStepCommand;
using Recipes.Application.UseCases.Tags.Commands.CreateTag.CreateTagCommand;
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
        : ICommandHandler<CreateRecipeCommand>
    {
        private IRecipeRepository _recipeRepository => recipeRepository;
        private ITagRepository _tagRepository => tagRepository;
        private IAsyncValidator<CreateRecipeCommand> _createRecipeCommandValidator => validator;
        private ICommandHandler<CreateTagCommand> _createTagCommandHandler => createTagCommandHandler;
        private ICommandHandler<CreateIngredientCommand> _createIngredientCommandHandler => createIngredientCommandHandler;
        private ICommandHandler<CreateStepCommand> _createStepCommandHandler => createStepCommandHandler;
        private IUnitOfWork _unitOfWork => unitOfWork;

        public async Task<Result> HandleAsync( CreateRecipeCommand createRecipeCommand )
        {
            Result validationResult = await _createRecipeCommandValidator.ValidationAsync( createRecipeCommand );
            if ( !validationResult.IsSuccess )
            {
                return Result.FromError( validationResult.Error.Message );
            }

            Recipe recipe = new Recipe(
            createRecipeCommand.Name,
            createRecipeCommand.Description,
            createRecipeCommand.CookTime,
            createRecipeCommand.CountPortion,
            createRecipeCommand.ImageUrl );

            await _recipeRepository.AddAsync( recipe );
            await _unitOfWork.CommitAsync();

            foreach ( var tagDto in createRecipeCommand.Tags )
            {
                var existingTag = await _tagRepository.GetByNameAsync( tagDto.Name );
                if ( existingTag == null )
                {
                    var createTagCommand = new CreateTagCommand { Name = tagDto.Name };
                    await _createTagCommandHandler.HandleAsync( createTagCommand );
                    existingTag = await _tagRepository.GetByNameAsync( tagDto.Name );
                }
                recipe.Tags.Add( existingTag );
            }

            await _unitOfWork.CommitAsync();

            foreach ( var ingredientDto in createRecipeCommand.Ingredients )
            {
                var createIngredientCommand = new CreateIngredientCommand
                {
                    Title = ingredientDto.Title,
                    Description = ingredientDto.Description,
                    RecipeId = recipe.Id
                };
                await _createIngredientCommandHandler.HandleAsync( createIngredientCommand );
            }

            foreach ( var stepDto in createRecipeCommand.Steps )
            {
                var createStepCommand = new CreateStepCommand
                {
                    StepNumber = stepDto.StepNumber,
                    StepDescription = stepDto.StepDescription,
                    RecipeId = recipe.Id
                };
                await _createStepCommandHandler.HandleAsync( createStepCommand );
            }

            await _unitOfWork.CommitAsync();

            return Result.FromSuccess();
        }
    }
}