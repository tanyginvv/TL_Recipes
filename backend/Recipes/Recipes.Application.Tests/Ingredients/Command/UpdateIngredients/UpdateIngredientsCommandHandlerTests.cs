using Moq;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Ingredients.Commands.CreateIngredient;
using Recipes.Application.UseCases.Ingredients.Commands.DeleteIngredient;
using Recipes.Application.UseCases.Ingredients.Commands.UpdateIngredient;
using Recipes.Application.UseCases.Ingredients.Commands.UpdateIngredients;
using Recipes.Application.UseCases.Recipes.Dtos;
using Recipes.Domain.Entities;

namespace Recipes.Application.Tests.Ingredients.Command.UpdatesIngredient;

public class UpdateIngredientsCommandHandlerTests
{
    private readonly Mock<ICommandHandler<UpdateIngredientCommand>> _updateIngredientCommandHandlerMock;
    private readonly Mock<ICommandHandler<DeleteIngredientCommand>> _deleteIngredientCommandHandlerMock;
    private readonly Mock<ICommandHandlerWithResult<CreateIngredientCommand, Ingredient>> _createIngredientCommandHandlerMock;
    private readonly Mock<IAsyncValidator<UpdateIngredientsCommand>> _validatorMock;
    private readonly UpdateIngredientsCommandHandler _handler;

    public UpdateIngredientsCommandHandlerTests()
    {
        _updateIngredientCommandHandlerMock = new Mock<ICommandHandler<UpdateIngredientCommand>>();
        _deleteIngredientCommandHandlerMock = new Mock<ICommandHandler<DeleteIngredientCommand>>();
        _createIngredientCommandHandlerMock = new Mock<ICommandHandlerWithResult<CreateIngredientCommand, Ingredient>>();
        _validatorMock = new Mock<IAsyncValidator<UpdateIngredientsCommand>>();
        _handler = new UpdateIngredientsCommandHandler(
            _updateIngredientCommandHandlerMock.Object,
            _deleteIngredientCommandHandlerMock.Object,
            _createIngredientCommandHandlerMock.Object,
            _validatorMock.Object
        );
    }

    [Fact]
    public async Task HandleImplAsync_Should_Create_NewIngredients_When_NotExist()
    {
        // Arrange
        UpdateIngredientsCommand command = new UpdateIngredientsCommand
        {
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1, Ingredients = new List<Ingredient>() },
            NewIngredients = new List<IngredientDto>
            {
                new IngredientDto { Title = "New Ingredient", Description = "Description" }
            }
        };

        _createIngredientCommandHandlerMock
            .Setup( x => x.HandleAsync( It.IsAny<CreateIngredientCommand>() ) )
            .ReturnsAsync( Result<Ingredient>.FromSuccess( new Ingredient( "", "", 1 ) ) );

