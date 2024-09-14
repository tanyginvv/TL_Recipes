using Moq;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Interfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Ingredients.Commands.UpdateIngredients;
using Recipes.Application.UseCases.Recipes.Commands.UpdateRecipe;
using Recipes.Application.UseCases.Steps.Commands.UpdateSteps;
using Recipes.Application.UseCases.Tags.Commands.UpdateRecipeTags;
using Recipes.Domain.Entities;
using Recipes.Application.UseCases.Recipes.Dtos;

namespace Recipes.Application.Tests.Recipes.Commands.UpdateRecipe;

public class UpdateRecipeCommandHandlerTests
{
    private readonly Mock<IRecipeRepository> _mockRecipeRepository;
    private readonly Mock<IAsyncValidator<UpdateRecipeCommand>> _mockValidator;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<ICommandHandler<UpdateStepsCommand>> _mockUpdateStepsCommandHandler;
    private readonly Mock<ICommandHandler<UpdateIngredientsCommand>> _mockUpdateIngredientsCommandHandler;
    private readonly Mock<ICommandHandler<UpdateTagsCommand>> _mockUpdateTagsCommandHandler;
    private readonly Mock<IImageTools> _mockImageTools;
    private readonly UpdateRecipeCommandHandler _handler;

    public UpdateRecipeCommandHandlerTests()
    {
        _mockRecipeRepository = new Mock<IRecipeRepository>();
        _mockValidator = new Mock<IAsyncValidator<UpdateRecipeCommand>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUpdateStepsCommandHandler = new Mock<ICommandHandler<UpdateStepsCommand>>();
        _mockUpdateIngredientsCommandHandler = new Mock<ICommandHandler<UpdateIngredientsCommand>>();
        _mockUpdateTagsCommandHandler = new Mock<ICommandHandler<UpdateTagsCommand>>();
        _mockImageTools = new Mock<IImageTools>();

        _handler = new UpdateRecipeCommandHandler(
            _mockRecipeRepository.Object,
            _mockValidator.Object,
            _mockUnitOfWork.Object,
            _mockUpdateStepsCommandHandler.Object,
            _mockUpdateIngredientsCommandHandler.Object,
            _mockUpdateTagsCommandHandler.Object,
            _mockImageTools.Object
        );
    }

