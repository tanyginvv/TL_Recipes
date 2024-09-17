using Moq;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Favourites.Command.CreateFavourite;
using Recipes.Domain.Entities;

namespace Recipes.Application.Tests.Favourites.Command;
public class CreateFavouriteCommandValidatorTests
{
    private readonly Mock<IRecipeRepository> _mockRecipeRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IFavouriteRepository> _mockFavouriteRepository;
    private readonly CreateFavouriteCommandValidator _validator;

    public CreateFavouriteCommandValidatorTests()
    {
        _mockRecipeRepository = new Mock<IRecipeRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockFavouriteRepository = new Mock<IFavouriteRepository>();
        _validator = new CreateFavouriteCommandValidator(
            _mockRecipeRepository.Object,
            _mockUserRepository.Object,
            _mockFavouriteRepository.Object );
    }

    [Fact]
    public async Task ValidateAsync_RecipeNotFound_ReturnsError()
    {
        // Arrange
        CreateFavouriteCommand command = new CreateFavouriteCommand { RecipeId = 1, UserId = 2 };
        _mockRecipeRepository.Setup( r => r.GetByIdAsync( command.RecipeId ) )
                             .ReturnsAsync( null as Recipe  );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Рецепта с таким id не существует", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_UserNotFound_ReturnsError()
    {
        // Arrange
        CreateFavouriteCommand command = new CreateFavouriteCommand { RecipeId = 1, UserId = 2 };
        _mockRecipeRepository.Setup( r => r.GetByIdAsync( command.RecipeId ) )
                             .ReturnsAsync( new Recipe( 1, "", "", 1, 1, "" ) ); // Assume recipe exists
        _mockUserRepository.Setup( u => u.GetByIdAsync( command.UserId ) )
                           .ReturnsAsync( null as User );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Пользователя с таким id не существует", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_FavouriteAlreadyExists_ReturnsError()
    {
        // Arrange
        CreateFavouriteCommand command = new CreateFavouriteCommand { RecipeId = 1, UserId = 2 };
        _mockRecipeRepository.Setup( r => r.GetByIdAsync( command.RecipeId ) )
                             .ReturnsAsync( new Recipe( 1, "", "", 1, 1, "" ) );
        _mockUserRepository.Setup( u => u.GetByIdAsync( command.UserId ) )
                           .ReturnsAsync( new User("","","") ); 
        _mockFavouriteRepository.Setup( f => f.GetFavouriteByAttributes( command.UserId, command.RecipeId ) )
                                 .ReturnsAsync( new Favourite( command.RecipeId, command.UserId ) );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Такое избранное уже существует", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        CreateFavouriteCommand command = new CreateFavouriteCommand { RecipeId = 1, UserId = 2 };
        _mockRecipeRepository.Setup( r => r.GetByIdAsync( command.RecipeId ) )
                             .ReturnsAsync( new Recipe( 1, "", "", 1, 1, "" ) ); // Assume recipe exists
        _mockUserRepository.Setup( u => u.GetByIdAsync( command.UserId ) )
                           .ReturnsAsync( new User("", "", "") ); // Assume user exists
        _mockFavouriteRepository.Setup( f => f.GetFavouriteByAttributes( command.UserId, command.RecipeId ) )
                                 .ReturnsAsync( null as Favourite );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Null( result.Error );
    }
}
