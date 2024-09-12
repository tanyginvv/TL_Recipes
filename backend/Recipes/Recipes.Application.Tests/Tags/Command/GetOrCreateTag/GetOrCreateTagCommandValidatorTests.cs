using Moq;
using System.Threading.Tasks;
using Xunit;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Tags.Commands.GetOrCreateTag;

public class GetOrCreateTagCommandValidatorTests
{
    private readonly GetOrCreateTagCommandValidator _validator;

    public GetOrCreateTagCommandValidatorTests()
    {
        _validator = new GetOrCreateTagCommandValidator();
    }

    [Fact]
    public async Task ValidateAsync_Should_Return_Error_When_Name_Is_Empty()
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
    public async Task ValidateAsync_Should_Return_Error_When_Name_Is_Too_Long()
    {
        // Arrange
        GetOrCreateTagCommand command = new GetOrCreateTagCommand { Name = new string( 'a', 51 ) }; // 51 символ

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Название тега не может быть больше 50 символов", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_Should_Return_Success_When_Name_Is_Valid()
    {
        // Arrange
        GetOrCreateTagCommand command = new GetOrCreateTagCommand { Name = "ValidTagName" };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
    }
}