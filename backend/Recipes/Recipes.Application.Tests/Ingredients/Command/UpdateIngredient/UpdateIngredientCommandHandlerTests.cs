using Moq;
using Recipes.Application.UseCases.Ingredients.Commands.UpdateIngredient;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Domain.Entities;
using Recipes.Application.CQRSInterfaces;
using Microsoft.Extensions.Logging;

namespace Recipes.Application.Tests.Ingredients.Command.UpdateIngredient;

public class UpdateIngredientCommandHandlerTests
{
    private readonly Mock<IIngredientRepository> _ingredientRepositoryMock;
    private readonly Mock<IAsyncValidator<UpdateIngredientCommand>> _validatorMock;
    private readonly UpdateIngredientCommandHandler _handler;
    private readonly Mock<ILogger<UpdateIngredientCommand>> _loggerMock;

    public UpdateIngredientCommandHandlerTests()
    {
        _ingredientRepositoryMock = new Mock<IIngredientRepository>();
        _validatorMock = new Mock<IAsyncValidator<UpdateIngredientCommand>>();
        _loggerMock = new Mock<ILogger<UpdateIngredientCommand>>();
        _handler = new UpdateIngredientCommandHandler( _ingredientRepositoryMock.Object, _validatorMock.Object, _loggerMock.Object );
    }

    [Fact]
    public async Task HandleAsync_IngredientNotFound_ReturnsError()
    {
        // Arrange
        UpdateIngredientCommand command = new UpdateIngredientCommand { Id = 1, Title = "New Title", Description = "New Description" };

        _ingredientRepositoryMock
            .Setup( x => x.GetByIdAsync( It.IsAny<int>() ) )
            .ReturnsAsync( null as Ingredient ); 

        _validatorMock
            .Setup( v => v.ValidateAsync( command ) )
            .ReturnsAsync( Result.FromError( "Такого id ингредиента не существует" ) );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Такого id ингредиента не существует", result.Error.Message );
        _ingredientRepositoryMock.Verify( x => x.GetByIdAsync( command.Id ), Times.Never );
    }

    [Fact]
    public async Task HandleAsync_IngredientExists_UpdatesIngredient()
    {
        // Arrange
        Ingredient ingredient = new Ingredient( "Old Title", "Old Description", 1 ) { Id = 1 };
        UpdateIngredientCommand command = new UpdateIngredientCommand { Id = 1, Title = "New Title", Description = "New Description" };

        _ingredientRepositoryMock
            .Setup( x => x.GetByIdAsync( It.IsAny<int>() ) )
            .ReturnsAsync( ingredient ); 

        _validatorMock
            .Setup( v => v.ValidateAsync( command ) )
            .ReturnsAsync( Result.Success );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Equal( "New Title", ingredient.Title );
        Assert.Equal( "New Description", ingredient.Description );
        _ingredientRepositoryMock.Verify( x => x.GetByIdAsync( command.Id ), Times.Once );
    }

    [Fact]
    public async Task HandleAsync_ValidationFails_ReturnsError()
    {
        // Arrange
        UpdateIngredientCommand command = new UpdateIngredientCommand { Id = 1, Title = "Invalid Title", Description = "New Description" };

        _validatorMock
            .Setup( v => v.ValidateAsync( command ) )
            .ReturnsAsync( Result.FromError( "Validation failed" ) );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Validation failed", result.Error.Message );
        _ingredientRepositoryMock.Verify( x => x.GetByIdAsync( It.IsAny<int>() ), Times.Never ); 
    }
}