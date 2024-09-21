using Microsoft.Extensions.Logging;
using Moq;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Interfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Ingredients.Commands.CreateIngredient;
using Recipes.Application.UseCases.Recipes.Commands.CreateRecipe;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.UseCases.Steps.Commands.CreateStep;
using Recipes.Application.UseCases.Tags.Commands.GetOrCreateTag;
using Recipes.Domain.Entities;

namespace Recipes.Application.Tests.Recipes.Commands.CreateRecipe;

public class CreateRecipeCommandHandlerTests
{
    private readonly Mock<IRecipeRepository> _mockRecipeRepository;
    private readonly Mock<IAsyncValidator<CreateRecipeCommand>> _mockValidator;
    private readonly Mock<ICommandHandlerWithResult<GetOrCreateTagCommand, Tag>> _mockCreateTagCommandHandler;
    private readonly Mock<ICommandHandlerWithResult<CreateIngredientCommand, Ingredient>> _mockCreateIngredientCommandHandler;
    private readonly Mock<ICommandHandlerWithResult<CreateStepCommand, Step>> _mockCreateStepCommandHandler;
    private readonly Mock<IImageTools> _mockImageTools;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly CreateRecipeCommandHandler _handler;
    private readonly Mock<ILogger<CreateRecipeCommand>> _logger;

    public CreateRecipeCommandHandlerTests()
    {
        _mockRecipeRepository = new Mock<IRecipeRepository>();
        _mockValidator = new Mock<IAsyncValidator<CreateRecipeCommand>>();
        _mockCreateTagCommandHandler = new Mock<ICommandHandlerWithResult<GetOrCreateTagCommand, Tag>>();
        _mockCreateIngredientCommandHandler = new Mock<ICommandHandlerWithResult<CreateIngredientCommand, Ingredient>>();
        _mockCreateStepCommandHandler = new Mock<ICommandHandlerWithResult<CreateStepCommand, Step>>();
        _mockImageTools = new Mock<IImageTools>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _logger = new Mock<ILogger<CreateRecipeCommand>>();

        _handler = new CreateRecipeCommandHandler(
            _mockRecipeRepository.Object,
            _mockValidator.Object,
            _mockCreateTagCommandHandler.Object,
            _mockCreateIngredientCommandHandler.Object,
            _mockCreateStepCommandHandler.Object,
            _mockImageTools.Object,
            _mockUnitOfWork.Object,
            _logger.Object );
    }

