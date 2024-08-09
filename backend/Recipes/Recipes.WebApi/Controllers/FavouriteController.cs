using Infrastructure.JwtAuthorizations;
using Microsoft.AspNetCore.Mvc;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Favourites.Dtos;
using Recipes.Application.UseCases.Likes.Command;
using Recipes.Application.UseCases.Likes.Dtos;
using Recipes.Application.UseCases.Likes.Queries.GetLikeBoolRecipeAndUser;
using Recipes.Application.UseCases.Likes.Queries.GetLikesCountForRecipeQuery;
namespace Recipes.WebApi.Controllers;

[ApiController]
[Route( "api/favourites" )]
public class FavouriteController : ControllerBase
{
    [JwtAuthorization]
    [HttpPost( "{userId}/{recipeId}" )]
    public async Task<IActionResult> CreateFavourite(
        [FromRoute] int userId,
        [FromRoute] int recipeId,
        [FromServices] ICommandHandler<CreateFavouriteCommand> commandHandler )
    {
        CreateFavouriteCommand favourite = new()
        {
            UserId = userId,
            RecipeId = recipeId
        };

        Result result = await commandHandler.HandleAsync( favourite );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        return Ok();
    }

    [JwtAuthorization]
    [HttpDelete( "{userId}/{recipeId}" )]
    public async Task<IActionResult> DeleteFavourite(
        [FromRoute] int userId,
        [FromRoute] int recipeId,
        [FromServices] ICommandHandler<DeleteFavouriteCommand> command )
    {
        DeleteFavouriteCommand deleteFavourite = new()
        {
            UserId = userId,
            RecipeId = recipeId
        };

        Result result = await command.HandleAsync( deleteFavourite );

        if ( !result.IsSuccess )
        {
            return NotFound( "Favourite not found." );
        }

        return NoContent();
    }

    [HttpGet( "count/{recipeId}" )]
    public async Task<IActionResult> GetFavouritesCount(
        [FromRoute] int recipeId,
        [FromServices] IQueryHandler<FavouritesCountDto, GetFavouritesCountForRecipeQuery> query )
    {
        GetFavouritesCountForRecipeQuery queryForRecipeQuery = new()
        {
            RecipeId = recipeId
        };
        Result<FavouritesCountDto> count = await query.HandleAsync( queryForRecipeQuery );

        if ( !count.IsSuccess )
        {
            return BadRequest();
        }
        return Ok( new { Count = count.Value } );
    }

    [HttpGet( "{userId}/{recipeId}" )]
    public async Task<IActionResult> GetFavouriteBool(
        [FromRoute] int userId,
        [FromRoute] int recipeId,
        [FromServices] IQueryHandler<FavouriteBoolDto, GetFavouriteBoolRecipeAndUserQuery> query )
    {
        GetFavouriteBoolRecipeAndUserQuery queryForRecipeQuery = new()
        {
            RecipeId = recipeId,
            UserId = userId
        };
        Result<FavouriteBoolDto> result = await query.HandleAsync( queryForRecipeQuery );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }
        return Ok( new { result.Value } );
    }
}
