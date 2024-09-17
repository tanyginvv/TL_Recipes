using Recipes.Application.UseCases.Ingredients.Commands.CreateIngredient;
using Recipes.Application.Results;

namespace Recipes.Application.Tests.UseCases.Ingredients.Commands;

public class CreateIngredientCommandValidatorTests
{
    private readonly CreateIngredientCommandValidator _validator;

    public CreateIngredientCommandValidatorTests()
    {
        _validator = new CreateIngredientCommandValidator();
    }

    [Fact]
    public async Task ValidateAsync_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        CreateIngredientCommand command = new CreateIngredientCommand
        {
            Title = "Valid Ingredient Title",
            Description = "Valid ingredient description",
            Recipe = new( 1, "", "", 1, 1, "" ) { Id = 1 }
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
    }

    [Fact]
    public async Task ValidateAsync_EmptyTitle_ReturnsError()
    {
        // Arrange
        CreateIngredientCommand command = new CreateIngredientCommand
        {
            Title = string.Empty,
            Description = "Valid ingredient description",
            Recipe = new( 1, "", "", 1, 1, "" ) { Id = 1 }
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Название ингредиента не может быть пустым", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_TitleExceedsMaxLength_ReturnsError()
    {
        // Arrange
        CreateIngredientCommand command = new CreateIngredientCommand
        {
            Title = new string( 'A', 101 ), // 101 characters long
            Description = "Valid ingredient description",
            Recipe = new( 1, "", "", 1, 1, "" ) { Id = 1 }
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Название ингредиента не может быть больше чем 100 символов", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_EmptyDescription_ReturnsError()
    {
        // Arrange
        CreateIngredientCommand command = new CreateIngredientCommand
        {
            Title = "Valid Ingredient Title",
            Description = string.Empty,
            Recipe = new( 1, "", "", 1, 1, "" ) { Id = 1 }
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Описание ингредиента не может быть пустым", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_DescriptionExceedsMaxLength_ReturnsError()
    {
        // Arrange
        CreateIngredientCommand command = new CreateIngredientCommand
        {
            Title = "Valid Ingredient Title",
            Description = new string( 'A', 251 ), // 251 characters long
            Recipe = new( 1, "", "", 1, 1, "" ) { Id = 1}
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Описание ингредиента не может быть больше чем 250 символов", result.Error.Message );
    }
}