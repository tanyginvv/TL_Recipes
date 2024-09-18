using Moq;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Favourites.Command.DeleteFavourite;
using Recipes.Domain.Entities;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Interfaces;

namespace Recipes.Application.Tests.Favourites.Command.DeleteFavourite;

public class DeleteFavouriteCommandHandlerTests
{
    private readonly Mock<IFavouriteRepository> _mockFavouriteRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IAsyncValidator<DeleteFavouriteCommand>> _mockValidator;
    private readonly DeleteFavouriteCommandHandler _handler;

    public DeleteFavouriteCommandHandlerTests()
    {
        _mockFavouriteRepository = new Mock<IFavouriteRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockValidator = new Mock<IAsyncValidator<DeleteFavouriteCommand>>();
        _handler = new DeleteFavouriteCommandHandler(
            _mockFavouriteRepository.Object,
            _mockUnitOfWork.Object,
            _mockValidator.Object );
    }

    [Fact]
    public async Task HandleAsync_FavouriteNotFound_ReturnsError()
    {
        // Arrange
        DeleteFavouriteCommand command = new DeleteFavouriteCommand { RecipeId = 1, UserId = 2 };
        _mockValidator.Setup( v => v.ValidateAsync( command ) )
                      .ReturnsAsync( Result.Success );
        _mockFavouriteRepository.Setup( r => r.GetFavouriteByAttributes( command.RecipeId, command.UserId ) )
                                 .ReturnsAsync( null as Favourite );
        _mockValidator.Setup( v => v.ValidateAsync( command ) )
                      .ReturnsAsync( Result.FromError( "Избранное не найдено" ) );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Избранное не найдено", result.Error.Message );
    }

    [Fact]
    public async Task HandleAsync_FavouriteExists_SuccessfullyDeleted()
    {
        // Arrange
        DeleteFavouriteCommand command = new DeleteFavouriteCommand { RecipeId = 1, UserId = 2 };
        Favourite favourite = new Favourite( command.RecipeId, command.UserId );
        _mockValidator.Setup( v => v.ValidateAsync( command ) )
                      .ReturnsAsync( Result.Success );
        _mockFavouriteRepository.Setup( r => r.GetFavouriteByAttributes( command.RecipeId, command.UserId ) )
                                 .ReturnsAsync( favourite );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        _mockFavouriteRepository.Verify( r => r.Delete( favourite ), Times.Once );
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Once );
        Assert.True( result.IsSuccess );
        Assert.Null( result.Error );
    }
}
