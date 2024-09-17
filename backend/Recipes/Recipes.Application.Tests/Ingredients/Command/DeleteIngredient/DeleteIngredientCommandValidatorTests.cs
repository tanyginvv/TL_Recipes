using Moq;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Ingredients.Commands.DeleteIngredient;

namespace Recipes.Application.Tests.Ingredients.Command.DeleteIngredient;

public class DeleteIngredientCommandValidatorTests
{
    private readonly DeleteIngredientCommandValidator _validator;

    public DeleteIngredientCommandValidatorTests()
    {
        Mock<IIngredientRepository> ingredientRepositoryMock = new Mock<IIngredientRepository>();
        _validator = new DeleteIngredientCommandValidator( ingredientRepositoryMock.Object );
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnSuccess_WhenIdIsValid()
    {
        // Arrange
        DeleteIngredientCommand command = new DeleteIngredientCommand { Id = 1 };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnError_WhenIdIsZero()
    {
        // Arrange
        DeleteIngredientCommand command = new DeleteIngredientCommand { Id = 0 };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Неверный ID ингредиента.", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnError_WhenIdIsNegative()
    {
        // Arrange
        DeleteIngredientCommand command = new DeleteIngredientCommand { Id = -1 };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Неверный ID ингредиента.", result.Error.Message );
    }
}