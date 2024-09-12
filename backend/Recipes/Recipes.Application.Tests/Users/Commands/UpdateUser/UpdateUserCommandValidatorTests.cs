using Moq;
using Recipes.Application.PasswordHasher;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Users.Commands.UpdateUser;
using Recipes.Domain.Entities;

namespace Recipes.Application.Tests.Users.Commands.UpdateUser;

public class UpdateUserCommandValidatorTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly UpdateUserCommandValidator _validator;

    public UpdateUserCommandValidatorTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _validator = new UpdateUserCommandValidator( _mockUserRepository.Object, _mockPasswordHasher.Object );
    }

    [Fact]
    public async Task ValidateAsync_UserNotFound_ReturnsError()
    {
        // Arrange
        UpdateUserCommand command = new UpdateUserCommand { Id = 1 };
        _mockUserRepository.Setup( r => r.GetByIdAsync( command.Id ) ).ReturnsAsync( null as User );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Пользователь не найден.", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_InvalidOldPassword_ReturnsError()
    {
        // Arrange
        UpdateUserCommand command = new UpdateUserCommand { Id = 1, OldPassword = "wrong_password" };
        User user = new User( "Name", "login", "hashed_password" );
        _mockUserRepository.Setup( r => r.GetByIdAsync( command.Id ) ).ReturnsAsync( user );
        _mockPasswordHasher.Setup( p => p.VerifyPassword( command.OldPassword, user.PasswordHash ) ).Returns( false );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Введеный пароль неверный", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_NewLoginAlreadyUsed_ReturnsError()
    {
        // Arrange
        UpdateUserCommand command = new UpdateUserCommand { Id = 1, Login = "new_login" };
        User user = new User( "Name", "old_login", "hashed_password" ) { Id = 1 };
        User existingUser = new User( "Other Name", "new_login", "other_password" );
        _mockUserRepository.Setup( r => r.GetByIdAsync( command.Id ) ).ReturnsAsync( user );
        _mockUserRepository.Setup( r => r.GetByLoginAsync( command.Login ) ).ReturnsAsync( existingUser );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Новый логин уже используется другим пользователем.", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        UpdateUserCommand command = new UpdateUserCommand { Id = 1, Login = "new_login", OldPassword = "old_password", NewPassword = "new_password" };
        User user = new User( "Name", "old_login", "hashed_password" );
        _mockUserRepository.Setup( r => r.GetByIdAsync( command.Id ) ).ReturnsAsync( user );
        _mockPasswordHasher.Setup( p => p.VerifyPassword( command.OldPassword, user.PasswordHash ) ).Returns( true );
        _mockUserRepository.Setup( r => r.GetByLoginAsync( command.Login ) ).ReturnsAsync( null as User );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
    }
}