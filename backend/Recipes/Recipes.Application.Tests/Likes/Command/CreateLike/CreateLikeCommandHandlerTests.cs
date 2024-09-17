using Moq;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.CQRSInterfaces;
using Recipes.Domain.Entities;
using Recipes.Application.UseCases.Likes.Command.CreateLike;
using Recipes.Application.Interfaces;

namespace Recipes.Application.Tests.Likes.Command.CreateLike;

public class CreateLikeCommandHandlerTests
{
    private readonly Mock<ILikeRepository> _mockLikeRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IAsyncValidator<CreateLikeCommand>> _mockValidator;
    private readonly CreateLikeCommandHandler _handler;

    public CreateLikeCommandHandlerTests()
    {
        _mockLikeRepository = new Mock<ILikeRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockValidator = new Mock<IAsyncValidator<CreateLikeCommand>>();
        _handler = new CreateLikeCommandHandler(
            _mockLikeRepository.Object,
            _mockUnitOfWork.Object,
            _mockValidator.Object );
    }

    [Fact]
    public async Task HandleImplAsync_ValidCommand_AddsLikeAndCommits()
    {
        // Arrange
        CreateLikeCommand command = new CreateLikeCommand { RecipeId = 1, UserId = 2 };
        _mockValidator.Setup( v => v.ValidateAsync( command ) )
                      .ReturnsAsync( Result.Success ); // Assume validation is successful

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        _mockLikeRepository.Verify( r => r.AddAsync( It.Is<Like>( l => l.RecipeId == command.RecipeId && l.UserId == command.UserId ) ), Times.Once );
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Once );
        Assert.True( result.IsSuccess );
        Assert.Null( result.Error );
    }

    [Fact]
    public async Task HandleImplAsync_InvalidCommand_ReturnsValidationError()
    {
        // Arrange
        CreateLikeCommand command = new CreateLikeCommand { RecipeId = 1, UserId = 2 };
        _mockValidator.Setup( v => v.ValidateAsync( command ) )
                      .ReturnsAsync( Result.FromError( "Validation error" ) );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        _mockLikeRepository.Verify( r => r.AddAsync( It.IsAny<Like>() ), Times.Never ); // Ensure no like is added
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Never ); // Ensure no commit is called
        Assert.False( result.IsSuccess );
        Assert.Equal( "Validation error", result.Error.Message );
    }

    [Fact]
    public async Task HandleImplAsync_ExceptionThrown_ReturnsError()
    {
        // Arrange
        CreateLikeCommand command = new CreateLikeCommand { RecipeId = 1, UserId = 2 };
        _mockValidator.Setup( v => v.ValidateAsync( command ) )
                      .ReturnsAsync( Result.Success ); // Assume validation is successful

        _mockLikeRepository.Setup( r => r.AddAsync( It.IsAny<Like>() ) )
                           .ThrowsAsync( new System.Exception( "Repository error" ) ); // Simulate an exception

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Never ); // Ensure no commit is called
        Assert.False( result.IsSuccess );
        Assert.Equal( "Repository error", result.Error.Message );
    }
}