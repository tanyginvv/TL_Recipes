using Moq;
using Recipes.Application.UseCases.Ingredients.Commands.DeleteIngredient;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Domain.Entities;
using Recipes.Application.CQRSInterfaces;

namespace Recipes.Application.Tests.UseCases.Ingredients.Commands;

public class DeleteIngredientCommandHandlerTests
{
    private readonly Mock<IIngredientRepository> _ingredientRepositoryMock;
    private readonly Mock<IAsyncValidator<DeleteIngredientCommand>> _validatorMock;
    private readonly DeleteIngredientCommandHandler _handler;

    public DeleteIngredientCommandHandlerTests()
    {
        _ingredientRepositoryMock = new Mock<IIngredientRepository>();
        _validatorMock = new Mock<IAsyncValidator<DeleteIngredientCommand>>();
        _handler = new DeleteIngredientCommandHandler( _ingredientRepositoryMock.Object, _validatorMock.Object );
    }

    [Fact]
    public async Task HandleImplAsync_IngredientExists_DeletesIngredientAndReturnsSuccess()
    {
        // Arrange
        DeleteIngredientCommand command = new DeleteIngredientCommand { Id = 1 };
        Ingredient ingredient = new Ingredient( "Test Ingredient", "Description", 1 );

        _ingredientRepositoryMock.Setup( repo => repo.GetByIdAsync( command.Id ) )
            .ReturnsAsync( ingredient );

        _ingredientRepositoryMock.Setup( repo => repo.Delete( ingredient ) )
            .Returns( Task.CompletedTask );

        _validatorMock.Setup( validator => validator.ValidateAsync( command ) )
            .ReturnsAsync( Result.Success );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.True( result.IsSuccess );
        _ingredientRepositoryMock.Verify( repo => repo.Delete( ingredient ), Times.Once );
    }

    [Fact]
    public async Task HandleImplAsync_IngredientDoesNotExist_ReturnsError()
    {
        // Arrange
        DeleteIngredientCommand command = new DeleteIngredientCommand { Id = 1 };

        _ingredientRepositoryMock.Setup( repo => repo.GetByIdAsync( command.Id ) )
            .ReturnsAsync( null as Ingredient );

        _validatorMock.Setup( validator => validator.ValidateAsync( command ) )
            .ReturnsAsync( Result.FromError( "Ингредиент не найден" ) );

        // Act
        Result result = await _handler.HandleAsync( command );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Ингредиент не найден", result.Error.Message );
        _ingredientRepositoryMock.Verify( repo => repo.Delete( It.IsAny<Ingredient>() ), Times.Never );
    }
}