    [Fact]
    public async Task HandleAsync_ValidCommand_SuccessfullyCreatesRecipe()
    {
        // Arrange
        CreateRecipeCommand command = new CreateRecipeCommand
        {
            AuthorId = 1,
            Name = "Valid Recipe",
            Description = "A valid description.",
            PortionCount = 2,
            CookTime = 30,
            ImageUrl = "http://example.com/image.jpg",
            Tags = new List<TagDto>(),
            Ingredients = new List<IngredientDto>(),
            Steps = new List<StepDto>()
        };

        _mockValidator.Setup( v => v.ValidateAsync( command ) ).ReturnsAsync( Result.Success );
        _mockCreateTagCommandHandler.Setup( h => h.HandleAsync( It.IsAny<GetOrCreateTagCommand>() ) )
            .ReturnsAsync( Result<Tag>.FromSuccess( new Tag( "" ) ) );
        _mockCreateIngredientCommandHandler.Setup( h => h.HandleAsync( It.IsAny<CreateIngredientCommand>() ) )
            .ReturnsAsync( Result<Ingredient>.FromSuccess( new Ingredient( "", "", 1 ) ) );
        _mockCreateStepCommandHandler.Setup( h => h.HandleAsync( It.IsAny<CreateStepCommand>() ) )
            .ReturnsAsync( Result<Step>.FromSuccess( new Step( 1, "", 1 ) ) );
        _mockRecipeRepository.Setup( r => r.AddAsync( It.IsAny<Recipe>() ) ).Returns( Task.CompletedTask );
        _mockUnitOfWork.Setup( u => u.CommitAsync() ).Returns( Task.CompletedTask );

        // Act
        Result<RecipeIdDto> result = await _handler.HandleAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.NotNull( result.Value );
        _mockRecipeRepository.Verify( r => r.AddAsync( It.IsAny<Recipe>() ), Times.Once );
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Once );
    }

    [Fact]
    public async Task HandleAsync_ValidationFails_ReturnsError()
    {
        // Arrange
        CreateRecipeCommand command = new CreateRecipeCommand
        {
            AuthorId = 1,
            Name = "Valid Recipe",
            Description = "A valid description.",
            PortionCount = 2,
            CookTime = 30,
            ImageUrl = "http://example.com/image.jpg",
            Tags = new List<TagDto>(),
            Ingredients = new List<IngredientDto>(),
            Steps = new List<StepDto>()
        };

        _mockValidator.Setup( v => v.ValidateAsync( command ) ).ReturnsAsync( Result.FromError( "Validation error" ) );

        // Act
        Result<RecipeIdDto> result = await _handler.HandleAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Validation error", result.Error.Message );
        _mockRecipeRepository.Verify( r => r.AddAsync( It.IsAny<Recipe>() ), Times.Never );
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Never );
    }

    [Fact]
    public async Task HandleAsync_CreateTagFails_ReturnsError()
    {
        // Arrange
        CreateRecipeCommand command = new CreateRecipeCommand
        {
            AuthorId = 1,
            Name = "Valid Recipe",
            Description = "A valid description.",
            PortionCount = 2,
            CookTime = 30,
            ImageUrl = "http://example.com/image.jpg",
            Tags = new List<TagDto> { new TagDto() },
            Ingredients = new List<IngredientDto>(),
            Steps = new List<StepDto>()
        };

        _mockValidator.Setup( v => v.ValidateAsync( command ) ).ReturnsAsync( Result.Success );
        _mockCreateTagCommandHandler.Setup( h => h.HandleAsync( It.IsAny<GetOrCreateTagCommand>() ) )
            .ReturnsAsync( Result<Tag>.FromError( "Tag creation error" ) );

        // Act
        Result<RecipeIdDto> result = await _handler.HandleAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Tag creation error", result.Error.Message );
        _mockRecipeRepository.Verify( r => r.AddAsync( It.IsAny<Recipe>() ), Times.Never );
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Never );
    }

    [Fact]
    public async Task HandleAsync_CreateIngredientFails_ReturnsError()
    {
        // Arrange
        CreateRecipeCommand command = new CreateRecipeCommand
        {
            AuthorId = 1,
            Name = "Valid Recipe",
            Description = "A valid description.",
            PortionCount = 2,
            CookTime = 30,
            ImageUrl = "http://example.com/image.jpg",
            Tags = new List<TagDto>(),
            Ingredients = new List<IngredientDto> { new IngredientDto() },
            Steps = new List<StepDto>()
        };

        _mockValidator.Setup( v => v.ValidateAsync( command ) ).ReturnsAsync( Result.Success );
        _mockCreateTagCommandHandler.Setup( h => h.HandleAsync( It.IsAny<GetOrCreateTagCommand>() ) )
            .ReturnsAsync( Result<Tag>.FromSuccess( new Tag( "" ) ) );
        _mockCreateIngredientCommandHandler.Setup( h => h.HandleAsync( It.IsAny<CreateIngredientCommand>() ) )
            .ReturnsAsync( Result<Ingredient>.FromError( "Ingredient creation error" ) );

        // Act
        Result<RecipeIdDto> result = await _handler.HandleAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Ingredient creation error", result.Error.Message );
        _mockRecipeRepository.Verify( r => r.AddAsync( It.IsAny<Recipe>() ), Times.Never );
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Never );
    }

    [Fact]
    public async Task HandleAsync_CreateStepFails_ReturnsError()
    {
        // Arrange
        CreateRecipeCommand command = new CreateRecipeCommand
        {
            AuthorId = 1,
            Name = "Valid Recipe",
            Description = "A valid description.",
            PortionCount = 2,
            CookTime = 30,
            ImageUrl = "http://example.com/image.jpg",
            Tags = new List<TagDto>(),
            Ingredients = new List<IngredientDto>(),
            Steps = new List<StepDto> { new StepDto() }
        };

        _mockValidator.Setup( v => v.ValidateAsync( command ) ).ReturnsAsync( Result.Success );
        _mockCreateTagCommandHandler.Setup( h => h.HandleAsync( It.IsAny<GetOrCreateTagCommand>() ) )
            .ReturnsAsync( Result<Tag>.FromSuccess( new Tag( "" ) ) );
        _mockCreateIngredientCommandHandler.Setup( h => h.HandleAsync( It.IsAny<CreateIngredientCommand>() ) )
            .ReturnsAsync( Result<Ingredient>.FromSuccess( new Ingredient( "", "", 1 ) ) );
        _mockCreateStepCommandHandler.Setup( h => h.HandleAsync( It.IsAny<CreateStepCommand>() ) )
            .ReturnsAsync( Result<Step>.FromError( "Step creation error" ) );

        // Act
        Result<RecipeIdDto> result = await _handler.HandleAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Step creation error", result.Error.Message );
        _mockRecipeRepository.Verify( r => r.AddAsync( It.IsAny<Recipe>() ), Times.Never );
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Never );
    }

    [Fact]
    public async Task HandleAsync_ErrorDuringExceptionHandling_DoesNotThrow()
    {
        // Arrange
        CreateRecipeCommand command = new CreateRecipeCommand
        {
            AuthorId = 1,
            Name = "Valid Recipe",
            Description = "A valid description.",
            PortionCount = 2,
            CookTime = 30,
            ImageUrl = "http://example.com/image.jpg",
            Tags = new List<TagDto>(),
            Ingredients = new List<IngredientDto>(),
            Steps = new List<StepDto>()
        };

        _mockValidator.Setup( v => v.ValidateAsync( command ) ).ReturnsAsync( Result.Success );
        _mockCreateTagCommandHandler.Setup( h => h.HandleAsync( It.IsAny<GetOrCreateTagCommand>() ) )
            .ReturnsAsync( Result<Tag>.FromSuccess( new Tag( "" ) ) );
        _mockCreateIngredientCommandHandler.Setup( h => h.HandleAsync( It.IsAny<CreateIngredientCommand>() ) )
            .ReturnsAsync( Result<Ingredient>.FromSuccess( new Ingredient( "", "", 1 ) ) );
        _mockCreateStepCommandHandler.Setup( h => h.HandleAsync( It.IsAny<CreateStepCommand>() ) )
            .ReturnsAsync( Result<Step>.FromSuccess( new Step( 1, "", 1 ) ) );
        _mockRecipeRepository.Setup( r => r.AddAsync( It.IsAny<Recipe>() ) ).Returns( Task.CompletedTask );
        _mockUnitOfWork.Setup( u => u.CommitAsync() ).Returns( Task.CompletedTask );

        _mockImageTools.Setup( i => i.DeleteImage( It.IsAny<string>() ) ).Throws( new Exception( "Delete image error" ) );
        _mockCreateTagCommandHandler.Setup( h => h.HandleAsync( It.IsAny<GetOrCreateTagCommand>() ) )
            .Throws( new Exception( "Test exception" ) );

        // Act
        Result<RecipeIdDto> result = await _handler.HandleAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
        _mockImageTools.Verify( i => i.DeleteImage( It.IsAny<string>() ), Times.Never );
    }
}