    [Fact]
    public async Task HandleAsync_RecipeNotFound_ReturnsError()
    {
        // Arrange
        UpdateRecipeCommand command = new UpdateRecipeCommand
        {
            Id = 1,
            AuthorId = 1,
            Name = "Updated Recipe",
            Description = "Updated Description",
            CookTime = 30,
            PortionCount = 4,
            ImageUrl = "new_image_url",
            Tags = new List<TagDto>(),
            Steps = new List<StepDto>(),
            Ingredients = new List<IngredientDto>()
        };

        _mockValidator.Setup( v => v.ValidateAsync( command ) ).ReturnsAsync( Result.FromError( "Рецепт не найден" ) );
        _mockRecipeRepository.Setup( r => r.GetByIdAsync( command.Id ) ).ReturnsAsync( null as Recipe  );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Рецепт не найден", result.Error.Message );
        _mockRecipeRepository.Verify( r => r.GetByIdAsync( It.IsAny<int>() ), Times.Never );
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Never );
        _mockImageTools.Verify( i => i.DeleteImage( It.IsAny<string>() ), Times.Once );
    }

    [Fact]
    public async Task HandleAsync_ValidCommand_UpdatesRecipe()
    {
        // Arrange
        UpdateRecipeCommand command = new UpdateRecipeCommand
        {
            Id = 1,
            AuthorId = 1,
            Name = "Updated Recipe",
            Description = "Updated Description",
            CookTime = 30,
            PortionCount = 4,
            ImageUrl = "new_image_url",
            Tags = new List<TagDto>(),
            Steps = new List<StepDto>(),
            Ingredients = new List<IngredientDto>()
        };

        Recipe oldRecipe = new Recipe( 1, "Old Name", "Old Description", 20, 2, "old_image_url" )
        {
            Id = 1
        };

        _mockValidator.Setup( v => v.ValidateAsync( command ) ).ReturnsAsync( Result.Success );
        _mockRecipeRepository.Setup( r => r.GetByIdAsync( command.Id ) ).ReturnsAsync( oldRecipe );
        _mockUpdateStepsCommandHandler.Setup( h => h.HandleAsync( It.IsAny<UpdateStepsCommand>() ) ).ReturnsAsync( Result.Success );
        _mockUpdateIngredientsCommandHandler.Setup( h => h.HandleAsync( It.IsAny<UpdateIngredientsCommand>() ) ).ReturnsAsync( Result.Success );
        _mockUpdateTagsCommandHandler.Setup( h => h.HandleAsync( It.IsAny<UpdateTagsCommand>() ) ).ReturnsAsync( Result.Success );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
        _mockRecipeRepository.Verify( r => r.GetByIdAsync( It.IsAny<int>() ), Times.Once );
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Once );
        _mockImageTools.Verify( i => i.DeleteImage( It.IsAny<string>() ), Times.Once );
    }

    [Fact]
    public async Task HandleAsync_FailureDuringUpdate_CleansUpImage()
    {
        // Arrange
        UpdateRecipeCommand command = new UpdateRecipeCommand
        {
            Id = 1,
            AuthorId = 1,
            Name = "Updated Recipe",
            Description = "Updated Description",
            CookTime = 30,
            PortionCount = 4,
            ImageUrl = "new_image_url",
            Tags = new List<TagDto>(),
            Steps = new List<StepDto>(),
            Ingredients = new List<IngredientDto>()
        };

        Recipe oldRecipe = new Recipe( 1, "Old Name", "Old Description", 20, 2, "old_image_url" )
        {
            Id = 1
        };

        _mockValidator.Setup( v => v.ValidateAsync( command ) ).ReturnsAsync( Result.Success );
        _mockRecipeRepository.Setup( r => r.GetByIdAsync( command.Id ) ).ReturnsAsync( oldRecipe );
        _mockUpdateStepsCommandHandler.Setup( h => h.HandleAsync( It.IsAny<UpdateStepsCommand>() ) ).ReturnsAsync( Result.Success );
        _mockUpdateIngredientsCommandHandler.Setup( h => h.HandleAsync( It.IsAny<UpdateIngredientsCommand>() ) ).ReturnsAsync( Result.Success );
        _mockUpdateTagsCommandHandler.Setup( h => h.HandleAsync( It.IsAny<UpdateTagsCommand>() ) ).ReturnsAsync( Result.Success );
        _mockUnitOfWork.Setup( u => u.CommitAsync() ).ThrowsAsync( new Exception( "Commit failed" ) );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Commit failed", result.Error.Message );
        _mockRecipeRepository.Verify( r => r.GetByIdAsync( It.IsAny<int>() ), Times.Once );
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Once );
        _mockImageTools.Verify( i => i.DeleteImage( command.ImageUrl ), Times.Once );
    }

    [Fact]
    public async Task HandleAsync_ValidationFails_ReturnsError()
    {
        // Arrange
        UpdateRecipeCommand command = new UpdateRecipeCommand
        {
            Id = 1,
            AuthorId = 1,
            Name = "Updated Recipe",
            Description = "Updated Description",
            CookTime = 30,
            PortionCount = 4,
            ImageUrl = "new_image_url",
            Tags = new List<TagDto>(),
            Steps = new List<StepDto>(),
            Ingredients = new List<IngredientDto>()
        };

        _mockValidator.Setup( v => v.ValidateAsync( command ) ).ReturnsAsync( Result.FromError( "Validation error" ) );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Validation error", result.Error.Message );
        _mockRecipeRepository.Verify( r => r.GetByIdAsync( It.IsAny<int>() ), Times.Never );
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Never );
        _mockImageTools.Verify( i => i.DeleteImage( It.IsAny<string>() ), Times.Once );
    }

    [Fact]
    public async Task HandleAsync_UpdateStepsFails_ReturnsError()
    {
        // Arrange
        UpdateRecipeCommand command = new UpdateRecipeCommand
        {
            Id = 1,
            AuthorId = 1,
            Name = "Updated Recipe",
            Description = "Updated Description",
            CookTime = 30,
            PortionCount = 4,
            ImageUrl = "new_image_url",
            Tags = new List<TagDto>(),
            Steps = new List<StepDto> { new StepDto() },
            Ingredients = new List<IngredientDto>()
        };

        Recipe oldRecipe = new Recipe( 1, "Old Name", "Old Description", 20, 2, "old_image_url" )
        {
            Id = 1
        };

        _mockValidator.Setup( v => v.ValidateAsync( command ) ).ReturnsAsync( Result.Success );
        _mockRecipeRepository.Setup( r => r.GetByIdAsync( command.Id ) ).ReturnsAsync( oldRecipe );
        _mockUpdateStepsCommandHandler.Setup( h => h.HandleAsync( It.IsAny<UpdateStepsCommand>() ) ).ThrowsAsync( new Exception( "Steps update error" ) );
        _mockUpdateIngredientsCommandHandler.Setup( h => h.HandleAsync( It.IsAny<UpdateIngredientsCommand>() ) ).ReturnsAsync( Result.Success );
        _mockUpdateTagsCommandHandler.Setup( h => h.HandleAsync( It.IsAny<UpdateTagsCommand>() ) ).ReturnsAsync( Result.Success );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Steps update error", result.Error.Message );
        _mockRecipeRepository.Verify( r => r.GetByIdAsync( It.IsAny<int>() ), Times.Once );
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Never );
        _mockImageTools.Verify( i => i.DeleteImage( It.IsAny<string>() ), Times.Once );
    }

    [Fact]
    public async Task HandleAsync_UpdateIngredientsFails_ReturnsError()
    {
        // Arrange
        UpdateRecipeCommand command = new UpdateRecipeCommand
        {
            Id = 1,
            AuthorId = 1,
            Name = "Updated Recipe",
            Description = "Updated Description",
            CookTime = 30,
            PortionCount = 4,
            ImageUrl = "new_image_url",
            Tags = new List<TagDto>(),
            Ingredients = new List<IngredientDto> { new IngredientDto() },
            Steps = new List<StepDto>()
        };

        Recipe oldRecipe = new Recipe( 1, "Old Name", "Old Description", 20, 2, "old_image_url" )
        {
            Id = 1
        };

        _mockValidator.Setup( v => v.ValidateAsync( command ) ).ReturnsAsync( Result.Success );
        _mockRecipeRepository.Setup( r => r.GetByIdAsync( command.Id ) ).ReturnsAsync( oldRecipe );
        _mockUpdateIngredientsCommandHandler.Setup( h => h.HandleAsync( It.IsAny<UpdateIngredientsCommand>() ) ).ThrowsAsync( new Exception( "Ingredients update error" ) );
        _mockUpdateStepsCommandHandler.Setup( h => h.HandleAsync( It.IsAny<UpdateStepsCommand>() ) ).ReturnsAsync( Result.Success );
        _mockUpdateTagsCommandHandler.Setup( h => h.HandleAsync( It.IsAny<UpdateTagsCommand>() ) ).ReturnsAsync( Result.Success );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Ingredients update error", result.Error.Message );
        _mockRecipeRepository.Verify( r => r.GetByIdAsync( It.IsAny<int>() ), Times.Once );
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Never );
        _mockImageTools.Verify( i => i.DeleteImage( It.IsAny<string>() ), Times.Once );
    }

    [Fact]
    public async Task HandleAsync_UpdateTagsFails_ReturnsError()
    {
        // Arrange
        UpdateRecipeCommand command = new UpdateRecipeCommand
        {
            Id = 1,
            AuthorId = 1,
            Name = "Updated Recipe",
            Description = "Updated Description",
            CookTime = 30,
            PortionCount = 4,
            ImageUrl = "new_image_url",
            Tags = new List<TagDto> { new TagDto() },
            Steps = new List<StepDto>(),
            Ingredients = new List<IngredientDto>()
        };

        Recipe oldRecipe = new Recipe( 1, "Old Name", "Old Description", 20, 2, "old_image_url" )
        {
            Id = 1
        };

        _mockValidator.Setup( v => v.ValidateAsync( command ) ).ReturnsAsync( Result.Success );
        _mockRecipeRepository.Setup( r => r.GetByIdAsync( command.Id ) ).ReturnsAsync( oldRecipe );
        _mockUpdateTagsCommandHandler.Setup( h => h.HandleAsync( It.IsAny<UpdateTagsCommand>() ) ).ThrowsAsync( new Exception( "Tags update error" ) );
        _mockUpdateIngredientsCommandHandler.Setup( h => h.HandleAsync( It.IsAny<UpdateIngredientsCommand>() ) ).ReturnsAsync( Result.Success );
        _mockUpdateStepsCommandHandler.Setup( h => h.HandleAsync( It.IsAny<UpdateStepsCommand>() ) ).ReturnsAsync( Result.Success );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Tags update error", result.Error.Message );
        _mockRecipeRepository.Verify( r => r.GetByIdAsync( It.IsAny<int>() ), Times.Once );
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Never );
        _mockImageTools.Verify( i => i.DeleteImage( It.IsAny<string>() ), Times.Once );
    }

    [Fact]
    public async Task HandleAsync_ExceptionDuringHandle_ReturnsError()
    {
        // Arrange
        UpdateRecipeCommand command = new UpdateRecipeCommand
        {
            Id = 1,
            AuthorId = 1,
            Name = "Updated Recipe",
            Description = "Updated Description",
            CookTime = 30,
            PortionCount = 4,
            ImageUrl = "new_image_url",
            Tags = new List<TagDto>(),
            Steps = new List<StepDto>(),
            Ingredients = new List<IngredientDto>()
        };

        Recipe oldRecipe = new Recipe( 1, "Old Name", "Old Description", 20, 2, "old_image_url" )
        {
            Id = 1
        };

        _mockValidator.Setup( v => v.ValidateAsync( command ) ).ReturnsAsync( Result.Success );
        _mockRecipeRepository.Setup( r => r.GetByIdAsync( command.Id ) ).ReturnsAsync( oldRecipe );
        _mockUpdateStepsCommandHandler.Setup( h => h.HandleAsync( It.IsAny<UpdateStepsCommand>() ) ).ThrowsAsync( new Exception( "Update steps exception" ) );
        _mockUpdateIngredientsCommandHandler.Setup( h => h.HandleAsync( It.IsAny<UpdateIngredientsCommand>() ) ).ReturnsAsync( Result.Success );
        _mockUpdateTagsCommandHandler.Setup( h => h.HandleAsync( It.IsAny<UpdateTagsCommand>() ) ).ReturnsAsync( Result.Success );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Update steps exception", result.Error.Message );
        _mockRecipeRepository.Verify( r => r.GetByIdAsync( It.IsAny<int>() ), Times.Once );
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Never );
        _mockImageTools.Verify( i => i.DeleteImage( It.IsAny<string>() ), Times.Once );
    }
}