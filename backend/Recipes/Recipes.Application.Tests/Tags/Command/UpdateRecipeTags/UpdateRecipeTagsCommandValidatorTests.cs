using Moq;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Tags.Commands.UpdateRecipeTags;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Domain.Entities;

namespace Recipes.Application.Tests;

public class UpdateTagsCommandValidatorTests
{
    private readonly Mock<IRecipeRepository> _recipeRepositoryMock;
    private readonly UpdateTagsCommandValidator _validator;

    public UpdateTagsCommandValidatorTests()
    {
        _recipeRepositoryMock = new Mock<IRecipeRepository>();
        _validator = new UpdateTagsCommandValidator( _recipeRepositoryMock.Object );
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnError_WhenRecipeTagsIsNull()
    {
        // Arrange
        UpdateTagsCommand command = new UpdateTagsCommand
        {
            RecipeId = 1,
            RecipeTags = null
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Список тегов не может быть пустым", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnError_WhenRecipeDoesNotExist()
    {
        // Arrange
        UpdateTagsCommand command = new UpdateTagsCommand
        {
            RecipeId = 1,
            RecipeTags = new List<TagDto> { new TagDto { Name = "Tag" } }
        };

        _recipeRepositoryMock
            .Setup( repo => repo.GetByIdAsync( It.IsAny<int>() ) )
            .ReturnsAsync( null as Recipe  );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Рецепт с указанным ID не найден", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnError_WhenTagNameIsTooLong()
    {
        // Arrange
        UpdateTagsCommand command = new UpdateTagsCommand
        {
            RecipeId = 1,
            RecipeTags = new List<TagDto> { new TagDto { Name = new string( 'a', 51 ) } }
        };

        _recipeRepositoryMock
            .Setup( repo => repo.GetByIdAsync( It.IsAny<int>() ) )
            .ReturnsAsync( new Recipe( 1, "", "", 1, 1, "" ) { Id = 1 } );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Название тега не может быть больше 50 символов", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnError_WhenTagNameIsEmpty()
    {
        // Arrange
        UpdateTagsCommand command = new UpdateTagsCommand
        {
            RecipeId = 1,
            RecipeTags = new List<TagDto> { new TagDto { Name = "" } }
        };

        _recipeRepositoryMock
            .Setup( repo => repo.GetByIdAsync( It.IsAny<int>() ) )
            .ReturnsAsync( new Recipe( 1, "", "", 1, 1, "" ) { Id = 1 } );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Название тега не может быть пустым", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnSuccess_WhenDataIsValid()
    {
        // Arrange
        UpdateTagsCommand command = new UpdateTagsCommand
        {
            RecipeId = 1,
            RecipeTags = new List<TagDto> { new TagDto { Name = "ValidTag" } }
        };

        _recipeRepositoryMock
            .Setup( repo => repo.GetByIdAsync( It.IsAny<int>() ) )
            .ReturnsAsync( new Recipe( 1, "", "", 1, 1, "" ) { Id = 1 } );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
    }
}