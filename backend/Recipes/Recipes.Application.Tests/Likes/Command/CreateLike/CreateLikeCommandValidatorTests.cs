using Moq;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Likes.Command.CreateLike;
using Recipes.Domain.Entities;

namespace Recipes.Application.Tests.Likes.Command.CreateLike;

public class CreateLikeCommandValidatorTests
{
    private readonly Mock<IRecipeRepository> _mockRecipeRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<ILikeRepository> _mockLikeRepository;
    private readonly CreateLikeCommandValidator _validator;

    public CreateLikeCommandValidatorTests()
    {
        _mockRecipeRepository = new Mock<IRecipeRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockLikeRepository = new Mock<ILikeRepository>();
        _validator = new CreateLikeCommandValidator(
            _mockRecipeRepository.Object,
            _mockUserRepository.Object,
            _mockLikeRepository.Object );
    }

    [Fact]
    public async Task ValidateAsync_RecipeDoesNotExist_ReturnsError()
    {
        // Arrange
        CreateLikeCommand command = new CreateLikeCommand { RecipeId = 1, UserId = 2 };
        _mockRecipeRepository.Setup( r => r.GetByIdAsync( command.RecipeId ) )
                             .ReturnsAsync( null as Recipe );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Рецепта с таким id не существует", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_UserDoesNotExist_ReturnsError()
    {
        // Arrange
        CreateLikeCommand command = new CreateLikeCommand { RecipeId = 1, UserId = 2 };
        _mockRecipeRepository.Setup( r => r.GetByIdAsync( command.RecipeId ) )
                             .ReturnsAsync( new Recipe(1, "", "", 1, 1, "") ); // Recipe exists
        _mockUserRepository.Setup( r => r.GetByIdAsync( command.UserId ) )
                           .ReturnsAsync( null as User );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Пользователя с таким id не существует", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_LikeAlreadyExists_ReturnsError()
    {
        // Arrange
        CreateLikeCommand command = new CreateLikeCommand { RecipeId = 1, UserId = 1 };
        _mockRecipeRepository.Setup( r => r.GetByIdAsync( command.RecipeId ) )
                             .ReturnsAsync( new Recipe(1, "", "", 1, 1, "") ); // Recipe exists
        _mockUserRepository.Setup( r => r.GetByIdAsync( command.UserId ) )
                           .ReturnsAsync( new User( "", "", "" ) ); // User exists
        _mockLikeRepository.Setup( r => r.GetLikeByAttributes( command.UserId, command.RecipeId ) )
                                 .ReturnsAsync( new Like( command.RecipeId, command.UserId ) ); // Like already exists
        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Такой лайк уже существует", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        CreateLikeCommand command = new CreateLikeCommand { RecipeId = 1, UserId = 2 };
        _mockRecipeRepository.Setup( r => r.GetByIdAsync( command.RecipeId ) )
                             .ReturnsAsync( new Recipe(1, "", "", 1, 1, "") ); // Recipe exists
        _mockUserRepository.Setup( r => r.GetByIdAsync( command.UserId ) )
                           .ReturnsAsync( new User( "", "", "" ) ); // User exists
        _mockLikeRepository.Setup( r => r.GetLikeByAttributes( command.UserId, command.RecipeId ) )
                           .ReturnsAsync( null as Like ); // Like does not exist

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Null( result.Error );
    }
}