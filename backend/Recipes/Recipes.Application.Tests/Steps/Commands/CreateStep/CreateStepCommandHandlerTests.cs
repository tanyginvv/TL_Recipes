using Microsoft.Extensions.Logging;
using Moq;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Steps.Commands.CreateStep;
using Recipes.Domain.Entities;

namespace Recipes.Application.Tests.Steps.Commands.CreateStep;

public class CreateStepCommandHandlerTests
{
    private readonly Mock<IStepRepository> _stepRepositoryMock;
    private readonly Mock<IAsyncValidator<CreateStepCommand>> _validatorMock;
    private readonly CreateStepCommandHandler _handler;
    private readonly Mock<ILogger<CreateStepCommand>> _logger;
    public CreateStepCommandHandlerTests()
    {
        _stepRepositoryMock = new Mock<IStepRepository>();
        _validatorMock = new Mock<IAsyncValidator<CreateStepCommand>>();
        _logger = new Mock<ILogger<CreateStepCommand>>();
        _handler = new CreateStepCommandHandler( _stepRepositoryMock.Object, _validatorMock.Object, _logger.Object );
    }

    [Fact]
    public async Task HandleAsync_ShouldCreateAndAddStep_WhenCommandIsValid()
    {
        // Arrange
        CreateStepCommand command = new CreateStepCommand
        {
            StepNumber = 1,
            StepDescription = "Step description",
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 123 } 
        };

        _validatorMock.Setup( v => v.ValidateAsync( command ) )
            .ReturnsAsync( Result.Success );

        // Act
        Result<Step> result = await _handler.HandleAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
        Assert.NotNull( result.Value );
        Assert.Equal( command.StepNumber, result.Value.StepNumber );
        Assert.Equal( command.StepDescription, result.Value.StepDescription );
        Assert.Equal( command.Recipe.Id, result.Value.RecipeId );

        _stepRepositoryMock.Verify( repo => repo.AddAsync( It.Is<Step>( step =>
            step.StepNumber == command.StepNumber &&
            step.StepDescription == command.StepDescription &&
            step.RecipeId == command.Recipe.Id ) ), Times.Once );
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnError_WhenValidationFails()
    {
        // Arrange
        CreateStepCommand command = new CreateStepCommand
        {
            StepNumber = 1,
            StepDescription = "Step description",
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 123 }
        };

        _validatorMock.Setup( v => v.ValidateAsync( command ) )
            .ReturnsAsync( Result.FromError( "Validation error" ) );

        // Act
        Result<Step> result = await _handler.HandleAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Validation error", result.Error.Message );

        _stepRepositoryMock.Verify( repo => repo.AddAsync( It.IsAny<Step>() ), Times.Never );
    }
}