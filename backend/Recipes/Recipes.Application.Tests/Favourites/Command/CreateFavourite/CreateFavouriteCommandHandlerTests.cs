using Moq;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Interfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Favourites.Command.CreateFavourite;
using Recipes.Domain.Entities;

namespace Recipes.Application.Tests.Favourites.Command.CreateFavourite;

public class CreateFavouriteCommandHandlerTests
{
    private readonly Mock<IFavouriteRepository> _mockFavouriteRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IAsyncValidator<CreateFavouriteCommand>> _mockValidator;
    private readonly CreateFavouriteCommandHandler _handler;

    public CreateFavouriteCommandHandlerTests()
    {
        _mockFavouriteRepository = new Mock<IFavouriteRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockValidator = new Mock<IAsyncValidator<CreateFavouriteCommand>>();
        _handler = new CreateFavouriteCommandHandler(
            _mockFavouriteRepository.Object,
            _mockUnitOfWork.Object,
            _mockValidator.Object );
    }

    [Fact]
    public async Task HandleAsync_ValidCommand_AddsFavouriteAndCommits()
    {
        // Arrange
        CreateFavouriteCommand command = new CreateFavouriteCommand { RecipeId = 1, UserId = 2 };
        _mockValidator.Setup( v => v.ValidateAsync( command ) )
                      .ReturnsAsync( Result.Success );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        _mockFavouriteRepository.Verify( r => r.AddAsync( It.Is<Favourite>( f => f.RecipeId == command.RecipeId && f.UserId == command.UserId ) ), Times.Once );
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Once );
        Assert.True( result.IsSuccess );
        Assert.Null( result.Error );
    }

    [Fact]
    public async Task HandleAsync_InvalidCommand_ReturnsValidationError()
    {
        // Arrange
        CreateFavouriteCommand command = new CreateFavouriteCommand { RecipeId = 1, UserId = 2 };
        _mockValidator.Setup( v => v.ValidateAsync( command ) )
                      .ReturnsAsync( Result.FromError( "Validation error" ) );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        _mockFavouriteRepository.Verify( r => r.AddAsync( It.IsAny<Favourite>() ), Times.Never );
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Never );
        Assert.False( result.IsSuccess );
        Assert.Equal( "Validation error", result.Error.Message );
    }
}