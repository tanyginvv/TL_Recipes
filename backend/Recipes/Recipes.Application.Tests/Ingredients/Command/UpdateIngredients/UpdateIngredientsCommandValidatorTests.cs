using Recipes.Application.Results;
using Recipes.Application.UseCases.Ingredients.Commands.UpdateIngredients;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Domain.Entities;

namespace Recipes.Application.Tests.Ingredients.Command.UpdateIngredients;
public class UpdateIngredientsCommandValidatorTests
{
    private readonly UpdateIngredientsCommandValidator _validator;

    public UpdateIngredientsCommandValidatorTests()
    {
        _validator = new UpdateIngredientsCommandValidator();
    }

    [Fact]
    public async Task ValidateAsync_Should_Return_Error_When_Recipe_Is_Null()
    {
        // Arrange
        UpdateIngredientsCommand command = new UpdateIngredientsCommand
        {
            Recipe = null,
            NewIngredients = new List<IngredientDto>
            {
                new IngredientDto { Title = "Valid Title", Description = "Valid Description" }
            }
        };

        // Act
        Results.Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Рецепт не может быть null.", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_Should_Return_Error_When_Ingredient_Title_Is_Empty()
    {
        // Arrange
        UpdateIngredientsCommand command = new UpdateIngredientsCommand
        {
            Recipe = new Recipe( 1, "", "", 1, 1, "" ),
            NewIngredients = new List<IngredientDto>
            {
                new IngredientDto { Title = "", Description = "Valid Description" }
            }
        };

        // Act
        Results.Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Название ингредиента не может быть пустым.", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_Should_Return_Error_When_Ingredient_Title_Is_Too_Long()
    {
        // Arrange
        UpdateIngredientsCommand command = new UpdateIngredientsCommand
        {
            Recipe = new Recipe(1, "", "", 1, 1, ""),
            NewIngredients = new List<IngredientDto>
            {
                new IngredientDto { Title = new string('a', 101), Description = "Valid Description" }
            }
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Название ингредиента не может быть больше чем 100 символов.", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_Should_Return_Error_When_Ingredient_Description_Is_Empty()
    {
        // Arrange
        UpdateIngredientsCommand command = new UpdateIngredientsCommand
        {
            Recipe = new Recipe( 1, "", "", 1, 1, "" ),
            NewIngredients = new List<IngredientDto>
            {
                new IngredientDto { Title = "Valid Title", Description = "" }
            }
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Описание ингредиента не может быть пустым.", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_Should_Return_Error_When_Ingredient_Description_Is_Too_Long()
    {
        // Arrange
        UpdateIngredientsCommand command = new UpdateIngredientsCommand
        {
            Recipe = new Recipe(1, "", "", 1, 1, ""),
            NewIngredients = new List<IngredientDto>
            {
                new IngredientDto { Title = "Valid Title", Description = new string('a', 251) }
            }
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Описание ингредиента не может быть больше чем 250 символов.", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_Should_Return_Success_When_All_Ingredients_Are_Valid()
    {
        // Arrange
        UpdateIngredientsCommand command = new UpdateIngredientsCommand
        {
            Recipe = new Recipe(1, "", "", 1, 1, ""),
            NewIngredients = new List<IngredientDto>
            {
                new IngredientDto { Title = "Valid Title", Description = "Valid Description" }
            }
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
    }
}