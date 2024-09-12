﻿using Moq;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Results;
using Recipes.Application.Repositories;
using Recipes.Application.UseCases.Recipes.Queries.GetRecipes;
using Recipes.Domain.Entities;
using Recipes.Application.Filters;
using Recipes.Application.UseCases.Recipes.Dtos;

namespace Recipes.Application.Tests.UseCases.Recipes.Queries.GetRecipes;

public class GetRecipesQueryHandlerTests
{
    private readonly Mock<IRecipeRepository> _recipeRepositoryMock;
    private readonly Mock<IAsyncValidator<GetRecipesQuery>> _validatorMock;
    private readonly GetRecipesQueryHandler _handler;

    public GetRecipesQueryHandlerTests()
    {
        _recipeRepositoryMock = new Mock<IRecipeRepository>();
        _validatorMock = new Mock<IAsyncValidator<GetRecipesQuery>>();
        _handler = new GetRecipesQueryHandler(
            _recipeRepositoryMock.Object,
            _validatorMock.Object
        );
    }

    [Fact]
    public async Task HandleImplAsync_Should_Return_Recipes_With_Like_And_Favourite_Status()
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
            new Recipe
            {
                Id = 1,
                Likes = new List<Like> { new Like { UserId = 1 } },
                Favourites = new List<Favourite> { new Favourite { UserId = 1 } }
            },
            new Recipe
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

        // Using .Count() to get the number of items in the list
        Assert.Equal( 2, dto.GetRecipePartDtos.Count() );

        GetRecipePartDto firstRecipeDto = dto.GetRecipePartDtos.First();
        Assert.True( firstRecipeDto.IsLiked );
        Assert.True( firstRecipeDto.IsFavourited );

        GetRecipePartDto secondRecipeDto = dto.GetRecipePartDtos.Last();
        Assert.False( secondRecipeDto.IsLiked );
        Assert.False( secondRecipeDto.IsFavourited );
    }


    [Fact]
    public async Task HandleImplAsync_Should_Return_Error_When_Validation_Fails()
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
    public async Task HandleImplAsync_Should_Return_Empty_List_If_No_Recipes()
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