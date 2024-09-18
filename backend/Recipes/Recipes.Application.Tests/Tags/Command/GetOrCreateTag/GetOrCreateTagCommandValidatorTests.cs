using Recipes.Application.Results;
using Recipes.Application.UseCases.Tags.Commands.GetOrCreateTag;

namespace Recipes.Application.Tests.Tags.Command.GetOrCreateTag;

public class GetOrCreateTagCommandValidatorTests
{
    private readonly GetOrCreateTagCommandValidator _validator;

    public GetOrCreateTagCommandValidatorTests()
    {
        _validator = new GetOrCreateTagCommandValidator();
    }

    [Fact]
    public async Task ValidateAsync_EmptyName_ShouldReturnError()
    {
        // Arrange
        GetOrCreateTagCommand command = new GetOrCreateTagCommand { Name = "" };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Название тега не может быть пустым", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_NameTooLong_ShouldReturnError()
    {
        // Arrange
        GetOrCreateTagCommand command = new GetOrCreateTagCommand { Name = new string( 'a', 51 ) }; 

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Название тега не может быть больше 50 символов", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_ValidName_ShouldReturnSuccess()
    {
        // Arrange
        GetOrCreateTagCommand command = new GetOrCreateTagCommand { Name = "ValidTagName" };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
    }
}