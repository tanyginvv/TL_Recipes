using Moq;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.CQRSInterfaces;
using Recipes.Domain.Entities;
using Recipes.Application.UseCases.Likes.Command.DeleteLike;
using Recipes.Application.Interfaces;

namespace Recipes.Application.Tests.Likes.Command.DeleteLike;

public class DeleteLikeCommandHandlerTests
{
    private readonly Mock<ILikeRepository> _mockLikeRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IAsyncValidator<DeleteLikeCommand>> _mockValidator;
    private readonly DeleteLikeCommandHandler _handler;

    public DeleteLikeCommandHandlerTests()
    {
        _mockLikeRepository = new Mock<ILikeRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockValidator = new Mock<IAsyncValidator<DeleteLikeCommand>>();
        _handler = new DeleteLikeCommandHandler(
            _mockLikeRepository.Object,
            _mockUnitOfWork.Object,
            _mockValidator.Object );
    }

    [Fact]
    public async Task HandleImplAsync_ExistingLike_DeletesLikeAndCommits()
    {
        // Arrange
        DeleteLikeCommand command = new DeleteLikeCommand { RecipeId = 1, UserId = 2 };
        Like like = new Like( command.RecipeId, command.UserId );

        _mockValidator.Setup( v => v.ValidateAsync( command ) )
                      .ReturnsAsync( Result.Success ); // Assume validation is successful
        _mockLikeRepository.Setup( r => r.GetLikeByAttributes( command.RecipeId, command.UserId ) )
                           .ReturnsAsync( like );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        _mockLikeRepository.Verify( r => r.Delete( It.Is<Like>( l => l.RecipeId == command.RecipeId && l.UserId == command.UserId ) ), Times.Once );
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Once );
        Assert.True( result.IsSuccess );
        Assert.Null( result.Error );
    }

    [Fact]
    public async Task HandleImplAsync_LikeNotFound_ReturnsError()
    {
        // Arrange
        DeleteLikeCommand command = new DeleteLikeCommand { RecipeId = 1, UserId = 2 };

        _mockValidator.Setup( v => v.ValidateAsync( command ) )
                      .ReturnsAsync( Result.Success ); // Assume validation is successful
        _mockLikeRepository.Setup( r => r.GetLikeByAttributes( command.RecipeId, command.UserId ) )
                           .ReturnsAsync( null as Like  ); // Simulate no like found
        _mockValidator.Setup( v => v.ValidateAsync(command)).ReturnsAsync( Result.FromError("Лайк не найден"));

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        _mockLikeRepository.Verify( r => r.Delete( It.IsAny<Like>() ), Times.Never ); // Ensure no delete is called
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Never ); // Ensure no commit is called
        Assert.False( result.IsSuccess );
        Assert.Equal( "Лайк не найден", result.Error.Message );
    }

    [Fact]
    public async Task HandleImplAsync_ExceptionThrown_ReturnsError()
    {
        // Arrange
        DeleteLikeCommand command = new DeleteLikeCommand { RecipeId = 1, UserId = 2 };
        Like like = new Like( command.RecipeId, command.UserId );

        _mockValidator.Setup( v => v.ValidateAsync( command ) )
                      .ReturnsAsync( Result.Success ); // Assume validation is successful
        _mockLikeRepository.Setup( r => r.GetLikeByAttributes( command.RecipeId, command.UserId ) )
                           .ReturnsAsync( like );
        _mockLikeRepository.Setup( r => r.Delete( It.IsAny<Like>() ) )
                           .ThrowsAsync( new System.Exception( "Repository error" ) ); // Simulate an exception

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Never ); // Ensure no commit is called
        Assert.False( result.IsSuccess );
        Assert.Equal( "Repository error", result.Error.Message );
    }
}