using Recipes.Application.UseCases.Ingredients.Commands.UpdateIngredient;
using Recipes.Application.Results;

namespace Recipes.Application.Tests.Ingredients.Command.UpdateIngredient;

public class UpdateIngredientCommandValidatorTests
{
    private readonly UpdateIngredientCommandValidator _validator;

    public UpdateIngredientCommandValidatorTests()
    {
        _validator = new UpdateIngredientCommandValidator();
    }

    [Fact]
    public async Task ValidateAsync_IdIsInvalid_ReturnsError()
    {
        // Arrange
        UpdateIngredientCommand command = new UpdateIngredientCommand
        {
            Id = 0, 
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
    public async Task ValidateAsync_TitleIsEmpty_ReturnsError()
    {
        // Arrange
        UpdateIngredientCommand command = new UpdateIngredientCommand
        {
            Id = 1,
            Title = "", 
            Description = "Valid Description"
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Название ингредиента не может быть пустым", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_TitleIsTooLong_ReturnsError()
    {
        // Arrange
        UpdateIngredientCommand command = new UpdateIngredientCommand
        {
            Id = 1,
            Title = new string( 'a', 101 ),
            Description = "Valid Description"
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Название ингредиента не может быть больше чем 100 символов", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_DescriptionIsEmpty_ReturnsError()
    {
        // Arrange
        UpdateIngredientCommand command = new UpdateIngredientCommand
        {
            Id = 1,
            Title = "Valid Title",
            Description = "" 
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Описание ингредиента не может быть пустым", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_DescriptionIsTooLong_ReturnsError()
    {
        // Arrange
        UpdateIngredientCommand command = new UpdateIngredientCommand
        {
            Id = 1,
            Title = "Valid Title",
            Description = new string( 'a', 251 ) 
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Описание ингредиента не может быть больше чем 250 символов", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_ValidCommand_ReturnsSuccess()
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