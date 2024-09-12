using Moq;
using Recipes.Application.Repositories;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Recipes.Queries.GetRecipeById;
using Recipes.Domain.Entities;

namespace Recipes.Application.Tests.Recipes.Queries.GetRecipeById;

public class GetRecipeByIdQueryValidatorTests
{
    private readonly Mock<IRecipeRepository> _mockRepository;
    private readonly GetRecipeByIdQueryValidator _validator;

    public GetRecipeByIdQueryValidatorTests()
    {
        _mockRepository = new Mock<IRecipeRepository>();
        _validator = new GetRecipeByIdQueryValidator( _mockRepository.Object );
    }

    [Fact]
    public async Task ValidateAsync_IdIsLessThanOrEqualToZero_ReturnsError()
    {
        // Arrange
        GetRecipeByIdQuery query = new GetRecipeByIdQuery { Id = 0, UserId = 0 };

        // Act
        Result result = await _validator.ValidateAsync( query );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Id рецепта меньше нуля", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_IdIsPositiveButRecipeNotFound_ReturnsError()
    {
        // Arrange
        GetRecipeByIdQuery query = new GetRecipeByIdQuery { Id = 1, UserId = 0 };
        _mockRepository.Setup( r => r.GetByIdAsync( query.Id ) ).ReturnsAsync( null as Recipe  );

        // Act
        Result result = await _validator.ValidateAsync( query );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Рецепт не найден", result.Error.Message );
    }

    [Fact]
    public async Task ValidateAsync_IdIsPositiveAndRecipeFound_ReturnsSuccess()
    {
        // Arrange
        GetRecipeByIdQuery query = new GetRecipeByIdQuery { Id = 1, UserId = 0 };
        Recipe recipe = new Recipe( query.Id, "Name", "Description", 30, 4, "image_url" );
        _mockRepository.Setup( r => r.GetByIdAsync( query.Id ) ).ReturnsAsync( recipe );

        // Act
        Result result = await _validator.ValidateAsync( query );

        // Assert
        Assert.True( result.IsSuccess );
    }
}