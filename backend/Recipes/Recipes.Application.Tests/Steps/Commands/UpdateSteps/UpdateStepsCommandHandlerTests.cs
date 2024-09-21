using Moq;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Steps.Commands.CreateStep;
using Recipes.Application.UseCases.Steps.Commands.DeleteStep;
using Recipes.Application.UseCases.Steps.Commands.UpdateStep;
using Recipes.Application.UseCases.Steps.Commands.UpdateSteps;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Recipes.Application.Tests.Steps.Commands.UpdateSteps;

public class UpdateStepsCommandHandlerTests
{
    private readonly Mock<ICommandHandler<UpdateStepCommand>> _updateStepCommandHandlerMock;
    private readonly Mock<ICommandHandler<DeleteStepCommand>> _deleteStepCommandHandlerMock;
    private readonly Mock<ICommandHandlerWithResult<CreateStepCommand, Step>> _createStepCommandHandlerMock;
    private readonly Mock<IAsyncValidator<UpdateStepsCommand>> _validatorMock;
    private readonly UpdateStepsCommandHandler _handler;
    private readonly Mock<ILogger<UpdateStepsCommand>> _logger;

    public UpdateStepsCommandHandlerTests()
    {
        _updateStepCommandHandlerMock = new Mock<ICommandHandler<UpdateStepCommand>>();
        _deleteStepCommandHandlerMock = new Mock<ICommandHandler<DeleteStepCommand>>();
        _createStepCommandHandlerMock = new Mock<ICommandHandlerWithResult<CreateStepCommand, Step>>();
        _logger = new Mock<ILogger<UpdateStepsCommand>>();
        _validatorMock = new Mock<IAsyncValidator<UpdateStepsCommand>>();
        _handler = new UpdateStepsCommandHandler(
            _updateStepCommandHandlerMock.Object,
            _deleteStepCommandHandlerMock.Object,
            _createStepCommandHandlerMock.Object,
            _validatorMock.Object,
            _logger.Object
        );
    }

    [Fact]
    public async Task HandleAsync_CreateNewStep_ShouldCreateStep()
    {
        // Arrange
        UpdateStepsCommand command = new UpdateStepsCommand
        {
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1, Steps = new List<Step>() },
            NewSteps = new List<StepDto>
            {
                new StepDto { StepNumber = 1, StepDescription = "New Step Description" }
            }
        };

        _createStepCommandHandlerMock
            .Setup( x => x.HandleAsync( It.IsAny<CreateStepCommand>() ) )
            .ReturnsAsync( Result<Step>.FromSuccess( new Step( 1, "", 1 ) ) );

