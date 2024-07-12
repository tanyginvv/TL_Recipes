using Application;
using Application.Repositories;
using Application.Result;
using Application.Validation;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Ingredients.Commands.CreateIngredient;
using Recipes.Application.Steps;
using Recipes.Application.Steps.Commands;
using Recipes.Application.Tags.Commands;
using Recipes.Application.Tags.Commands.CreateTag;
using Recipes.Domain.Entities;
using Recipes.Infrastructure.Entities.Steps;
using Recipes.Infrastructure.Entities.Tags;
using Recipes.Infrastructure.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Recipes.Application.Recipes.Commands.CreateRecipe
{
    public class CreateRecipeCommandHandler : ICommandHandler<CreateRecipeCommand>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IStepRepository _stepRepository;
        private readonly IAsyncValidator<CreateRecipeCommand> _createRecipeCommandValidator;
        private readonly ICommandHandler<CreateTagCommand> _createTagCommandHandler;
        private readonly ICommandHandler<CreateIngredientCommand> _createIngredientCommandHandler;
        private readonly ICommandHandler<CreateStepCommand> _createStepCommandHandler;
        private readonly IUnitOfWork _unitOfWork;

        public CreateRecipeCommandHandler(
            IRecipeRepository recipeRepository,
            ITagRepository tagRepository,
            IIngredientRepository ingredientRepository,
            IStepRepository stepRepository,
            IAsyncValidator<CreateRecipeCommand> validator,
            ICommandHandler<CreateTagCommand> createTagCommandHandler,
            ICommandHandler<CreateIngredientCommand> createIngredientCommandHandler,
            ICommandHandler<CreateStepCommand> createStepCommandHandler,
            IUnitOfWork unitOfWork )
        {
            _recipeRepository = recipeRepository;
            _tagRepository = tagRepository;
            _ingredientRepository = ingredientRepository;
            _stepRepository = stepRepository;
            _createRecipeCommandValidator = validator;
            _createTagCommandHandler = createTagCommandHandler;
            _createIngredientCommandHandler = createIngredientCommandHandler;
            _createStepCommandHandler = createStepCommandHandler;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> HandleAsync( CreateRecipeCommand createRecipeCommand )
        {
            ValidationResult validationResult = await _createRecipeCommandValidator.ValidationAsync( createRecipeCommand );
            if ( validationResult.IsFail )
            {
                return new CommandResult( validationResult );
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

            return new CommandResult( validationResult );
        }
    }
}