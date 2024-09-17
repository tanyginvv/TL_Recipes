using Moq;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Steps.Commands.DeleteStep;
using Recipes.Domain.Entities;

namespace Recipes.Application.Tests.Steps.Commands.DeleteStep;

public class DeleteStepCommandValidatorTests
{
    private readonly Mock<IStepRepository> _stepRepositoryMock;
    private readonly DeleteStepCommandValidator _validator;

    public DeleteStepCommandValidatorTests()
    {
        _stepRepositoryMock = new Mock<IStepRepository>();
        _validator = new DeleteStepCommandValidator( _stepRepositoryMock.Object );
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnError_WhenStepNotFound()
    {
        // Arrange
        DeleteStepCommand command = new DeleteStepCommand { StepId = 1 };
        _stepRepositoryMock.Setup( repo => repo.GetByStepIdAsync( command.StepId ) )
            .ReturnsAsync( null as Step );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Шаг не найден", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnError_WhenStepIdDoesNotMatch()
    {
        // Arrange
        DeleteStepCommand command = new DeleteStepCommand { StepId = 1 };
        Step step = new Step( 2, "Description", 1 ); // Assume this is a valid step with ID 2
        _stepRepositoryMock.Setup( repo => repo.GetByStepIdAsync( command.StepId ) )
            .ReturnsAsync( step );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "ID шага не соответствует указанному номеру шага", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_ShouldReturnSuccess_WhenStepIdIsValid()
    {
        // Arrange
        DeleteStepCommand command = new DeleteStepCommand { StepId = 1 };
        Step step = new Step( 1, "Description", 1 ) { Id = 1 }; // Assume this is a valid step with ID 1
        _stepRepositoryMock.Setup( repo => repo.GetByStepIdAsync( command.StepId ) )
            .ReturnsAsync( step );

        // Act
        Result result = await _validator.ValidateAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
    }
}