        _validatorMock
            .Setup( x => x.ValidateAsync( command ) )
            .ReturnsAsync( Result.Success );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        _createStepCommandHandlerMock.Verify( x => x.HandleAsync( It.IsAny<CreateStepCommand>() ), Times.Once );
        Assert.True( result.IsSuccess );
    }

    [Fact]
    public async Task HandleAsync_CreateMultipleNewSteps_ShouldCreateSteps()
    {
        // Arrange
        UpdateStepsCommand command = new UpdateStepsCommand
        {
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1, Steps = new List<Step>() },
            NewSteps = new List<StepDto>
            {
                new StepDto { StepNumber = 1, StepDescription = "New Step 1 Description" },
                new StepDto { StepNumber = 2, StepDescription = "New Step 2 Description" }
            }
        };

        _createStepCommandHandlerMock
            .Setup( x => x.HandleAsync( It.IsAny<CreateStepCommand>() ) )
            .ReturnsAsync( Result<Step>.FromSuccess( new Step( 1, "", 1 ) ) );

        _validatorMock
            .Setup( x => x.ValidateAsync( command ) )
            .ReturnsAsync( Result.Success );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        _createStepCommandHandlerMock.Verify( x => x.HandleAsync( It.IsAny<CreateStepCommand>() ), Times.Exactly( 2 ) );
        Assert.True( result.IsSuccess );
    }

    [Fact]
    public async Task HandleAsync_UpdateExistingStepDescription_ShouldUpdateStep()
    {
        // Arrange
        Step existingStep = new Step( 1, "", 1 ) { Id = 1, StepNumber = 1, StepDescription = "Old Description" };
        UpdateStepsCommand command = new UpdateStepsCommand
        {
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1, Steps = new List<Step> { existingStep } },
            NewSteps = new List<StepDto>
            {
                new StepDto { StepNumber = 1, StepDescription = "Updated Description" }
            }
        };

        _updateStepCommandHandlerMock
            .Setup( x => x.HandleAsync( It.IsAny<UpdateStepCommand>() ) )
            .ReturnsAsync( Result.Success );

        _validatorMock
            .Setup( x => x.ValidateAsync( command ) )
            .ReturnsAsync( Result.Success );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        _updateStepCommandHandlerMock.Verify( x => x.HandleAsync( It.IsAny<UpdateStepCommand>() ), Times.Once );
        Assert.True( result.IsSuccess );
    }

    [Fact]
    public async Task HandleAsync_UpdateMultipleExistingSteps_ShouldUpdateSteps()
    {
        // Arrange
        List<Step> existingSteps = new List<Step>
        {
            new Step(1, "", 1) { Id = 1, StepNumber = 1, StepDescription = "Old Description 1" },
            new Step(1, "", 1) { Id = 2, StepNumber = 2, StepDescription = "Old Description 2" }
        };
        UpdateStepsCommand command = new UpdateStepsCommand
        {
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1, Steps = existingSteps },
            NewSteps = new List<StepDto>
            {
                new StepDto { StepNumber = 1, StepDescription = "New Description 1" },
                new StepDto { StepNumber = 2, StepDescription = "New Description 2" }
            }
        };

        _updateStepCommandHandlerMock
            .Setup( x => x.HandleAsync( It.IsAny<UpdateStepCommand>() ) )
            .ReturnsAsync( Result.Success );

        _validatorMock
            .Setup( x => x.ValidateAsync( command ) )
            .ReturnsAsync( Result.Success );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        _updateStepCommandHandlerMock.Verify( x => x.HandleAsync( It.IsAny<UpdateStepCommand>() ), Times.Exactly( 2 ) );
        Assert.True( result.IsSuccess );
    }

    [Fact]
    public async Task HandleAsync_DeleteStepsThatAreNoLongerPresent_ShouldDeleteSteps()
    {
        // Arrange
        Step existingStep = new Step( 1, "", 1 ) { Id = 1, StepNumber = 1, StepDescription = "To Be Deleted" };
        UpdateStepsCommand command = new UpdateStepsCommand
        {
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1, Steps = new List<Step> { existingStep } },
            NewSteps = new List<StepDto>()
        };

        _deleteStepCommandHandlerMock
            .Setup( x => x.HandleAsync( It.IsAny<DeleteStepCommand>() ) )
            .ReturnsAsync( Result.Success );

        _validatorMock
            .Setup( x => x.ValidateAsync( command ) )
            .ReturnsAsync( Result.Success );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        _deleteStepCommandHandlerMock.Verify( x => x.HandleAsync( It.IsAny<DeleteStepCommand>() ), Times.Once );
        Assert.True( result.IsSuccess );
    }

    [Fact]
    public async Task HandleAsync_NoChanges_ShouldNotCreateUpdateOrDeleteSteps()
    {
        // Arrange
        Step existingStep = new Step( 1, "", 1 ) { Id = 1, StepNumber = 1, StepDescription = "Description" };
        UpdateStepsCommand command = new UpdateStepsCommand
        {
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1, Steps = new List<Step> { existingStep } },
            NewSteps = new List<StepDto>
            {
                new StepDto { StepNumber = 1, StepDescription = "Description" }
            }
        };

        _validatorMock
            .Setup( x => x.ValidateAsync( command ) )
            .ReturnsAsync( Result.Success );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        _createStepCommandHandlerMock.Verify( x => x.HandleAsync( It.IsAny<CreateStepCommand>() ), Times.Never );
        _updateStepCommandHandlerMock.Verify( x => x.HandleAsync( It.IsAny<UpdateStepCommand>() ), Times.Never );
        _deleteStepCommandHandlerMock.Verify( x => x.HandleAsync( It.IsAny<DeleteStepCommand>() ), Times.Never );
        Assert.True( result.IsSuccess );
    }

    [Fact]
    public async Task HandleAsync_EmptyNewStepsList_ShouldNotCreateUpdateStepsButShouldDeleteSteps()
    {
        // Arrange
        Step existingStep = new Step( 1, "", 1 ) { Id = 1, StepNumber = 1, StepDescription = "Description" };
        UpdateStepsCommand command = new UpdateStepsCommand
        {
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1, Steps = new List<Step> { existingStep } },
            NewSteps = new List<StepDto>()
        };

        _validatorMock
            .Setup( x => x.ValidateAsync( command ) )
            .ReturnsAsync( Result.Success );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        _createStepCommandHandlerMock.Verify( x => x.HandleAsync( It.IsAny<CreateStepCommand>() ), Times.Never );
        _updateStepCommandHandlerMock.Verify( x => x.HandleAsync( It.IsAny<UpdateStepCommand>() ), Times.Never );
        _deleteStepCommandHandlerMock.Verify( x => x.HandleAsync( It.IsAny<DeleteStepCommand>() ), Times.Once );
        Assert.True( result.IsSuccess );
    }
}