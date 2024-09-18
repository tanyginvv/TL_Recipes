using Microsoft.Extensions.Logging;
using Moq;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Interfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Recipes.Commands.DeleteRecipe;
using Recipes.Domain.Entities;

namespace Recipes.Application.Tests.Recipes.Commands.DeleteRecipe;

public class DeleteRecipeCommandHandlerTests
{
    private readonly Mock<IRecipeRepository> _mockRecipeRepository;
    private readonly Mock<IAsyncValidator<DeleteRecipeCommand>> _mockValidator;
    private readonly Mock<IImageTools> _mockImageTools;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly DeleteRecipeCommandHandler _handler;
    private readonly Mock<ILogger<DeleteRecipeCommand>> _loggerMock;

    public DeleteRecipeCommandHandlerTests()
    {
        _mockRecipeRepository = new Mock<IRecipeRepository>();
        _mockValidator = new Mock<IAsyncValidator<DeleteRecipeCommand>>();
        _mockImageTools = new Mock<IImageTools>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<DeleteRecipeCommand>>();

        _handler = new DeleteRecipeCommandHandler(
            _mockRecipeRepository.Object,
            _mockValidator.Object,
            _mockUnitOfWork.Object,
            _mockImageTools.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task HandleAsync_RecipeNotFound_ReturnsError()
    {
        // Arrange
        DeleteRecipeCommand command = new DeleteRecipeCommand
        {
            RecipeId = 14,
            AuthorId = 1
        };

        _mockValidator.Setup( v => v.ValidateAsync( command ) )
            .ReturnsAsync( Result.FromError( "Рецепт не найден" ) );
        _mockRecipeRepository.Setup( r => r.GetByIdAsync( command.RecipeId ) )
            .ReturnsAsync( null as Recipe );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Рецепт не найден", result.Error.Message );
        _mockRecipeRepository.Verify( r => r.Delete( It.IsAny<Recipe>() ), Times.Never );
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Never );
        _mockImageTools.Verify( i => i.DeleteImage( It.IsAny<string>() ), Times.Never );
    }

    [Fact]
    public async Task HandleAsync_ValidCommand_DeletesRecipe()
    {
        // Arrange
        DeleteRecipeCommand command = new DeleteRecipeCommand
        {
            RecipeId = 1,
            AuthorId = 1
        };

        Recipe recipe = new Recipe( 1, "Name", "Description", 30, 4, "test_image_url" )
        {
            Id = 1
        };

        _mockValidator.Setup( v => v.ValidateAsync( command ) )
            .ReturnsAsync( Result.Success );
        _mockRecipeRepository.Setup( r => r.GetByIdAsync( command.RecipeId ) )
            .ReturnsAsync( recipe );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
        _mockRecipeRepository.Verify( r => r.Delete( recipe ), Times.Once );
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Once );
        _mockImageTools.Verify( i => i.DeleteImage( recipe.ImageUrl ), Times.Once );
    }
}