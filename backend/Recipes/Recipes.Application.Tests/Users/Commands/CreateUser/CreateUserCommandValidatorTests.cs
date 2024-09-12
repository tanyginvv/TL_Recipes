using Moq;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Users.Commands.CreateUser;

namespace Recipes.Application.Tests.Users.Commands.CreateUser;

public class CreateUserCommandValidatorTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly CreateUserCommandValidator _validator;

    public CreateUserCommandValidatorTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _validator = new CreateUserCommandValidator( _mockUserRepository.Object );
    }

    [Fact]
    public async Task ValidateAsync_LoginIsNullOrEmpty_ReturnsError()
    {
        // Arrange
        CreateUserCommand command = new CreateUserCommand { Login = string.Empty };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Логин не может быть пустым", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_LoginAlreadyExists_ReturnsError()
    {
        // Arrange
        CreateUserCommand command = new CreateUserCommand { Login = "existing_login" };
        _mockUserRepository.Setup( r => r.ContainsAsync( user => user.Login == command.Login ) )
            .ReturnsAsync( true );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Пользователь с таким логином уже существует", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_ValidCommand_ReturnsSuccess()
    {
        // Arrange
        CreateUserCommand command = new CreateUserCommand { Login = "new_login" };
        _mockUserRepository.Setup( r => r.ContainsAsync( user => user.Login == command.Login ) )
            .ReturnsAsync( false );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
    }
}