using Recipes.Application.Results;
using Recipes.Application.UseCases.Steps.Commands.CreateStep;
using Recipes.Domain.Entities;

namespace Recipes.Application.Tests.Steps.Commands.CreateStep;

public class CreateStepCommandValidatorTests
{
    private readonly CreateStepCommandValidator _validator;

    public CreateStepCommandValidatorTests()
    {
        _validator = new CreateStepCommandValidator();
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnError_WhenStepDescriptionIsNullOrEmpty()
    {
        // Arrange
        CreateStepCommand commandWithEmptyDescription = new CreateStepCommand
        {
            StepDescription = string.Empty,
            StepNumber = 1,
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1 }
        };

        CreateStepCommand commandWithNullDescription = new CreateStepCommand
        {
            StepDescription = null,
            StepNumber = 1,
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1 }
        };

        // Act
        Result resultEmptyDescription = await _validator.ValidateAsync( commandWithEmptyDescription );
        Result resultNullDescription = await _validator.ValidateAsync( commandWithNullDescription );

        // Assert
        Assert.False( resultEmptyDescription.IsSuccess );
        Assert.Equal( "Описание шага не может быть пустым", resultEmptyDescription.Error.Message );

        Assert.False( resultNullDescription.IsSuccess );
        Assert.Equal( "Описание шага не может быть пустым", resultNullDescription.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnError_WhenStepNumberIsLessThanOrEqualToZero()
    {
        // Arrange
        CreateStepCommand commandWithInvalidNumber = new CreateStepCommand
        {
            StepDescription = "Valid description",
            StepNumber = 0,
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1 }
        };

        CreateStepCommand commandWithNegativeNumber = new CreateStepCommand
        {
            StepDescription = "Valid description",
            StepNumber = -1,
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1 }
        };

        // Act
        Result resultInvalidNumber = await _validator.ValidateAsync( commandWithInvalidNumber );
        Result resultNegativeNumber = await _validator.ValidateAsync( commandWithNegativeNumber );

        // Assert
        Assert.False( resultInvalidNumber.IsSuccess );
        Assert.Equal( "Номер шага не может быть меньше или равен нулю", resultInvalidNumber.Error.Message );

        Assert.False( resultNegativeNumber.IsSuccess );
        Assert.Equal( "Номер шага не может быть меньше или равен нулю", resultNegativeNumber.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnSuccess_WhenCommandIsValid()
    {
        // Arrange
        CreateStepCommand command = new CreateStepCommand
        {
            StepDescription = "Valid description",
            StepNumber = 1,
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1 }
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
    }
}