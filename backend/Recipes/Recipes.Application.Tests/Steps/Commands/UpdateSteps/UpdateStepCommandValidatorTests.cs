using Recipes.Application.Results;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Application.UseCases.Steps.Commands.UpdateSteps;
using Recipes.Domain.Entities;

namespace Recipes.Application.Tests.Steps.Commands.UpdateSteps;

public class UpdateStepsCommandValidatorTests
{
    private readonly UpdateStepsCommandValidator _validator;

    public UpdateStepsCommandValidatorTests()
    {
        _validator = new UpdateStepsCommandValidator();
    }

    [Fact]
    public async Task ValidateAsync_Should_Return_Success_When_Valid_Command()
    {
        // Arrange
        UpdateStepsCommand command = new UpdateStepsCommand
        {
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1 },
            NewSteps = new List<StepDto>
            {
                new StepDto { StepNumber = 1, StepDescription = "Valid description" }
            }
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
    }

    [Fact]
    public async Task ValidateAsync_Should_Return_Error_When_Recipe_Is_Null()
    {
        // Arrange
        UpdateStepsCommand command = new UpdateStepsCommand
        {
            Recipe = null,
            NewSteps = new List<StepDto>
            {
                new StepDto { StepNumber = 1, StepDescription = "Valid description" }
            }
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Рецепт не может быть null.", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_Should_Return_Error_When_StepNumber_Is_Zero()
    {
        // Arrange
        UpdateStepsCommand command = new UpdateStepsCommand
        {
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1 },
            NewSteps = new List<StepDto>
            {
                new StepDto { StepNumber = 0, StepDescription = "Valid description" }
            }
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Номер шага должен быть больше нуля.", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_Should_Return_Error_When_StepDescription_Is_Empty()
    {
        // Arrange
        UpdateStepsCommand command = new UpdateStepsCommand
        {
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1 },
            NewSteps = new List<StepDto>
            {
                new StepDto { StepNumber = 1, StepDescription = "" }
            }
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Описание шага не может быть пустым.", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_Should_Return_Error_When_StepDescription_Is_Too_Long()
    {
        // Arrange
        UpdateStepsCommand command = new UpdateStepsCommand
        {
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1 },
            NewSteps = new List<StepDto>
            {
                new StepDto { StepNumber = 1, StepDescription = new string('x', 251) }
            }
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Описание шага не может быть больше чем 250 символов.", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_Should_Return_Error_When_StepNumbers_Are_Not_Unique()
    {
        // Arrange
        UpdateStepsCommand command = new UpdateStepsCommand
        {
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1 },
            NewSteps = new List<StepDto>
            {
                new StepDto { StepNumber = 1, StepDescription = "Description 1" },
                new StepDto { StepNumber = 1, StepDescription = "Description 2" } // Duplicate step number
            }
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Номера шагов должны быть уникальными.", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_Should_Return_Error_When_NewSteps_Is_Empty()
    {
        // Arrange
        UpdateStepsCommand command = new UpdateStepsCommand
        {
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1 },
            NewSteps = new List<StepDto>()
        };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Список шагов не может быть пустым.", result.Error.Message );
    }
}