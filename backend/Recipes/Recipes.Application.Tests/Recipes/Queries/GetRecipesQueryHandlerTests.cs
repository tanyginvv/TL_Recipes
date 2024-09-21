using Moq;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Results;
using Recipes.Application.Repositories;
using Recipes.Application.UseCases.Recipes.Queries.GetRecipes;
using Recipes.Domain.Entities;
using Recipes.Application.Filters;
using Recipes.Application.UseCases.Recipes.Dtos;
using Microsoft.Extensions.Logging;

namespace Recipes.Application.Tests.Recipes.Queries;

public class GetRecipesQueryHandlerTests
{
    private readonly Mock<IRecipeRepository> _recipeRepositoryMock;
    private readonly Mock<IAsyncValidator<GetRecipesQuery>> _validatorMock;
    private readonly GetRecipesQueryHandler _handler;
    private readonly Mock<ILogger<GetRecipesQuery>> _logger;
    public GetRecipesQueryHandlerTests()
    {
        _recipeRepositoryMock = new Mock<IRecipeRepository>();
        _validatorMock = new Mock<IAsyncValidator<GetRecipesQuery>>();
        _logger = new Mock<ILogger<GetRecipesQuery>>();
        _handler = new GetRecipesQueryHandler(
            _recipeRepositoryMock.Object,
            _validatorMock.Object,
            _logger.Object
        );
    }

    [Fact]
    public async Task HandleAsync_ValidQuery_ReturnsRecipesWithLikeAndFavouriteStatus()
    {
        // Arrange
        GetRecipesQuery query = new GetRecipesQuery
        {
            UserId = 1,
            SearchTerms = new List<string> { "test" },
            PageNumber = 1,
            RecipeQueryType = RecipeQueryType.All
        };

        List<Recipe> recipes = new List<Recipe>
        {
            new Recipe(1, "", "", 1, 1, "")
            {
                Id = 1,
                Likes = new List<Like> { new Like(1, 1) { UserId = 1 } },
                Favourites = new List<Favourite> { new Favourite(1, 1) { UserId = 1 } }
            },
            new Recipe(1, "", "", 1, 1, "")
            {
                Id = 2,
                Likes = new List<Like>(),
                Favourites = new List<Favourite>()
            }
        };

        _recipeRepositoryMock
            .Setup( x => x.GetRecipesAsync( It.IsAny<List<IFilter<Recipe>>>() ) )
            .ReturnsAsync( recipes );

        _recipeRepositoryMock
            .Setup( x => x.AnyAsync( It.IsAny<List<IFilter<Recipe>>>() ) )
            .ReturnsAsync( false );

        _validatorMock
            .Setup( x => x.ValidateAsync( query ) )
            .ReturnsAsync( Result.Success );

        // Act
        Result<GetRecipesListDto> result = await _handler.HandleAsync( query );

        // Assert
        Assert.True( result.IsSuccess );
        GetRecipesListDto dto = result.Value;

        Assert.Equal( 2, dto.GetRecipePartDtos.Count() );
        GetRecipePartDto firstRecipeDto = dto.GetRecipePartDtos.First();
        Assert.True( firstRecipeDto.IsLiked );
        Assert.True( firstRecipeDto.IsFavourited );

        GetRecipePartDto secondRecipeDto = dto.GetRecipePartDtos.Last();
        Assert.False( secondRecipeDto.IsLiked );
        Assert.False( secondRecipeDto.IsFavourited );
    }

    [Fact]
    public async Task HandleAsync_ValidationFails_ReturnsError()
    {
        // Arrange
        GetRecipesQuery query = new GetRecipesQuery
        {
            UserId = 1,
            SearchTerms = new List<string> { "test" },
            PageNumber = 1,
            RecipeQueryType = RecipeQueryType.All
        };

        _validatorMock
            .Setup( x => x.ValidateAsync( query ) )
            .ReturnsAsync( Result.FromError( "Validation failed" ) );

        // Act
        Result<GetRecipesListDto> result = await _handler.HandleAsync( query );

        // Assert
        Assert.False( result.IsSuccess );
        Assert.Equal( "Validation failed", result.Error.Message );
        _recipeRepositoryMock.Verify( x => x.GetRecipesAsync( It.IsAny<List<IFilter<Recipe>>>() ), Times.Never );
    }

    [Fact]
    public async Task HandleAsync_NoRecipesFound_ReturnsEmptyList()
    {
        // Arrange
        GetRecipesQuery query = new GetRecipesQuery
        {
            UserId = 1,
            SearchTerms = new List<string> { "test" },
            PageNumber = 1,
            RecipeQueryType = RecipeQueryType.All
        };

        _recipeRepositoryMock
            .Setup( x => x.GetRecipesAsync( It.IsAny<List<IFilter<Recipe>>>() ) )
            .ReturnsAsync( new List<Recipe>() );

        _recipeRepositoryMock
            .Setup( x => x.AnyAsync( It.IsAny<List<IFilter<Recipe>>>() ) )
            .ReturnsAsync( false );

        _validatorMock
            .Setup( x => x.ValidateAsync( query ) )
            .ReturnsAsync( Result.Success );

        // Act
        Result<GetRecipesListDto> result = await _handler.HandleAsync( query );

        // Assert
        Assert.True( result.IsSuccess );
        GetRecipesListDto dto = result.Value;
        Assert.Empty( dto.GetRecipePartDtos );
        Assert.False( dto.IsNextPageAvailable );
    }
}