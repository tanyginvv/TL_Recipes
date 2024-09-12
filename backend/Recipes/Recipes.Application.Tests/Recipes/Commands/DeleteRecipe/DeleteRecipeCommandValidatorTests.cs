using Moq;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Recipes.Commands.DeleteRecipe;
using Recipes.Domain.Entities;

namespace Recipes.Application.Tests.Recipes.Commands.DeleteRecipe;

public class DeleteRecipeCommandValidatorTests
{
    private readonly Mock<IRecipeRepository> _mockRecipeRepository;
    private readonly IAsyncValidator<DeleteRecipeCommand> _validator;

    public DeleteRecipeCommandValidatorTests()
    {
        _mockRecipeRepository = new Mock<IRecipeRepository>();
        _validator = new DeleteRecipeCommandValidator( _mockRecipeRepository.Object );
    }

    [Fact]
    public async Task ValidateAsync_InvalidRecipeId_ReturnsError()
    {
        // Arrange
        DeleteRecipeCommand command = new DeleteRecipeCommand
        {
            RecipeId = 0, // Invalid ID
            AuthorId = 1
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "ID рецепта должно быть больше нуля", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_RecipeNotFound_ReturnsError()
    {
        // Arrange
        DeleteRecipeCommand command = new DeleteRecipeCommand
        {
            RecipeId = 1,
            AuthorId = 1
        };

        _mockRecipeRepository.Setup( repo => repo.GetByIdAsync( command.RecipeId ) )
            .ReturnsAsync( null as Recipe  );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Рецепт не найден", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_UserDoesNotOwnRecipe_ReturnsError()
    {
        // Arrange
        DeleteRecipeCommand command = new DeleteRecipeCommand
        {
            RecipeId = 1,
            AuthorId = 2
        };

        Recipe recipe = new Recipe( 1, "Name", "Description", 30, 4, "test_image_url" )
        {
            Id = 1,
            AuthorId = 1
        };

        _mockRecipeRepository.Setup( repo => repo.GetByIdAsync( command.RecipeId ) )
            .ReturnsAsync( recipe );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "У пользователя нет доступа к удалению данного рецепта", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        DeleteRecipeCommand command = new DeleteRecipeCommand
        {
            RecipeId = 1,
            AuthorId = 1
        };

        Recipe recipe = new Recipe( 1, "Name", "Description", 30, 4, "test_image_url" )
        {
            Id = 1,
            AuthorId = 1
        };

        _mockRecipeRepository.Setup( repo => repo.GetByIdAsync( command.RecipeId ) )
            .ReturnsAsync( recipe );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
    }
}