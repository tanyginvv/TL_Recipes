using Recipes.Application.UseCases.Ingredients.Commands.UpdateIngredient;
using Recipes.Application.Results;

public class UpdateIngredientCommandValidatorTests
{
    private readonly UpdateIngredientCommandValidator _validator;

    public UpdateIngredientCommandValidatorTests()
    {
        _validator = new UpdateIngredientCommandValidator();
    }

    [Fact]
    public async Task ValidateAsync_Should_ReturnError_When_IdIsInvalid()
    {
        // Arrange
        UpdateIngredientCommand command = new UpdateIngredientCommand
        {
            Id = 0, // Invalid ID
            Title = "Valid Title",
            Description = "Valid Description"
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "ID ингредиента должен быть больше нуля", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_Should_ReturnError_When_TitleIsEmpty()
    {
        // Arrange
        UpdateIngredientCommand command = new UpdateIngredientCommand
        {
            Id = 1,
            Title = "", // Empty title
            Description = "Valid Description"
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Название ингредиента не может быть пустым", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_Should_ReturnError_When_TitleIsTooLong()
    {
        // Arrange
        UpdateIngredientCommand command = new UpdateIngredientCommand
        {
            Id = 1,
            Title = new string( 'a', 101 ), // Title longer than 100 characters
            Description = "Valid Description"
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Название ингредиента не может быть больше чем 100 символов", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_Should_ReturnError_When_DescriptionIsEmpty()
    {
        // Arrange
        UpdateIngredientCommand command = new UpdateIngredientCommand
        {
            Id = 1,
            Title = "Valid Title",
            Description = "" // Empty description
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Описание ингредиента не может быть пустым", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_Should_ReturnError_When_DescriptionIsTooLong()
    {
        // Arrange
        UpdateIngredientCommand command = new UpdateIngredientCommand
        {
            Id = 1,
            Title = "Valid Title",
            Description = new string( 'a', 251 ) // Description longer than 250 characters
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Описание ингредиента не может быть больше чем 250 символов", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_Should_ReturnSuccess_When_ValidCommand()
    {
        // Arrange
        UpdateIngredientCommand command = new UpdateIngredientCommand
        {
            Id = 1,
            Title = "Valid Title",
            Description = "Valid Description"
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
    }
}