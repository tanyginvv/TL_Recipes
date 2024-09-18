using Moq;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Steps.Commands.UpdateStep;
using Recipes.Domain.Entities;

namespace Recipes.Application.Tests.Steps.Commands.UpdateStep;

public class UpdateStepCommandHandlerTests
{
    private readonly Mock<IStepRepository> _stepRepositoryMock;
    private readonly Mock<IAsyncValidator<UpdateStepCommand>> _validatorMock;
    private readonly UpdateStepCommandHandler _handler;

    public UpdateStepCommandHandlerTests()
    {
        _stepRepositoryMock = new Mock<IStepRepository>();
        _validatorMock = new Mock<IAsyncValidator<UpdateStepCommand>>();
        _handler = new UpdateStepCommandHandler( _stepRepositoryMock.Object, _validatorMock.Object );
    }

    [Fact]
    public async Task HandleImplAsync_ShouldReturnError_WhenStepNotFound()
    {
        // Arrange
        UpdateStepCommand command = new UpdateStepCommand { StepId = 1, StepNumber = 2, StepDescription = "Updated description" };
        _stepRepositoryMock.Setup( repo => repo.GetByStepIdAsync( command.StepId ) )
            .ReturnsAsync( null as Step );
        _validatorMock.Setup( r => r.ValidateAsync( command ) ).ReturnsAsync( Result.FromError( "Шаг не найден или не относится к указанному рецепту." ) );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Шаг не найден или не относится к указанному рецепту.", result.Error.Message );
    }

    [Fact]
    public async Task HandleImplAsync_ShouldReturnError_WhenStepIdDoesNotMatch()
    {
        // Arrange
        UpdateStepCommand command = new UpdateStepCommand { StepId = 1, StepNumber = 2, StepDescription = "Updated description" };
        Step step = new Step( 2, "Original description", 1 ); 
        _stepRepositoryMock.Setup( repo => repo.GetByStepIdAsync( command.StepId ) )
            .ReturnsAsync( step );
        _validatorMock.Setup( r => r.ValidateAsync( command ) ).ReturnsAsync( Result.FromError( "Шаг не найден или не относится к указанному рецепту." ) );
        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Шаг не найден или не относится к указанному рецепту.", result.Error.Message );
    }

    [Fact]
    public async Task HandleImplAsync_ShouldReturnSuccess_WhenStepIsUpdated()
    {
        // Arrange
        UpdateStepCommand command = new UpdateStepCommand { StepId = 1, StepNumber = 2, StepDescription = "Updated description" };
        Step step = new Step( command.StepId, "Original description", 1 ) { Id = 1 }; 
        _stepRepositoryMock.Setup( repo => repo.GetByStepIdAsync( command.StepId ) )
            .ReturnsAsync( step );
        _validatorMock.Setup( r => r.ValidateAsync( command ) ).ReturnsAsync( Result.FromSuccess );
        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.Equal( command.StepNumber, step.StepNumber );
        Assert.Equal( command.StepDescription, step.StepDescription );
    }
}