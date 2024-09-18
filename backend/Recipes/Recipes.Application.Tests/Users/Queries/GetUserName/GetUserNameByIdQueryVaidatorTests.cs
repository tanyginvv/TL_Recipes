using Moq;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Users.Queries.GetUserNameById;
using Recipes.Domain.Entities;
using System.Linq.Expressions;

public class GetUserNameByIdQueryValidatorTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly GetUserNameByIdQueryValidator _validator;

    public GetUserNameByIdQueryValidatorTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _validator = new GetUserNameByIdQueryValidator( _mockUserRepository.Object );
    }

    [Fact]
    public async Task ValidateAsync_UserExists_ReturnsSuccess()
    {
        // Arrange
        GetUserNameByIdQuery query = new GetUserNameByIdQuery { Id = 1 };
        _mockUserRepository.Setup( r => r.ContainsAsync( It.IsAny<Expression<Func<User, bool>>>() ) )
                           .ReturnsAsync( true );

        // Act
        Result result = await _validator.ValidateAsync( query );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Null( result.Error );
    }

    [Fact]
    public async Task ValidateAsync_UserDoesNotExist_ReturnsError()
    {
        // Arrange
        GetUserNameByIdQuery query = new GetUserNameByIdQuery { Id = 1 };
        _mockUserRepository.Setup( r => r.ContainsAsync( It.IsAny<Expression<Func<User, bool>>>() ) )
                           .ReturnsAsync( false );

        // Act
        Result result = await _validator.ValidateAsync( query );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Пользователя с таким id нет", result.Error.Message );
    }
}