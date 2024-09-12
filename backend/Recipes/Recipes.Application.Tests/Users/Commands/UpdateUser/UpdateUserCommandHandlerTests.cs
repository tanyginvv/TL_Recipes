using Moq;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Interfaces;
using Recipes.Application.PasswordHasher;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Users.Commands.UpdateUser;
using Recipes.Domain.Entities;

namespace Recipes.Application.Tests.Users.Commands.UpdateUser;

public class UpdateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<IAsyncValidator<UpdateUserCommand>> _mockValidator;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IPasswordHasher> _mockPasswordHasher;
    private readonly UpdateUserCommandHandler _handler;

    public UpdateUserCommandHandlerTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockValidator = new Mock<IAsyncValidator<UpdateUserCommand>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockPasswordHasher = new Mock<IPasswordHasher>();
        _handler = new UpdateUserCommandHandler(
            _mockUserRepository.Object,
            _mockValidator.Object,
            _mockUnitOfWork.Object,
            _mockPasswordHasher.Object
        );
    }

    [Fact]
    public async Task HandleAsync_UserNotFound_ReturnsError()
    {
        // Arrange
        UpdateUserCommand command = new UpdateUserCommand
        {
            Id = 1,
            Login = "test"
        };
        _mockUserRepository.Setup( r => r.GetByIdAsync( command.Id ) ).ReturnsAsync( null as User );
        _mockValidator.Setup( x => x.ValidateAsync( command ) ).ReturnsAsync( Result.FromError( "Пользователь не найден" ) );
        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Пользователь не найден", result.Error.Message );
    }

    [Fact]
    public async Task HandleAsync_UpdateUserDetails_ReturnsSuccess()
    {
        // Arrange
        UpdateUserCommand command = new UpdateUserCommand
        {
            Id = 1,
            Name = "Updated Name",
            Description = "Updated Description",
            Login = "updated_login",
            OldPassword = "old_password",
            NewPassword = "new_password"
        };

        User user = new User( "Old Name", "old_login", "old_password_hash" )
        {
            Id = command.Id,
            Description = "Old Description",
            PasswordHash = "old_password_hash"
        };

        _mockUserRepository.Setup( r => r.GetByIdAsync( command.Id ) ).ReturnsAsync( user );
        _mockPasswordHasher.Setup( p => p.GeneratePassword( command.NewPassword ) ).Returns( "new_password_hash" );
        _mockValidator.Setup( x => x.ValidateAsync( command ) ).ReturnsAsync( Result.Success );
        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Equal( "Updated Name", user.Name );
        Assert.Equal( "Updated Description", user.Description );
        Assert.Equal( "updated_login", user.Login );
        Assert.Equal( "new_password_hash", user.PasswordHash );

        _mockUserRepository.Verify( r => r.GetByIdAsync( command.Id ), Times.Once );
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Once );
    }

    [Fact]
    public async Task HandleAsync_UpdatePasswordOnly_ReturnsSuccess()
    {
        // Arrange
        UpdateUserCommand command = new UpdateUserCommand
        {
            Id = 1,
            OldPassword = "old_password",
            NewPassword = "new_password"
        };

        User user = new User( "Existing Name", "existing_login", "old_password_hash" )
        {
            Id = command.Id
        };

        _mockUserRepository.Setup( r => r.GetByIdAsync( command.Id ) ).ReturnsAsync( user );
        _mockPasswordHasher.Setup( p => p.GeneratePassword( command.NewPassword ) ).Returns( "new_password_hash" );
        _mockValidator.Setup( x => x.ValidateAsync( command ) ).ReturnsAsync( Result.Success );
        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Equal( "new_password_hash", user.PasswordHash );

        _mockUserRepository.Verify( r => r.GetByIdAsync( command.Id ), Times.Once );
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Once );
    }

    [Fact]
    public async Task HandleAsync_NoUpdates_ReturnsSuccess()
    {
        // Arrange
        UpdateUserCommand command = new UpdateUserCommand
        {
            Id = 1,
            Name = null,
            Description = null,
            Login = null,
            OldPassword = null,
            NewPassword = null
        };

        User user = new User( "Original Name", "original_login", "original_password_hash" )
        {
            Id = 1
        };

        _mockUserRepository.Setup( r => r.GetByIdAsync( command.Id ) ).ReturnsAsync( user );
        _mockValidator.Setup( x => x.ValidateAsync( command ) ).ReturnsAsync( Result.Success );
        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Equal( "Original Name", user.Name );
        Assert.Equal( "original_login", user.Login );
        Assert.Equal( "original_password_hash", user.PasswordHash );

        _mockUserRepository.Verify( r => r.GetByIdAsync( command.Id ), Times.Once );
        _mockUnitOfWork.Verify( u => u.CommitAsync(), Times.Once );
    }
}