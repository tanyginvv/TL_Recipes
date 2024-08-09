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
[Route( "api/likes" )]
public class LikeController : ControllerBase
{
    [JwtAuthorization]
    [HttpPost( "{userId}/{recipeId}" )]
    public async Task<IActionResult> CreateLike(
        [FromRoute] int userId,
        [FromRoute] int recipeId,
        [FromServices] ICommandHandler<CreateLikeCommand> commandHandler )
    {
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
    [HttpDelete( "{userId}/{recipeId}" )]
    public async Task<IActionResult> DeleteLike(
        [FromRoute] int userId,
        [FromRoute] int recipeId,
        [FromServices] ICommandHandler<DeleteLikeCommand> command )
    {
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

        return NoContent();
    }

    [HttpGet( "count/{recipeId}" )]
    public async Task<IActionResult> GetLikesCount(
        [FromRoute] int recipeId,
        [FromServices] IQueryHandler<LikesCountDto, GetLikesCountForRecipeQuery> query )
    {
        GetLikesCountForRecipeQuery queryForRecipeQuery = new()
        {
            RecipeId = recipeId
        };

        Result<LikesCountDto> count = await query.HandleAsync( queryForRecipeQuery );

        if ( !count.IsSuccess )
        {
            return BadRequest();
        }

        return Ok( new { Count = count.Value } );
    }

    [HttpGet( "{userId}/{recipeId}" )]
    public async Task<IActionResult> GetLikeBool(
        [FromRoute] int userId,
        [FromRoute] int recipeId,
        [FromServices] IQueryHandler<LikeBoolDto, GetLikeBoolRecipeAndUserQuery> query )
    {
        GetLikeBoolRecipeAndUserQuery queryForRecipeQuery = new()
        {
            RecipeId = recipeId,
            UserId = userId
        };
        Result<LikeBoolDto> result = await query.HandleAsync( queryForRecipeQuery );

        if ( !result.IsSuccess )
        {
            return BadRequest( result.Error );
        }
        return Ok( new { result.Value } );
    }
}
