using Microsoft.AspNetCore.Mvc;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Favourites.Command.CreateFavourite;
using Recipes.Application.UseCases.Favourites.Command.DeleteFavourite;
using Recipes.WebApi.Extensions;
using Recipes.WebApi.JwtAuthorization;
namespace Recipes.WebApi.Controllers;

[ApiController]
[Route( "api/favourites" )]
public class FavouritesController : ControllerBase
{
    [JwtAuthorization]
    [HttpPost( "{recipeId}" )]
    public async Task<ActionResult<Result>> CreateFavourite(
        [FromRoute] int recipeId,
        [FromServices] ICommandHandler<CreateFavouriteCommand> commandHandler )
    {
        int userId = HttpContext.GetUserIdFromAccessToken();
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
    [HttpDelete( "{recipeId}" )]
    public async Task<ActionResult<Result>> DeleteFavourite(
        [FromRoute] int recipeId,
        [FromServices] ICommandHandler<DeleteFavouriteCommand> command )
    {
        int userId = HttpContext.GetUserIdFromAccessToken();
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

        return Ok();
    }
}