        _validatorMock
           .Setup( x => x.ValidateAsync( command ) )
           .ReturnsAsync( Result.Success );
        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        _createIngredientCommandHandlerMock.Verify( x => x.HandleAsync( It.IsAny<CreateIngredientCommand>() ), Times.Once );
        Assert.True( result.IsSuccess );
    }

    [Fact]
    public async Task HandleImplAsync_Should_Create_Multiple_NewIngredients()
    {
        // Arrange
        UpdateIngredientsCommand command = new UpdateIngredientsCommand
        {
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1, Ingredients = new List<Ingredient>() },
            NewIngredients = new List<IngredientDto>
            {
                new IngredientDto { Title = "New Ingredient 1", Description = "Description 1" },
                new IngredientDto { Title = "New Ingredient 2", Description = "Description 2" }
            }
        };

        _createIngredientCommandHandlerMock
            .Setup( x => x.HandleAsync( It.IsAny<CreateIngredientCommand>() ) )
            .ReturnsAsync( Result<Ingredient>.FromSuccess( new Ingredient( "", "", 1 ) ) );

        _validatorMock
           .Setup( x => x.ValidateAsync( command ) )
           .ReturnsAsync( Result.Success );
        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        _createIngredientCommandHandlerMock.Verify( x => x.HandleAsync( It.IsAny<CreateIngredientCommand>() ), Times.Exactly( 2 ) );
        Assert.True( result.IsSuccess );
    }

    [Fact]
    public async Task HandleImplAsync_Should_Update_ExistingIngredients_When_DescriptionChanged()
    {
        // Arrange
        Ingredient existingIngredient = new Ingredient( "", "", 1 ) { Id = 1, Title = "Existing Ingredient", Description = "Old Description" };
        UpdateIngredientsCommand command = new UpdateIngredientsCommand
        {
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1, Ingredients = new List<Ingredient> { existingIngredient } },
            NewIngredients = new List<IngredientDto>
            {
                new IngredientDto { Title = "Existing Ingredient", Description = "New Description" }
            }
        };

        _updateIngredientCommandHandlerMock
            .Setup( x => x.HandleAsync( It.IsAny<UpdateIngredientCommand>() ) )
            .ReturnsAsync( Result.Success );

        _validatorMock
           .Setup( x => x.ValidateAsync( command ) )
           .ReturnsAsync( Result.Success );
        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        _updateIngredientCommandHandlerMock.Verify( x => x.HandleAsync( It.IsAny<UpdateIngredientCommand>() ), Times.Once );
        Assert.True( result.IsSuccess );
    }

    [Fact]
    public async Task HandleImplAsync_Should_Update_Multiple_ExistingIngredients()
    {
        // Arrange
        List<Ingredient> existingIngredients = new List<Ingredient>
        {
            new Ingredient( "", "", 1 ) { Id = 1, Title = "Ingredient 1", Description = "Old Description 1" },
            new Ingredient( "", "", 1 ) { Id = 2, Title = "Ingredient 2", Description = "Old Description 2" }
        };
        UpdateIngredientsCommand command = new UpdateIngredientsCommand
        {
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1, Ingredients = existingIngredients },
            NewIngredients = new List<IngredientDto>
            {
                new IngredientDto { Title = "Ingredient 1", Description = "New Description 1" },
                new IngredientDto { Title = "Ingredient 2", Description = "New Description 2" }
            }
        };

        _updateIngredientCommandHandlerMock
            .Setup( x => x.HandleAsync( It.IsAny<UpdateIngredientCommand>() ) )
            .ReturnsAsync( Result.Success );

        _validatorMock
           .Setup( x => x.ValidateAsync( command ) )
           .ReturnsAsync( Result.Success );
        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        _updateIngredientCommandHandlerMock.Verify( x => x.HandleAsync( It.IsAny<UpdateIngredientCommand>() ), Times.Exactly( 2 ) );
        Assert.True( result.IsSuccess );
    }

    [Fact]
    public async Task HandleImplAsync_Should_Delete_Ingredients_That_Are_No_Long_Apart()
    {
        // Arrange
        Ingredient existingIngredient = new Ingredient( "", "", 1 ) { Id = 1, Title = "To Be Deleted" };
        UpdateIngredientsCommand command = new UpdateIngredientsCommand
        {
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1, Ingredients = new List<Ingredient> { existingIngredient } },
            NewIngredients = new List<IngredientDto>()
        };

        _deleteIngredientCommandHandlerMock
            .Setup( x => x.HandleAsync( It.IsAny<DeleteIngredientCommand>() ) )
            .ReturnsAsync( Result.Success );

        _validatorMock
           .Setup( x => x.ValidateAsync( command ) )
           .ReturnsAsync( Result.Success );
        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        _deleteIngredientCommandHandlerMock.Verify( x => x.HandleAsync( It.IsAny<DeleteIngredientCommand>() ), Times.Once );
        Assert.True( result.IsSuccess );
    }

    [Fact]
    public async Task HandleImplAsync_Should_Handle_No_Changes()
    {
        // Arrange
        Ingredient existingIngredient = new Ingredient( "", "", 1 ) { Id = 1, Title = "Existing Ingredient", Description = "Description" };
        UpdateIngredientsCommand command = new UpdateIngredientsCommand
        {
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1, Ingredients = new List<Ingredient> { existingIngredient } },
            NewIngredients = new List<IngredientDto>
            {
                new IngredientDto { Title = "Existing Ingredient", Description = "Description" }
            }
        };
        _validatorMock
           .Setup( x => x.ValidateAsync( command ) )
           .ReturnsAsync( Result.Success );
        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        _createIngredientCommandHandlerMock.Verify( x => x.HandleAsync( It.IsAny<CreateIngredientCommand>() ), Times.Never );
        _updateIngredientCommandHandlerMock.Verify( x => x.HandleAsync( It.IsAny<UpdateIngredientCommand>() ), Times.Never );
        _deleteIngredientCommandHandlerMock.Verify( x => x.HandleAsync( It.IsAny<DeleteIngredientCommand>() ), Times.Never );
        Assert.True( result.IsSuccess );
    }

    [Fact]
    public async Task HandleImplAsync_Should_Handle_Empty_NewIngredients_List()
    {
        // Arrange
        Ingredient existingIngredient = new Ingredient( "", "", 1 ) { Id = 1, Title = "Existing Ingredient", Description = "Description" };
        UpdateIngredientsCommand command = new UpdateIngredientsCommand
        {
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1, Ingredients = new List<Ingredient> { existingIngredient } },
            NewIngredients = new List<IngredientDto>()
        };

        _validatorMock
           .Setup( x => x.ValidateAsync( command ) )
           .ReturnsAsync( Result.Success );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        _createIngredientCommandHandlerMock.Verify( x => x.HandleAsync( It.IsAny<CreateIngredientCommand>() ), Times.Never );
        _updateIngredientCommandHandlerMock.Verify( x => x.HandleAsync( It.IsAny<UpdateIngredientCommand>() ), Times.Never );
        _deleteIngredientCommandHandlerMock.Verify( x => x.HandleAsync( It.IsAny<DeleteIngredientCommand>() ), Times.Once );
        Assert.True( result.IsSuccess );
    }

    [Fact]
    public async Task HandleImplAsync_Should_Handle_Validation_Failure()
    {
        // Arrange
        UpdateIngredientsCommand command = new UpdateIngredientsCommand
        {
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1, Ingredients = new List<Ingredient>() },
            NewIngredients = new List<IngredientDto>
            {
                new IngredientDto { Title = "Invalid Ingredient", Description = "Description" }
            }
        };

        _validatorMock
            .Setup( x => x.ValidateAsync( command ) )
            .ReturnsAsync( Result.FromError( "Validation failed" ) );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Validation failed", result.Error.Message );
        _createIngredientCommandHandlerMock.Verify( x => x.HandleAsync( It.IsAny<CreateIngredientCommand>() ), Times.Never );
        _updateIngredientCommandHandlerMock.Verify( x => x.HandleAsync( It.IsAny<UpdateIngredientCommand>() ), Times.Never );
        _deleteIngredientCommandHandlerMock.Verify( x => x.HandleAsync( It.IsAny<DeleteIngredientCommand>() ), Times.Never );
    }

    [Fact]
    public async Task HandleImplAsync_Should_Handle_Validation_Success()
    {
        // Arrange
        UpdateIngredientsCommand command = new UpdateIngredientsCommand
        {
            Recipe = new Recipe( 1, "", "", 1, 1, "" ) { Id = 1, Ingredients = new List<Ingredient>() },
            NewIngredients = new List<IngredientDto>
            {
                new IngredientDto { Title = "Valid Ingredient", Description = "Description" }
            }
        };

        _validatorMock
            .Setup( x => x.ValidateAsync( command ) )
            .ReturnsAsync( Result.Success );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
        _createIngredientCommandHandlerMock.Verify( x => x.HandleAsync( It.IsAny<CreateIngredientCommand>() ), Times.Once );
        _updateIngredientCommandHandlerMock.Verify( x => x.HandleAsync( It.IsAny<UpdateIngredientCommand>() ), Times.Never );
        _deleteIngredientCommandHandlerMock.Verify( x => x.HandleAsync( It.IsAny<DeleteIngredientCommand>() ), Times.Never );
    }
}
