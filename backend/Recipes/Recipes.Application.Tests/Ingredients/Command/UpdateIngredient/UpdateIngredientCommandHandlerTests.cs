using Moq;
using Recipes.Application.UseCases.Ingredients.Commands.UpdateIngredient;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Domain.Entities;
using Recipes.Application.CQRSInterfaces;

namespace Recipes.Application.Tests.Ingredients.Command.UpdateIngredient;

public class UpdateIngredientCommandHandlerTests
{
    private readonly Mock<IIngredientRepository> _ingredientRepositoryMock;
    private readonly Mock<IAsyncValidator<UpdateIngredientCommand>> _validatorMock;
    private readonly UpdateIngredientCommandHandler _handler;

    public UpdateIngredientCommandHandlerTests()
    {
        _ingredientRepositoryMock = new Mock<IIngredientRepository>();
        _validatorMock = new Mock<IAsyncValidator<UpdateIngredientCommand>>();
        _handler = new UpdateIngredientCommandHandler( _ingredientRepositoryMock.Object, _validatorMock.Object );
    }

    [Fact]
    public async Task HandleImplAsync_Should_ReturnError_When_IngredientNotFound()
    {
        // Arrange
        UpdateIngredientCommand command = new UpdateIngredientCommand { Id = 1, Title = "New Title", Description = "New Description" };

        _ingredientRepositoryMock
            .Setup( x => x.GetByIdAsync( It.IsAny<int>() ) )
            .ReturnsAsync( null as Ingredient  ); // Ingredient not found

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
    public async Task HandleImplAsync_Should_UpdateIngredient_When_IngredientExists()
    {
        // Arrange
        Ingredient ingredient = new Ingredient( "Old Title", "Old Description", 1 ) { Id = 1 };
        UpdateIngredientCommand command = new UpdateIngredientCommand { Id = 1, Title = "New Title", Description = "New Description" };

        _ingredientRepositoryMock
            .Setup( x => x.GetByIdAsync( It.IsAny<int>() ) )
            .ReturnsAsync( ingredient ); // Ingredient found

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
    public async Task HandleImplAsync_Should_NotUpdateIngredient_When_ValidationFails()
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