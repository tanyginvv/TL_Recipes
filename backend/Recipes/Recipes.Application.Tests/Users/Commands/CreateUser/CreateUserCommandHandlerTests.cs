using Microsoft.Extensions.Logging;
using Moq;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Interfaces;
using Recipes.Application.PasswordHasher;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Users.Commands.CreateUser;
using Recipes.Domain.Entities;

namespace Recipes.Application.Tests.Users.Commands.CreateUser;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IAsyncValidator<CreateUserCommand>> _mockValidator;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly CreateUserCommandHandler _handler;
    private readonly Mock<ILogger<CreateUserCommand>> _loggerMock;

    public CreateUserCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockValidator = new Mock<IAsyncValidator<CreateUserCommand>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _loggerMock = new Mock<ILogger<CreateUserCommand>>();
        _handler = new CreateUserCommandHandler(
            _mockUserRepository.Object,
            _mockValidator.Object,
            _mockUnitOfWork.Object,
            _mockPasswordHasher.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task HandleAsync_ValidCommand_CreatesUserAndCommits()
    {
        // Arrange
        CreateUserCommand command = new CreateUserCommand
        {
            Name = "John Doe",
            Login = "johndoe",
            Password = "password123"
        };

        _mockValidator.Setup( v => v.ValidateAsync( command ) )
            .ReturnsAsync( Result.Success );

        _mockPasswordHasher.Setup( h => h.GeneratePassword( command.Password ) )
            .Returns( "hashed_password" );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        _mockUserRepository.Verify( r => r.AddAsync( It.Is<User>( u => u.Name == command.Name && u.Login == command.Login && u.PasswordHash == "hashed_password" ) ), Times.Once );
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Once );
        Assert.True( result.IsSuccess );
    }

    [Fact]
    public async Task HandleAsync_InvalidCommand_ReturnsError()
    {
        // Arrange
        CreateUserCommand command = new CreateUserCommand
        {
            Name = "John Doe",
            Login = "johndoe",
            Password = "password123"
        };

        _mockValidator.Setup( v => v.ValidateAsync( command ) )
            .ReturnsAsync( Result.FromError( "Validation error" ) );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        _mockUserRepository.Verify( r => r.AddAsync( It.IsAny<User>() ), Times.Never );
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Never );
        Assert.False( result.IsSuccess );
        Assert.Equal( "Validation error", result.Error.Message );
    }

    [Fact]
    public async Task HandleAsync_HashesPasswordCorrectly()
    {
        // Arrange
        CreateUserCommand command = new CreateUserCommand
        {
            Name = "John Doe",
            Login = "johndoe",
            Password = "password123"
        };

        _mockValidator.Setup( v => v.ValidateAsync( command ) )
            .ReturnsAsync( Result.Success );

        _mockPasswordHasher.Setup( h => h.GeneratePassword( command.Password ) )
            .Returns( "hashed_password" );

        // Act
        await _handler.HandleAsync( command );

        // Assert
        _mockPasswordHasher.Verify( h => h.GeneratePassword( command.Password ), Times.Once );
    }
}