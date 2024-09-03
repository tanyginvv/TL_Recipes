using Microsoft.AspNetCore.Mvc;
using Recipes.Application.CQRSInterfaces;
using Recipes.Application.Results;
using Recipes.Application.UseCases.Likes.Command.CreateLike;
using Recipes.Application.UseCases.Likes.Command.DeleteLike;
using Recipes.WebApi.Extensions;
using Recipes.WebApi.JwtAuthorization;

namespace Recipes.WebApi.Controllers;

[ApiController]
[Route( "api/likes" )]
public class LikesController : ControllerBase
{
    [JwtAuthorization]
    [HttpPost( "{recipeId}" )]
    public async Task<ActionResult<Result>> CreateLike(
        [FromRoute] int recipeId,
        [FromServices] ICommandHandler<CreateLikeCommand> commandHandler )
    {
        int userId = HttpContext.GetUserIdFromAccessToken();
        CreateLikeCommand like = new()
        {
            UserId = userId,
            RecipeId = recipeId
        };

        Result result = await commandHandler.HandleAsync( like );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }

        return Ok();
    }

    [JwtAuthorization]
    [HttpDelete( "{recipeId}" )]
    public async Task<ActionResult<Result>> DeleteLike(
        [FromRoute] int recipeId,
        [FromServices] ICommandHandler<DeleteLikeCommand> command )
    {
        int userId = HttpContext.GetUserIdFromAccessToken();
        DeleteLikeCommand deleteLike = new()
        {
            UserId = userId,
            RecipeId = recipeId
        };

        Result result = await command.HandleAsync( deleteLike );

        if ( !result.IsSuccess )
        {
            return NotFound( "Like not found." );
        }

        return Ok();
    }
}