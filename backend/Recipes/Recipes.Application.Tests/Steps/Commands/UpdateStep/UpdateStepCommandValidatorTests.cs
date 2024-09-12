using Moq;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Steps.Commands.UpdateStep;
using Recipes.Domain.Entities;

namespace Recipes.Application.Tests.UseCases.Steps.Commands;

public class UpdateStepCommandValidatorTests
{
    private readonly Mock<IStepRepository> _stepRepositoryMock;
    private readonly UpdateStepCommandValidator _validator;

    public UpdateStepCommandValidatorTests()
    {
        _stepRepositoryMock = new Mock<IStepRepository>();
        _validator = new UpdateStepCommandValidator( _stepRepositoryMock.Object );
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnError_WhenStepIdIsZeroOrNegative()
    {
        // Arrange
        UpdateStepCommand command = new UpdateStepCommand { StepId = 0, StepNumber = 1, StepDescription = "Description" };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "ID шага должен быть больше нуля", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnError_WhenStepNumberIsZeroOrNegative()
    {
        // Arrange
        UpdateStepCommand command = new UpdateStepCommand { StepId = 1, StepNumber = 0, StepDescription = "Description" };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Номер шага должен быть больше нуля", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnError_WhenStepDescriptionIsEmpty()
    {
        // Arrange
        UpdateStepCommand command = new UpdateStepCommand { StepId = 1, StepNumber = 1, StepDescription = string.Empty };

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Описание шага не может быть пустым", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnError_WhenStepNotFound()
    {
        // Arrange
        UpdateStepCommand command = new UpdateStepCommand { StepId = 1, StepNumber = 1, StepDescription = "Description" };
        _stepRepositoryMock.Setup( repo => repo.GetByStepIdAsync( command.StepId ) )
            .ReturnsAsync( null as Step  );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Шаг не найден или не относится к указанному рецепту.", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnSuccess_WhenAllConditionsAreMet()
    {
        // Arrange
        UpdateStepCommand command = new UpdateStepCommand { StepId = 1, StepNumber = 1, StepDescription = "Description" };
        Step step = new Step( command.StepNumber, command.StepDescription, 1 ) { Id = 1}; // Assuming a valid step
        _stepRepositoryMock.Setup( repo => repo.GetByStepIdAsync( command.StepId ) )
            .ReturnsAsync( step );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
    }
}