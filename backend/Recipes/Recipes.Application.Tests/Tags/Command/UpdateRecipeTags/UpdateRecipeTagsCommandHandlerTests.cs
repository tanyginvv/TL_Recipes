﻿using Moq;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Tags.Commands.GetOrCreateTag;
using Recipes.Application.UseCases.Tags.Commands.UpdateRecipeTags;
using Recipes.Domain.Entities;
using Recipes.Application.UseCases.Recipes.Dtos;
using Microsoft.Extensions.Logging;

namespace Recipes.Application.Tests.Tags.Command.UpdateRecipeTags;

public class UpdateTagsCommandHandlerTests
{
    private readonly Mock<IRecipeRepository> _recipeRepositoryMock;
    private readonly Mock<ITagRepository> _tagRepositoryMock;
    private readonly Mock<ICommandHandlerWithResult<GetOrCreateTagCommand, Tag>> _createTagCommandHandlerMock;
    private readonly Mock<IAsyncValidator<UpdateTagsCommand>> _validatorMock;
    private readonly UpdateTagsCommandHandler _handler;
    private readonly Mock<ILogger<UpdateTagsCommand>> _logger;

    public UpdateTagsCommandHandlerTests()
    {
        _recipeRepositoryMock = new Mock<IRecipeRepository>();
        _tagRepositoryMock = new Mock<ITagRepository>();
        _createTagCommandHandlerMock = new Mock<ICommandHandlerWithResult<GetOrCreateTagCommand, Tag>>();
        _validatorMock = new Mock<IAsyncValidator<UpdateTagsCommand>>();
        _logger = new Mock<ILogger<UpdateTagsCommand>>();

        _handler = new UpdateTagsCommandHandler(
            _recipeRepositoryMock.Object,
            _tagRepositoryMock.Object,
            _createTagCommandHandlerMock.Object,
            _validatorMock.Object,
            _logger.Object
        );
    }

    [Fact]
    public async Task HandleAsync_RecipeNotFound_ShouldReturnError()
    {
        // Arrange
        UpdateTagsCommand command = new UpdateTagsCommand { RecipeId = 1, RecipeTags = new List<TagDto>() };
        _recipeRepositoryMock.Setup( repo => repo.GetByIdAsync( command.RecipeId ) ).ReturnsAsync( null as Recipe );
        _validatorMock.Setup( r => r.ValidateAsync( command ) ).ReturnsAsync( Result.FromError( "Рецепт не найден" ) );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Рецепт не найден", result.Error.Message );
    }

    [Fact]
    public async Task HandleAsync_ShouldRemoveOldTagsAndAddNewTags()
    {
        // Arrange
        Recipe recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1, Tags = new List<Tag> { new Tag( "OldTag" ) } };
        UpdateTagsCommand command = new UpdateTagsCommand
        {
            RecipeId = recipe.Id,
            RecipeTags = new List<TagDto> { new TagDto { Name = "NewTag" } }
        };

        Tag newTag = new Tag( "NewTag" );
        _recipeRepositoryMock.Setup( repo => repo.GetByIdAsync( recipe.Id ) ).ReturnsAsync( recipe );
        _tagRepositoryMock.Setup( repo => repo.GetByNameAsync( "NewTag" ) ).ReturnsAsync( null as Tag );
        _createTagCommandHandlerMock.Setup( handler => handler.HandleAsync( It.IsAny<GetOrCreateTagCommand>() ) )
            .ReturnsAsync( Result<Tag>.FromSuccess( newTag ) );
        _validatorMock.Setup( r => r.ValidateAsync( command ) ).ReturnsAsync( Result.FromSuccess );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Single( recipe.Tags ); 
        Assert.DoesNotContain( recipe.Tags, tag => tag.Name == "OldTag" );
    }

    [Fact]
    public async Task HandleAsync_ShouldAddNewTagsWithoutDuplicates()
    {
        // Arrange
        Tag existingTag = new Tag( "ExistingTag" );
        Recipe recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1, Tags = new List<Tag> { existingTag } };
        UpdateTagsCommand command = new UpdateTagsCommand
        {
            RecipeId = recipe.Id,
            RecipeTags = new List<TagDto> { new TagDto { Name = "ExistingTag" }, new TagDto { Name = "NewTag" } }
        };

        Tag newTag = new Tag( "NewTag" );
        _recipeRepositoryMock.Setup( repo => repo.GetByIdAsync( recipe.Id ) ).ReturnsAsync( recipe );
        _tagRepositoryMock.Setup( repo => repo.GetByNameAsync( "ExistingTag" ) ).ReturnsAsync( existingTag );
        _tagRepositoryMock.Setup( repo => repo.GetByNameAsync( "NewTag" ) ).ReturnsAsync( null as Tag );
        _createTagCommandHandlerMock.Setup( handler => handler.HandleAsync( It.IsAny<GetOrCreateTagCommand>() ) )
            .ReturnsAsync( Result<Tag>.FromSuccess( newTag ) );
        _validatorMock.Setup( r => r.ValidateAsync( command ) ).ReturnsAsync( Result.FromSuccess );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Contains( recipe.Tags, tag => tag.Name == "ExistingTag" );
    }
}