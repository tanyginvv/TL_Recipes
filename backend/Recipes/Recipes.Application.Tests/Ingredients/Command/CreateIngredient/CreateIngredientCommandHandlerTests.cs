using Moq;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Ingredients.Commands.CreateIngredient;
using Recipes.Domain.Entities;
using Recipes.Application.CQRSInterfaces;
using Microsoft.Extensions.Logging;

namespace Recipes.Application.Tests.Ingredients.Command.CreateIngredient;

public class CreateIngredientCommandHandlerTests
{
    private readonly Mock<IIngredientRepository> _mockIngredientRepository;
    private readonly Mock<IAsyncValidator<CreateIngredientCommand>> _mockValidator;
    private readonly CreateIngredientCommandHandler _handler;
    private readonly Mock<ILogger<CreateIngredientCommand>> _loggerMock;

    public CreateIngredientCommandHandlerTests()
    {
        _mockIngredientRepository = new Mock<IIngredientRepository>();
        _mockValidator = new Mock<IAsyncValidator<CreateIngredientCommand>>();
        _loggerMock = new Mock<ILogger<CreateIngredientCommand>>();
        _handler = new CreateIngredientCommandHandler(
            _mockIngredientRepository.Object,
            _mockValidator.Object,
            _loggerMock.Object );
    }

    [Fact]
    public async Task HandleImplAsync_ValidCommand_ReturnsSuccessWithIngredient()
    {
        // Arrange
        CreateIngredientCommand command = new CreateIngredientCommand
        {
            Title = "New Ingredient",
            Description = "Description of the new ingredient",
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1 }
        };
        Ingredient ingredient = new Ingredient( command.Title, command.Description, command.Recipe.Id );

        _mockValidator.Setup( v => v.ValidateAsync( command ) )
            .ReturnsAsync( Result.Success );
        _mockIngredientRepository.Setup( r => r.AddAsync( It.IsAny<Ingredient>() ) )
            .Returns( Task.CompletedTask );

        // Act
        Result<Ingredient> result = await _handler.HandleAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.NotNull( result.Value );
        Assert.Equal( command.Title, result.Value.Title );
        Assert.Equal( command.Description, result.Value.Description );
        Assert.Equal( command.Recipe.Id, result.Value.RecipeId );
    }

    [Fact]
    public async Task HandleImplAsync_InvalidCommand_ReturnsError()
    {
        // Arrange
        CreateIngredientCommand command = new CreateIngredientCommand
        {
            Title = "New Ingredient",
            Description = "Description of the new ingredient",
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1 }
        };

        _mockValidator.Setup( v => v.ValidateAsync( command ) )
            .ReturnsAsync( Result.FromError( "Validation failed" ) );

        // Act
        Result<Ingredient> result = await _handler.HandleAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Validation failed", result.Error.Message );
    }

    [Fact]
    public async Task HandleImplAsync_RepositoryError_ReturnsError()
    {
        // Arrange
        CreateIngredientCommand command = new CreateIngredientCommand
        {
            Title = "New Ingredient",
            Description = "Description of the new ingredient",
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1 }
        };

        _mockValidator.Setup( v => v.ValidateAsync( command ) )
            .ReturnsAsync( Result.Success );
        _mockIngredientRepository.Setup( r => r.AddAsync( It.IsAny<Ingredient>() ) )
            .ThrowsAsync( new Exception( "Repository error" ) );

        // Act
        Result<Ingredient> result = await _handler.HandleAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Repository error", result.Error.Message );
    }
}