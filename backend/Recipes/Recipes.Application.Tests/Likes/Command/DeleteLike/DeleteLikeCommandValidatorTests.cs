using Moq;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Likes.Command.DeleteLike;
using Recipes.Domain.Entities;

namespace Recipes.Application.Tests.Likes.Command.DeleteLike;

public class DeleteLikeCommandValidatorTests
{
    private readonly Mock<IRecipeRepository> _mockRecipeRepository;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<ILikeRepository> _mockLikeRepository;
    private readonly DeleteLikeCommandValidator _validator;

    public DeleteLikeCommandValidatorTests()
    {
        _mockRecipeRepository = new Mock<IRecipeRepository>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockLikeRepository = new Mock<ILikeRepository>();
        _validator = new DeleteLikeCommandValidator(
            _mockRecipeRepository.Object,
            _mockUserRepository.Object,
            _mockLikeRepository.Object );
    }

    [Fact]
    public async Task ValidateAsync_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        DeleteLikeCommand command = new DeleteLikeCommand { RecipeId = 1, UserId = 2 };

        _mockRecipeRepository.Setup( r => r.GetByIdAsync( command.RecipeId ) )
                             .ReturnsAsync( new Recipe( 1, "", "", 1, 1, "" ) ); 
        _mockUserRepository.Setup( u => u.GetByIdAsync( command.UserId ) )
                           .ReturnsAsync( new User( "", "", "" ) ); 
        _mockLikeRepository.Setup( l => l.GetLikeByAttributes( command.RecipeId, command.UserId ) )
                           .ReturnsAsync( new Like( command.RecipeId, command.UserId ) );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Null( result.Error );
    }

    [Fact]
    public async Task ValidateAsync_InvalidRecipe_ReturnsError()
    {
        // Arrange
        DeleteLikeCommand command = new DeleteLikeCommand { RecipeId = 1, UserId = 2 };

        _mockRecipeRepository.Setup( r => r.GetByIdAsync( command.RecipeId ) )
                             .ReturnsAsync( null as Recipe );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Рецепта с таким id не существует", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_InvalidUser_ReturnsError()
    {
        // Arrange
        DeleteLikeCommand command = new DeleteLikeCommand { RecipeId = 1, UserId = 2 };

        _mockRecipeRepository.Setup( r => r.GetByIdAsync( command.RecipeId ) )
                             .ReturnsAsync( new Recipe( 1, "", "", 1, 1, "" ) ); 
        _mockUserRepository.Setup( u => u.GetByIdAsync( command.UserId ) )
                           .ReturnsAsync( null as User ); 

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Пользователя с таким id не существует", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_LikeDoesNotExist_ReturnsError()
    {
        // Arrange
        DeleteLikeCommand command = new DeleteLikeCommand { RecipeId = 1, UserId = 2 };

        _mockRecipeRepository.Setup( r => r.GetByIdAsync( command.RecipeId ) )
                             .ReturnsAsync( new Recipe( 1, "", "", 1, 1, "" ) ); 
        _mockUserRepository.Setup( u => u.GetByIdAsync( command.UserId ) )
                           .ReturnsAsync( new User( "", "", "" ) ); 
        _mockLikeRepository.Setup( l => l.GetLikeByAttributes( command.RecipeId, command.UserId ) )
                           .ReturnsAsync( null as Like ); 

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Такого лайка не существует", result.Error.Message );
    }
}