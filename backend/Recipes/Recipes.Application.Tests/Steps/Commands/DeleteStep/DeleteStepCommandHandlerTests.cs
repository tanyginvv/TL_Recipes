using Microsoft.Extensions.Logging;
using Moq;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Steps.Commands.DeleteStep;
using Recipes.Domain.Entities;

namespace Recipes.Application.Tests.Steps.Commands.DeleteStep;

public class DeleteStepCommandHandlerTests
{
    private readonly Mock<IStepRepository> _stepRepositoryMock;
    private readonly Mock<IAsyncValidator<DeleteStepCommand>> _validatorMock;
    private readonly DeleteStepCommandHandler _handler;
    private readonly Mock<ILogger<DeleteStepCommand>> _loggerMock;

    public DeleteStepCommandHandlerTests()
    {
        _stepRepositoryMock = new Mock<IStepRepository>();
        _validatorMock = new Mock<IAsyncValidator<DeleteStepCommand>>();
        _loggerMock = new Mock<ILogger<DeleteStepCommand>>();
        _handler = new DeleteStepCommandHandler( _stepRepositoryMock.Object, _validatorMock.Object, _loggerMock.Object );
    }

    [Fact]
    public async Task HandleImplAsync_ShouldReturnError_WhenStepNotFound()
    {
        // Arrange
        DeleteStepCommand command = new DeleteStepCommand { StepId = 1 };
        _stepRepositoryMock.Setup( repo => repo.GetByStepIdAsync( command.StepId ) )
            .ReturnsAsync( null as Step );
        _validatorMock.Setup( r => r.ValidateAsync( command ) ).ReturnsAsync( Result.FromError( "Шаг не найден" ) );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Шаг не найден", result.Error.Message );
    }

    [Fact]
    public async Task HandleImplAsync_ShouldReturnError_WhenStepIdDoesNotMatch()
    {
        // Arrange
        DeleteStepCommand command = new DeleteStepCommand { StepId = 1 };
        Step step = new Step( 2, "Description", 1 ); 
        _stepRepositoryMock.Setup( repo => repo.GetByStepIdAsync( command.StepId ) )
            .ReturnsAsync( step );
        _validatorMock.Setup( r => r.ValidateAsync( command ) ).ReturnsAsync( Result.FromError( "ID шага не соответствует указанному номеру шага" ) );
        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "ID шага не соответствует указанному номеру шага", result.Error.Message );
    }

    [Fact]
    public async Task HandleImplAsync_ShouldDeleteStep_WhenStepIdIsValid()
    {
        // Arrange
        DeleteStepCommand command = new DeleteStepCommand { StepId = 1 };
        Step step = new Step( 1, "Description", 1 ); 
        _stepRepositoryMock.Setup( repo => repo.GetByStepIdAsync( command.StepId ) )
            .ReturnsAsync( step );
        _stepRepositoryMock.Setup( repo => repo.Delete( step ) )
            .Returns( Task.CompletedTask );
        _validatorMock.Setup( r => r.ValidateAsync( command ) ).ReturnsAsync( Result.FromSuccess );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
        _stepRepositoryMock.Verify( repo => repo.Delete( step ), Times.Never );
    }
}