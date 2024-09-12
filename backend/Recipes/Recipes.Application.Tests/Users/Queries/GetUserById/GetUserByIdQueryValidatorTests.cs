using Moq;
using Xunit;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Users.Queries.GetUserById;
using Recipes.Domain.Entities;
using System.Linq.Expressions;

public class GetUserByIdQueryValidatorTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly GetUserByIdQueryValidator _validator;

    public GetUserByIdQueryValidatorTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _validator = new GetUserByIdQueryValidator( _mockUserRepository.Object );
    }

    [Fact]
    public async Task ValidateAsync_UserNotExists_ReturnsError()
    {
        // Arrange
        GetUserByIdQuery query = new GetUserByIdQuery { Id = 1 };
        _mockUserRepository.Setup( r => r.ContainsAsync( u => u.Id == query.Id ) ).ReturnsAsync( false );

        // Act
        Result result = await _validator.ValidateAsync( query );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Пользователя с таким id нет", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_UserExists_ReturnsSuccess()
    {
        // Arrange
        GetUserByIdQuery query = new GetUserByIdQuery { Id = 1 };
        _mockUserRepository.Setup( r => r.ContainsAsync( It.IsAny<Expression<Func<User, bool>>>() ) )
                           .ReturnsAsync( true );

        // Act
        Result result = await _validator.ValidateAsync( query );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Null( result.Error );
    }
}